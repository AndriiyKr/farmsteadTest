<<<<<<< HEAD
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
=======
using Newtonsoft.Json;
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

namespace FarmsteadMap.WPF
{
    public enum EditorTool
    {
        Select,       
        Pan,          
        PlaceAsset,   
        DrawArea,     
        Ruler         
    }

    public partial class UserMapView : UserControl
    {
        public event EventHandler RequestNavigateBack;
        public event EventHandler RequestLogout;

        private long _currentMapId;
        private string _currentUsername;

        private const double PixelsPerMeter = 100; 
        private readonly double[] _niceScaleValues = { 0.1, 0.2, 0.5, 1, 2, 5, 10, 25, 50, 100, 250, 500, 1000 };
        private const double TargetScaleBarWidthPixels = 100;
        private double _currentZoom = 1.0;

        private EditorTool _currentTool = EditorTool.Pan; 
        private EditorTool _previousTool = EditorTool.Pan;
        private bool _isMouseDown = false;
        private bool _isDrawing = false; 
        private Point _mouseDownPos; 

        private Point _scrollViewerStartOffset;

        private Line _rulerLine;
        private TextBlock _rulerText;

        private string _currentAreaType; 
        private Polygon _drawAreaShape; 
        private Line _drawAreaGhostLine; 
        private List<Point> _drawAreaPoints = new List<Point>();

        private Dictionary<string, Brush> _landscapeBrushes = new Dictionary<string, Brush>();


        public class Asset { public long Id { get; set; } public string Name { get; set; } public string Image { get; set; } public string Type { get; set; } }

