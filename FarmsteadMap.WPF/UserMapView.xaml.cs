// <copyright file="UserMapView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Newtonsoft.Json;

    /// <summary>
    /// Логіка взаємодії для UserMapView.xaml (Редактор мапи).
    /// </summary>
    public partial class UserMapView : UserControl
    {
        private const double PixelsPerMeter = 100;
        private const double TargetScaleBarWidthPixels = 100;

        private readonly double[] niceScaleValues = { 0.1, 0.2, 0.5, 1, 2, 5, 10, 25, 50, 100, 250, 500, 1000 };
        private readonly Dictionary<string, Brush> landscapeBrushes = new Dictionary<string, Brush>();
        private readonly List<Point> drawAreaPoints = new List<Point>();
        private long currentMapId;
        private string? currentUsername;
        private double currentZoom = 1.0;
        private EditorTool currentTool = EditorTool.Pan;
        private EditorTool previousTool = EditorTool.Pan;
        private bool isMouseDown;
        private bool isDrawing;
        private Point mouseDownPos;
        private Point scrollViewerStartOffset;
        private Line? rulerLine;
        private TextBlock? rulerText;
        private string currentAreaType = string.Empty;
        private Polygon? drawAreaShape;
        private Line? drawAreaGhostLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapView"/> class.
        /// </summary>
        public UserMapView()
        {
            this.InitializeComponent();
            this.SetActiveTool(EditorTool.Pan);
            this.InitializeBrushes();
        }

        /// <summary>
        /// Подія, що запитує повернення до попереднього View.
        /// </summary>
        public event EventHandler? RequestNavigateBack;

        /// <summary>
        /// Подія, що запитує вихід з акаунту.
        /// </summary>
        public event EventHandler? RequestLogout;

        /// <summary>
        /// Завантажує дані мапи та ассетів.
        /// </summary>
        /// <param name="mapId">ID мапи для завантаження.</param>
        /// <param name="username">Ім'я поточного користувача.</param>
        public async void LoadMap(long mapId, string username)
        {
            this.currentMapId = mapId;
            this.currentUsername = username;
            this.WelcomeTextBlock.Text = $"Вітаємо, {username}!";

            // TODO: Завантажити дані мапи з BLL
            var mapData = new { Name = "Тестова мапа", MapJson = "{\"viewport\":{\"zoom\":1.0,\"offsetX\":0,\"offsetY\":0},\"objects\":[], \"landscape\":[]}" };
            this.MapNameTextBlock.Text = mapData.Name;

            await this.LoadToolbarAssets();

            MapScene? scene = null;
            try
            {
                if (!string.IsNullOrEmpty(mapData.MapJson))
                {
                    scene = JsonConvert.DeserializeObject<MapScene>(mapData.MapJson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження мапи: {ex.Message}");
            }

            scene ??= new MapScene();

            this.UpdateZoom(scene.Viewport.Zoom, force: true, isInitialLoad: true);
            this.EditorScrollViewer.ScrollToHorizontalOffset(scene.Viewport.OffsetX);
            this.EditorScrollViewer.ScrollToVerticalOffset(scene.Viewport.OffsetY);

            this.RenderScene(scene);
        }

        private void InitializeBrushes()
        {
            // TODO: Замінити прості кольори на ImageBrush / DrawingBrush
            this.landscapeBrushes["grass"] = Brushes.LightGreen;
            this.landscapeBrushes["soil"] = Brushes.Brown;
            this.landscapeBrushes["path"] = Brushes.Gray;
        }

        private async Task LoadToolbarAssets()
        {
            // TODO: Замінити на BLL-виклики
            this.TreesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Яблуня", Image = "/Images/apple_tree_icon.png", Type = "tree" } };
            this.VegetablesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Морква (грядка)", Image = "/Images/carrot_icon.png", Type = "veg" } };
            this.FlowersListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Троянди (клумба)", Image = "/Images/rose_icon.png", Type = "flower" } };
            this.BuildingsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Будинок", Image = "/Images/house_icon.png", Type = "building" } };
            this.DecorationsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Лавка", Image = "/Images/bench_icon.png", Type = "decoration" } };
            await Task.Delay(10);
        }

        private void RenderScene(MapScene scene)
        {
            this.ObjectCanvas.Children.Clear();
            this.LandscapeCanvas.Children.Clear();

            if (scene.Landscape != null)
            {
                foreach (var area in scene.Landscape)
                {
                    var shape = new Polygon
                    {
                        Points = new PointCollection(area.Points),
                        Stroke = Brushes.DarkGreen,
                        StrokeThickness = 1,
                        Tag = area,
                    };

                    if (this.landscapeBrushes.TryGetValue(area.AreaType, out var brush))
                    {
                        shape.Fill = brush;
                    }
                    else
                    {
                        shape.Fill = Brushes.Transparent;
                    }

                    this.LandscapeCanvas.Children.Add(shape);
                }
            }

            if (scene.Objects != null)
            {
                foreach (var obj in scene.Objects)
                {
                    // Логіка відтворення об'єктів (MapObject)
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var scene = new MapScene
            {
                Viewport =
                {
                    Zoom = this.currentZoom,
                    OffsetX = this.EditorScrollViewer.HorizontalOffset,
                    OffsetY = this.EditorScrollViewer.VerticalOffset,
                },
            };

            foreach (FrameworkElement element in this.LandscapeCanvas.Children)
            {
                if (element is Polygon polygon && polygon.Tag is LandscapeArea area)
                {
                    scene.Landscape.Add(area);
                }
            }

            foreach (FrameworkElement element in this.ObjectCanvas.Children)
            {
                if (element.Tag is MapObject obj)
                {
                    obj.X = Canvas.GetLeft(element);
                    obj.Y = Canvas.GetTop(element);
                    scene.Objects.Add(obj);
                }
            }

            string jsonMapData = JsonConvert.SerializeObject(scene, Formatting.Indented);

            MessageBox.Show($"Мапу збережено! (JSON: {jsonMapData.Length} байт)");
        }

        private void SetActiveTool(EditorTool tool)
        {
            if (this.isDrawing)
            {
                if (this.currentTool == EditorTool.DrawArea)
                {
                    this.FinishDrawingArea(false);
                }
                else if (this.currentTool == EditorTool.Ruler)
                {
                    this.ClearToolCanvas();
                }
            }

            this.previousTool = this.currentTool;
            this.currentTool = tool;

            this.ToolCanvas.Cursor = tool switch
            {
                EditorTool.Pan => Cursors.Hand,
                EditorTool.Ruler => Cursors.Cross,
                EditorTool.DrawArea => Cursors.Pen,
                _ => Cursors.Arrow,
            };
        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            this.SetActiveTool(EditorTool.Pan);
        }

        private void RulerTool_Click(object sender, RoutedEventArgs e)
        {
            this.SetActiveTool(EditorTool.Ruler);
        }

        private void DrawAreaTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            this.currentAreaType = button.Tag as string ?? string.Empty;
            this.SetActiveTool(EditorTool.DrawArea);
            this.StartNewDrawing();
        }

        private void ToolCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isMouseDown = true;
            this.mouseDownPos = e.GetPosition(this.ToolCanvas);

            switch (this.currentTool)
            {
                case EditorTool.Pan:
                    this.scrollViewerStartOffset = new Point(this.EditorScrollViewer.HorizontalOffset, this.EditorScrollViewer.VerticalOffset);
                    this.ToolCanvas.CaptureMouse();
                    this.ToolCanvas.Cursor = Cursors.SizeAll;
                    break;

                case EditorTool.Ruler:
                    this.isDrawing = true;
                    this.ClearToolCanvas();
                    this.rulerLine = new Line
                    {
                        X1 = this.mouseDownPos.X,
                        Y1 = this.mouseDownPos.Y,
                        X2 = this.mouseDownPos.X,
                        Y2 = this.mouseDownPos.Y,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2 / this.currentZoom,
                    };
                    this.rulerText = new TextBlock
                    {
                        Text = "0.00 m",
                        Foreground = Brushes.White,
                        Background = Brushes.Red,
                        Padding = new Thickness(3, 1, 3, 1),
                    };

                    this.ToolCanvas.Children.Add(this.rulerLine);
                    this.ToolCanvas.Children.Add(this.rulerText);
                    Canvas.SetLeft(this.rulerText, this.mouseDownPos.X + 5);
                    Canvas.SetTop(this.rulerText, this.mouseDownPos.Y + 5);
                    break;

                case EditorTool.DrawArea:
                    if (!this.isDrawing)
                    {
                        this.StartNewDrawing();
                    }

                    if (this.drawAreaShape != null && this.drawAreaGhostLine != null)
                    {
                        this.drawAreaPoints.Add(this.mouseDownPos);
                        this.drawAreaShape.Points.Add(this.mouseDownPos);
                        this.drawAreaGhostLine.Visibility = Visibility.Visible;
                    }

                    break;
            }
        }

        private void ToolCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(this.ToolCanvas);

            if (this.isMouseDown)
            {
                switch (this.currentTool)
                {
                    case EditorTool.Pan:
                        double deltaX = currentMousePos.X - this.mouseDownPos.X;
                        double deltaY = currentMousePos.Y - this.mouseDownPos.Y;
                        this.EditorScrollViewer.ScrollToHorizontalOffset(this.scrollViewerStartOffset.X - deltaX);
                        this.EditorScrollViewer.ScrollToVerticalOffset(this.scrollViewerStartOffset.Y - deltaY);
                        break;

                    case EditorTool.Ruler:
                        if (this.rulerLine != null)
                        {
                            this.rulerLine.X2 = currentMousePos.X;
                            this.rulerLine.Y2 = currentMousePos.Y;
                            this.UpdateRulerText(currentMousePos);
                        }

                        break;
                }
            }
            else
            {
                if (this.isDrawing && this.currentTool == EditorTool.DrawArea)
                {
                    this.UpdateGhostLine(currentMousePos);
                }
            }
        }

        private void ToolCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isMouseDown = false;

            switch (this.currentTool)
            {
                case EditorTool.Pan:
                    this.ToolCanvas.ReleaseMouseCapture();
                    this.ToolCanvas.Cursor = Cursors.Hand;
                    break;

                case EditorTool.Ruler:
                    this.isDrawing = false;
                    break;

                case EditorTool.DrawArea:
                    // Клік для додавання точки, а не відпускання
                    break;
            }
        }

        private void ToolCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.isDrawing && this.currentTool == EditorTool.DrawArea)
            {
                this.FinishDrawingArea(true);
                e.Handled = true;
            }
        }

        private void ClearToolCanvas()
        {
            this.ToolCanvas.Children.Clear();
            this.rulerLine = null;
            this.rulerText = null;
            this.drawAreaShape = null;
            this.drawAreaGhostLine = null;
        }

        private void UpdateRulerText(Point endPoint)
        {
            if (this.rulerLine == null || this.rulerText == null)
            {
                return;
            }

            double pixelDistance = Math.Sqrt(Math.Pow(endPoint.X - this.rulerLine.X1, 2) + Math.Pow(endPoint.Y - this.rulerLine.Y1, 2));
            double realMeters = pixelDistance / PixelsPerMeter;

            this.rulerText.Text = $"{realMeters:F2} m";
            Canvas.SetLeft(this.rulerText, endPoint.X + 5);
            Canvas.SetTop(this.rulerText, endPoint.Y + 5);
        }

        private void StartNewDrawing()
        {
            this.isDrawing = true;
            this.drawAreaPoints.Clear();
            this.ClearToolCanvas();

            Brush fillBrush = Brushes.Transparent;
            if (this.landscapeBrushes.TryGetValue(this.currentAreaType, out var brush))
            {
                fillBrush = brush.Clone();
            }

            this.drawAreaShape = new Polygon
            {
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 2 / this.currentZoom,
                StrokeDashArray = new DoubleCollection { 3, 2 },
                Fill = fillBrush,
                Opacity = 0.5,
            };

            this.drawAreaGhostLine = new Line
            {
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1 / this.currentZoom,
                StrokeDashArray = new DoubleCollection { 3, 2 },
                Visibility = Visibility.Collapsed,
            };

            this.ToolCanvas.Children.Add(this.drawAreaShape);
            this.ToolCanvas.Children.Add(this.drawAreaGhostLine);
        }

        private void UpdateGhostLine(Point mousePos)
        {
            if (this.drawAreaPoints.Count > 0 && this.drawAreaGhostLine != null)
            {
                Point lastPoint = this.drawAreaPoints.Last();
                this.drawAreaGhostLine.X1 = lastPoint.X;
                this.drawAreaGhostLine.Y1 = lastPoint.Y;
                this.drawAreaGhostLine.X2 = mousePos.X;
                this.drawAreaGhostLine.Y2 = mousePos.Y;
                this.drawAreaGhostLine.Visibility = Visibility.Visible;
            }
        }

        private void FinishDrawingArea(bool save)
        {
            if (save && this.drawAreaPoints.Count >= 3)
            {
                Brush fillBrush = Brushes.Transparent;
                if (this.landscapeBrushes.TryGetValue(this.currentAreaType, out var brush))
                {
                    fillBrush = brush;
                }

                var finalShape = new Polygon
                {
                    Points = new PointCollection(this.drawAreaPoints),
                    Stroke = Brushes.DarkGreen,
                    StrokeThickness = 1,
                    Fill = fillBrush,
                    Tag = new LandscapeArea
                    {
                        AreaType = this.currentAreaType,
                        Points = new List<Point>(this.drawAreaPoints),
                    },
                };

                this.LandscapeCanvas.Children.Add(finalShape);
            }

            this.isDrawing = false;
            this.drawAreaPoints.Clear();
            this.ClearToolCanvas();

            this.SetActiveTool(this.previousTool);
        }

        private void AssetList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                if (listBox.SelectedItem is not Asset asset)
                {
                    return;
                }

                this.SetActiveTool(EditorTool.PlaceAsset);
                DragDrop.DoDragDrop(listBox, asset, DragDropEffects.Copy);
                this.SetActiveTool(EditorTool.Pan);
            }
        }

        private void ToolCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void ToolCanvas_Drop(object sender, DragEventArgs e)
        {
            if (this.currentTool != EditorTool.PlaceAsset)
            {
                return;
            }

            if (e.Data.GetData(typeof(Asset)) is Asset asset)
            {
                Point position = e.GetPosition(this.ToolCanvas);

                var newElement = new Image
                {
                    Width = 100,
                    Height = 100,
                    Source = new ImageSourceConverter().ConvertFromString(asset.Image) as ImageSource,
                    Stretch = Stretch.Fill,
                };

                var newMapObject = new MapObject
                {
                    DbId = asset.Id,
                    Type = asset.Type,
                    X = position.X - 50,
                    Y = position.Y - 50,
                    Width = 100,
                    Height = 100,
                };
                newElement.Tag = newMapObject;

                this.ObjectCanvas.Children.Add(newElement);
                Canvas.SetLeft(newElement, newMapObject.X);
                Canvas.SetTop(newElement, newMapObject.Y);
            }
        }

        private void UpdateScaleBar(double currentZoom)
        {
            double realMetersInTargetWidth = TargetScaleBarWidthPixels / (PixelsPerMeter * currentZoom);
            double niceRealMeters = this.niceScaleValues.OrderBy(x => Math.Abs(x - realMetersInTargetWidth)).First();
            double finalPixelWidth = niceRealMeters * PixelsPerMeter * currentZoom;

            this.ScaleBar.Width = finalPixelWidth;
            this.ScaleBarLabel.Text = $"{niceRealMeters} m";
        }

        private void UpdateZoom(double newScale, bool force = false, bool isInitialLoad = false)
        {
            if (!this.IsLoaded && !force)
            {
                return;
            }

            this.currentZoom = Math.Clamp(newScale, 0.1, 2.0);

            this.CanvasScaleTransform.ScaleX = this.currentZoom;
            this.CanvasScaleTransform.ScaleY = this.currentZoom;

            this.UpdateScaleBar(this.currentZoom);
        }

        private void ToolCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = 1.2;
            if (e.Delta < 0)
            {
                this.UpdateZoom(this.currentZoom / zoomFactor);
            }
            else
            {
                this.UpdateZoom(this.currentZoom * zoomFactor);
            }

            e.Handled = true;
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateZoom(this.currentZoom * 1.5);
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateZoom(this.currentZoom / 1.5);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.RequestNavigateBack?.Invoke(this, EventArgs.Empty);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.RequestLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}