        public UserMapView()
        {
            InitializeComponent();
            SetActiveTool(EditorTool.Pan);
            InitializeBrushes();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void InitializeBrushes()
        {
            // TODO: Замінити прості кольори на ImageBrush / DrawingBrush
<<<<<<< HEAD
            this.landscapeBrushes["grass"] = Brushes.LightGreen;
            this.landscapeBrushes["soil"] = Brushes.Brown;
            this.landscapeBrushes["path"] = Brushes.Gray;
=======
            _landscapeBrushes["grass"] = Brushes.LightGreen;
            _landscapeBrushes["soil"] = Brushes.Brown;
            _landscapeBrushes["path"] = Brushes.Gray;
        }

        #region Завантаження та Збереження

        public async void LoadMap(long mapId, string username)
        {
            _currentMapId = mapId;
            _currentUsername = username;
            WelcomeTextBlock.Text = $"Вітаємо, {username}!";

            //  TODO: Завантажити дані мапи з BLL
            var mapData = new { Name = "Тестова мапа", MapJson = "{\"viewport\":{\"zoom\":1.0,\"offsetX\":0,\"offsetY\":0},\"objects\":[], \"landscape\":[]}" };
            MapNameTextBlock.Text = mapData.Name;

            await LoadToolbarAssets();

            MapScene scene = null;
            try
            {
                if (!string.IsNullOrEmpty(mapData.MapJson))
                    scene = JsonConvert.DeserializeObject<MapScene>(mapData.MapJson);
            }
            catch (Exception ex) { MessageBox.Show($"Помилка завантаження мапи: {ex.Message}"); }

            if (scene == null) scene = new MapScene();

            UpdateZoom(scene.viewport?.zoom ?? 1.0, force: true, isInitialLoad: true);
            EditorScrollViewer.ScrollToHorizontalOffset(scene.viewport.offsetX);
            EditorScrollViewer.ScrollToVerticalOffset(scene.viewport.offsetY);

            RenderScene(scene);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private async Task LoadToolbarAssets()
        {
            // TODO: Замінити на BLL-виклики
<<<<<<< HEAD
            this.TreesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Яблуня", Image = "/Images/apple_tree_icon.png", Type = "tree" } };
            this.VegetablesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Морква (грядка)", Image = "/Images/carrot_icon.png", Type = "veg" } };
            this.FlowersListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Троянди (клумба)", Image = "/Images/rose_icon.png", Type = "flower" } };
            this.BuildingsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Будинок", Image = "/Images/house_icon.png", Type = "building" } };
            this.DecorationsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Лавка", Image = "/Images/bench_icon.png", Type = "decoration" } };
=======
            TreesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Яблуня", Image = "/Images/apple_tree_icon.png", Type = "tree" } };
            VegetablesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Морква (грядка)", Image = "/Images/carrot_icon.png", Type = "veg" } };
            FlowersListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Троянди (клумба)", Image = "/Images/rose_icon.png", Type = "flower" } };
            BuildingsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Будинок", Image = "/Images/house_icon.png", Type = "building" } };
            DecorationsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Лавка", Image = "/Images/bench_icon.png", Type = "decoration" } };
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            await Task.Delay(10);
        }

        private void RenderScene(MapScene scene)
        {
<<<<<<< HEAD
            this.ObjectCanvas.Children.Clear();
            this.LandscapeCanvas.Children.Clear();

            if (scene.Landscape != null)
            {
                foreach (var area in scene.Landscape)
=======
            ObjectCanvas.Children.Clear();
            LandscapeCanvas.Children.Clear();

            if (scene.landscape != null)
            {
                foreach (var area in scene.landscape)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                {
                    var shape = new Polygon
                    {
                        Points = new PointCollection(area.Points),
<<<<<<< HEAD
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
=======
                        Stroke = Brushes.DarkGreen, 
                        StrokeThickness = 1,
                        Tag = area
                    };
                    shape.Fill = _landscapeBrushes.ContainsKey(area.AreaType) ? _landscapeBrushes[area.AreaType] : Brushes.Transparent;

                    LandscapeCanvas.Children.Add(shape);
                }
            }

            if (scene.objects != null)
            {
                foreach (var obj in scene.objects)
                {

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                }
            }
        }

<<<<<<< HEAD
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
=======
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MapScene scene = new MapScene();

            scene.viewport.zoom = _currentZoom;
            scene.viewport.offsetX = EditorScrollViewer.HorizontalOffset;
            scene.viewport.offsetY = EditorScrollViewer.VerticalOffset;

            foreach (FrameworkElement element in LandscapeCanvas.Children)
            {
                if (element is Polygon shape && shape.Tag is LandscapeArea area)
                {
                    scene.landscape.Add(area);
                }
            }

            foreach (FrameworkElement element in ObjectCanvas.Children)
            {
                if (element.Tag is MapObject obj)
                {
                    obj.x = Canvas.GetLeft(element);
                    obj.y = Canvas.GetTop(element);
                    scene.objects.Add(obj);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                }
            }

            string jsonMapData = JsonConvert.SerializeObject(scene, Formatting.Indented);

            MessageBox.Show($"Мапу збережено! (JSON: {jsonMapData.Length} байт)");
        }

<<<<<<< HEAD
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
=======
        #endregion

        #region Обробники Інструментів (Toolbar)

        private void SetActiveTool(EditorTool tool)
        {
            if (_isDrawing)
            {
                if (_currentTool == EditorTool.DrawArea)
                    FinishDrawingArea(false); 
                else if (_currentTool == EditorTool.Ruler)
                    ClearToolCanvas(); 
            }

            _previousTool = _currentTool;
            _currentTool = tool;

            switch (tool)
            {
                case EditorTool.Pan:
                    ToolCanvas.Cursor = Cursors.Hand;
                    break;
                case EditorTool.Ruler:
                    ToolCanvas.Cursor = Cursors.Cross;
                    break;
                case EditorTool.DrawArea:
                    ToolCanvas.Cursor = Cursors.Pen;
                    break;
                default:
                    ToolCanvas.Cursor = Cursors.Arrow;
                    break;
            }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.SetActiveTool(EditorTool.Pan);
=======
            SetActiveTool(EditorTool.Pan);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void RulerTool_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.SetActiveTool(EditorTool.Ruler);
=======
            SetActiveTool(EditorTool.Ruler);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void DrawAreaTool_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
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
=======
            var button = sender as Button;
            if (button == null) return;

            _currentAreaType = button.Tag as string; 
            SetActiveTool(EditorTool.DrawArea);
            StartNewDrawing();
        }

        #endregion

        #region Керування Мишею (Головна логіка State Machine)

        private void ToolCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _mouseDownPos = e.GetPosition(ToolCanvas);

            switch (_currentTool)
            {
                case EditorTool.Pan:
                    _scrollViewerStartOffset = new Point(EditorScrollViewer.HorizontalOffset, EditorScrollViewer.VerticalOffset);
                    ToolCanvas.CaptureMouse(); 

                    ToolCanvas.Cursor = Cursors.SizeAll; 

                    break;

                case EditorTool.Ruler:
                    _isDrawing = true;
                    ClearToolCanvas();
                    _rulerLine = new Line
                    {
                        X1 = _mouseDownPos.X,
                        Y1 = _mouseDownPos.Y,
                        X2 = _mouseDownPos.X,
                        Y2 = _mouseDownPos.Y,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2 / _currentZoom
                    };
                    _rulerText = new TextBlock
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                    {
                        Text = "0.00 m",
                        Foreground = Brushes.White,
                        Background = Brushes.Red,
<<<<<<< HEAD
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

=======
                        Padding = new Thickness(3, 1, 3, 1)
                    };

                    ToolCanvas.Children.Add(_rulerLine);
                    ToolCanvas.Children.Add(_rulerText);
                    Canvas.SetLeft(_rulerText, _mouseDownPos.X + 5);
                    Canvas.SetTop(_rulerText, _mouseDownPos.Y + 5);
                    break;

                case EditorTool.DrawArea:
                    if (!_isDrawing)
                    {
                        StartNewDrawing();
                    }
                    _drawAreaPoints.Add(_mouseDownPos);
                    _drawAreaShape.Points.Add(_mouseDownPos);
                    _drawAreaGhostLine.Visibility = Visibility.Visible;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                    break;
            }
        }

        private void ToolCanvas_MouseMove(object sender, MouseEventArgs e)
        {
<<<<<<< HEAD
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

=======
            Point currentMousePos = e.GetPosition(ToolCanvas);

            if (_isMouseDown)
            {
                switch (_currentTool)
                {
                    case EditorTool.Pan:
                        double deltaX = currentMousePos.X - _mouseDownPos.X;
                        double deltaY = currentMousePos.Y - _mouseDownPos.Y;
                        EditorScrollViewer.ScrollToHorizontalOffset(_scrollViewerStartOffset.X - deltaX);
                        EditorScrollViewer.ScrollToVerticalOffset(_scrollViewerStartOffset.Y - deltaY);
                        break;

                    case EditorTool.Ruler:
                        _rulerLine.X2 = currentMousePos.X;
                        _rulerLine.Y2 = currentMousePos.Y;
                        UpdateRulerText(currentMousePos);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                        break;
                }
            }
            else
            {
<<<<<<< HEAD
                if (this.isDrawing && this.currentTool == EditorTool.DrawArea)
                {
                    this.UpdateGhostLine(currentMousePos);
=======
                if (_isDrawing && _currentTool == EditorTool.DrawArea)
                {
                    UpdateGhostLine(currentMousePos);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                }
            }
        }

        private void ToolCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
<<<<<<< HEAD
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
=======
            _isMouseDown = false;

            switch (_currentTool)
            {
                case EditorTool.Pan:
                    ToolCanvas.ReleaseMouseCapture();
                    ToolCanvas.Cursor = Cursors.Hand;
                    break;

                case EditorTool.Ruler:
                    _isDrawing = false; 
                    break;

                case EditorTool.DrawArea:
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                    break;
            }
        }

        private void ToolCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
<<<<<<< HEAD
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
=======
            if (_isDrawing && _currentTool == EditorTool.DrawArea)
            {
                FinishDrawingArea(true); 
                e.Handled = true; 
            }
        }

        #endregion

        #region Логіка Інструментів (Допоміжні методи)

        private void ClearToolCanvas()
        {
            ToolCanvas.Children.Clear();
            _rulerLine = null;
            _rulerText = null;
            _drawAreaShape = null;
            _drawAreaGhostLine = null;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void UpdateRulerText(Point endPoint)
        {
<<<<<<< HEAD
            if (this.rulerLine == null || this.rulerText == null)
            {
                return;
            }

            double pixelDistance = Math.Sqrt(Math.Pow(endPoint.X - this.rulerLine.X1, 2) + Math.Pow(endPoint.Y - this.rulerLine.Y1, 2));
            double realMeters = pixelDistance / PixelsPerMeter;

            this.rulerText.Text = $"{realMeters:F2} m";
            Canvas.SetLeft(this.rulerText, endPoint.X + 5);
            Canvas.SetTop(this.rulerText, endPoint.Y + 5);
=======
            double pixelDistance = Math.Sqrt(Math.Pow(endPoint.X - _rulerLine.X1, 2) + Math.Pow(endPoint.Y - _rulerLine.Y1, 2));
            double realMeters = pixelDistance / PixelsPerMeter; 

            _rulerText.Text = $"{realMeters:F2} m"; 
            Canvas.SetLeft(_rulerText, endPoint.X + 5);
            Canvas.SetTop(_rulerText, endPoint.Y + 5);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void StartNewDrawing()
        {
<<<<<<< HEAD
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
=======
            _isDrawing = true;
            _drawAreaPoints.Clear();
            ClearToolCanvas();

            _drawAreaShape = new Polygon
            {
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 2 / _currentZoom,
                StrokeDashArray = new DoubleCollection { 3, 2 },
                Fill = _landscapeBrushes.ContainsKey(_currentAreaType) ?
                       _landscapeBrushes[_currentAreaType].Clone() : Brushes.Transparent,
                Opacity = 0.5
            };

            _drawAreaGhostLine = new Line
            {
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 1 / _currentZoom,
                StrokeDashArray = new DoubleCollection { 3, 2 },
                Visibility = Visibility.Collapsed
            };

            ToolCanvas.Children.Add(_drawAreaShape);
            ToolCanvas.Children.Add(_drawAreaGhostLine);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void UpdateGhostLine(Point mousePos)
        {
<<<<<<< HEAD
            if (this.drawAreaPoints.Count > 0 && this.drawAreaGhostLine != null)
            {
                Point lastPoint = this.drawAreaPoints.Last();
                this.drawAreaGhostLine.X1 = lastPoint.X;
                this.drawAreaGhostLine.Y1 = lastPoint.Y;
                this.drawAreaGhostLine.X2 = mousePos.X;
                this.drawAreaGhostLine.Y2 = mousePos.Y;
                this.drawAreaGhostLine.Visibility = Visibility.Visible;
=======
            if (_drawAreaPoints.Count > 0)
            {
                Point lastPoint = _drawAreaPoints.Last();
                _drawAreaGhostLine.X1 = lastPoint.X;
                _drawAreaGhostLine.Y1 = lastPoint.Y;
                _drawAreaGhostLine.X2 = mousePos.X;
                _drawAreaGhostLine.Y2 = mousePos.Y;
                _drawAreaGhostLine.Visibility = Visibility.Visible;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
        }

        private void FinishDrawingArea(bool save)
        {
<<<<<<< HEAD
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

=======
            if (save && _drawAreaPoints.Count >= 3)
            {
                var finalShape = new Polygon
                {
                    Points = new PointCollection(_drawAreaPoints),
                    Stroke = Brushes.DarkGreen, 
                    StrokeThickness = 1,
                    Fill = _landscapeBrushes.ContainsKey(_currentAreaType) ?
                           _landscapeBrushes[_currentAreaType] : Brushes.Transparent
                };

                finalShape.Tag = new LandscapeArea
                {
                    AreaType = _currentAreaType,
                    Points = _drawAreaPoints.ToList()
                };

                LandscapeCanvas.Children.Add(finalShape);
            }

            _isDrawing = false;
            _drawAreaPoints.Clear();
            ClearToolCanvas();

            SetActiveTool(_previousTool);
        }

        #endregion

        #region Drag-and-Drop (Розміщення Об'єктів)

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        private void AssetList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
<<<<<<< HEAD
                if (listBox.SelectedItem is not Asset asset)
                {
                    return;
                }

                this.SetActiveTool(EditorTool.PlaceAsset);
                DragDrop.DoDragDrop(listBox, asset, DragDropEffects.Copy);
                this.SetActiveTool(EditorTool.Pan);
=======
                var asset = listBox.SelectedItem as Asset;
                if (asset == null) return;

                SetActiveTool(EditorTool.PlaceAsset);
                DragDrop.DoDragDrop(listBox, asset, DragDropEffects.Copy);
                SetActiveTool(EditorTool.Pan);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
        }

        private void ToolCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void ToolCanvas_Drop(object sender, DragEventArgs e)
        {
<<<<<<< HEAD
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
=======
            if (_currentTool != EditorTool.PlaceAsset) return; 

            if (e.Data.GetData(typeof(Asset)) is Asset asset)
            {
                Point position = e.GetPosition(ToolCanvas); 

                var newElement = new Image
                {
                    Width = 100, 
                    Height = 100, 
                    Source = new ImageSourceConverter().ConvertFromString(asset.Image) as ImageSource,
                    Stretch = Stretch.Fill
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                };

                var newMapObject = new MapObject
                {
<<<<<<< HEAD
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
=======
                    db_id = asset.Id,
                    type = asset.Type,
                    x = position.X - 50, 
                    y = position.Y - 50, 
                    Width = 100,
                    Height = 100
                };
                newElement.Tag = newMapObject;

                ObjectCanvas.Children.Add(newElement);
                Canvas.SetLeft(newElement, newMapObject.x);
                Canvas.SetTop(newElement, newMapObject.y);
            }
        }

        #endregion

        #region Керування (Zoom, Навігація)

        private void UpdateScaleBar(double currentZoom)
        {
            double realMetersInTargetWidth = TargetScaleBarWidthPixels / (PixelsPerMeter * currentZoom);
            double niceRealMeters = _niceScaleValues.OrderBy(x => Math.Abs(x - realMetersInTargetWidth)).First();
            double finalPixelWidth = niceRealMeters * PixelsPerMeter * currentZoom;

            ScaleBar.Width = finalPixelWidth;
            ScaleBarLabel.Text = $"{niceRealMeters} m";
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void UpdateZoom(double newScale, bool force = false, bool isInitialLoad = false)
        {
<<<<<<< HEAD
            if (!this.IsLoaded && !force)
            {
                return;
            }

            this.currentZoom = Math.Clamp(newScale, 0.1, 2.0);

            this.CanvasScaleTransform.ScaleX = this.currentZoom;
            this.CanvasScaleTransform.ScaleY = this.currentZoom;

            this.UpdateScaleBar(this.currentZoom);
=======
            if (!this.IsLoaded && !force) return;

            _currentZoom = Math.Clamp(newScale, 0.1, 2.0); 

            CanvasScaleTransform.ScaleX = _currentZoom;
            CanvasScaleTransform.ScaleY = _currentZoom;

            UpdateScaleBar(_currentZoom);

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void ToolCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = 1.2;
            if (e.Delta < 0)
            {
<<<<<<< HEAD
                this.UpdateZoom(this.currentZoom / zoomFactor);
            }
            else
            {
                this.UpdateZoom(this.currentZoom * zoomFactor);
            }

=======
                UpdateZoom(_currentZoom / zoomFactor);
            }
            else
            {
                UpdateZoom(_currentZoom * zoomFactor);
            }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            e.Handled = true;
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.UpdateZoom(this.currentZoom * 1.5);
=======
            UpdateZoom(_currentZoom * 1.5); 
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.UpdateZoom(this.currentZoom / 1.5);
=======
            UpdateZoom(_currentZoom / 1.5);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.RequestNavigateBack?.Invoke(this, EventArgs.Empty);
=======
            RequestNavigateBack?.Invoke(this, EventArgs.Empty);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.RequestLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}
=======
            RequestLogout?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    #region Допоміжні класи для JSON (з Шарами)

    public class MapObject
    {
        public long db_id { get; set; }
        public string type { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double rotation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class LandscapeArea
    {
        public string AreaType { get; set; } 
        public List<Point> Points { get; set; }
    }

    public class Viewport
    {
        public double zoom { get; set; } = 1.0;
        public double offsetX { get; set; } = 0;
        public double offsetY { get; set; } = 0;
    }

    public class MapScene
    {
        public Viewport viewport { get; set; } = new Viewport();
        public List<MapObject> objects { get; set; } = new List<MapObject>();
        public List<LandscapeArea> landscape { get; set; } = new List<LandscapeArea>();
    }

    #endregion
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
