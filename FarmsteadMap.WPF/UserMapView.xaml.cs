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
        }

        private void InitializeBrushes()
        {
            // TODO: Замінити прості кольори на ImageBrush / DrawingBrush
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
        }

        private async Task LoadToolbarAssets()
        {
            // TODO: Замінити на BLL-виклики
            TreesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Яблуня", Image = "/Images/apple_tree_icon.png", Type = "tree" } };
            VegetablesListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Морква (грядка)", Image = "/Images/carrot_icon.png", Type = "veg" } };
            FlowersListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Троянди (клумба)", Image = "/Images/rose_icon.png", Type = "flower" } };
            BuildingsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Будинок", Image = "/Images/house_icon.png", Type = "building" } };
            DecorationsListBox.ItemsSource = new List<Asset> { new Asset { Id = 1, Name = "Лавка", Image = "/Images/bench_icon.png", Type = "decoration" } };
            await Task.Delay(10);
        }

        private void RenderScene(MapScene scene)
        {
            ObjectCanvas.Children.Clear();
            LandscapeCanvas.Children.Clear();

            if (scene.landscape != null)
            {
                foreach (var area in scene.landscape)
                {
                    var shape = new Polygon
                    {
                        Points = new PointCollection(area.Points),
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

                }
            }
        }

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
                }
            }

            string jsonMapData = JsonConvert.SerializeObject(scene, Formatting.Indented);

            MessageBox.Show($"Мапу збережено! (JSON: {jsonMapData.Length} байт)");
        }

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
        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool(EditorTool.Pan);
        }

        private void RulerTool_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTool(EditorTool.Ruler);
        }

        private void DrawAreaTool_Click(object sender, RoutedEventArgs e)
        {
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
                    {
                        Text = "0.00 m",
                        Foreground = Brushes.White,
                        Background = Brushes.Red,
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
                    break;
            }
        }

        private void ToolCanvas_MouseMove(object sender, MouseEventArgs e)
        {
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
                        break;
                }
            }
            else
            {
                if (_isDrawing && _currentTool == EditorTool.DrawArea)
                {
                    UpdateGhostLine(currentMousePos);
                }
            }
        }

        private void ToolCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
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
                    break;
            }
        }

        private void ToolCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
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
        }

        private void UpdateRulerText(Point endPoint)
        {
            double pixelDistance = Math.Sqrt(Math.Pow(endPoint.X - _rulerLine.X1, 2) + Math.Pow(endPoint.Y - _rulerLine.Y1, 2));
            double realMeters = pixelDistance / PixelsPerMeter; 

            _rulerText.Text = $"{realMeters:F2} m"; 
            Canvas.SetLeft(_rulerText, endPoint.X + 5);
            Canvas.SetTop(_rulerText, endPoint.Y + 5);
        }

        private void StartNewDrawing()
        {
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
        }

        private void UpdateGhostLine(Point mousePos)
        {
            if (_drawAreaPoints.Count > 0)
            {
                Point lastPoint = _drawAreaPoints.Last();
                _drawAreaGhostLine.X1 = lastPoint.X;
                _drawAreaGhostLine.Y1 = lastPoint.Y;
                _drawAreaGhostLine.X2 = mousePos.X;
                _drawAreaGhostLine.Y2 = mousePos.Y;
                _drawAreaGhostLine.Visibility = Visibility.Visible;
            }
        }

        private void FinishDrawingArea(bool save)
        {
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

        private void AssetList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                var asset = listBox.SelectedItem as Asset;
                if (asset == null) return;

                SetActiveTool(EditorTool.PlaceAsset);
                DragDrop.DoDragDrop(listBox, asset, DragDropEffects.Copy);
                SetActiveTool(EditorTool.Pan);
            }
        }

        private void ToolCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void ToolCanvas_Drop(object sender, DragEventArgs e)
        {
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
                };

                var newMapObject = new MapObject
                {
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
        }

        private void UpdateZoom(double newScale, bool force = false, bool isInitialLoad = false)
        {
            if (!this.IsLoaded && !force) return;

            _currentZoom = Math.Clamp(newScale, 0.1, 2.0); 

            CanvasScaleTransform.ScaleX = _currentZoom;
            CanvasScaleTransform.ScaleY = _currentZoom;

            UpdateScaleBar(_currentZoom);

        }

        private void ToolCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = 1.2;
            if (e.Delta < 0)
            {
                UpdateZoom(_currentZoom / zoomFactor);
            }
            else
            {
                UpdateZoom(_currentZoom * zoomFactor);
            }
            e.Handled = true;
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(_currentZoom * 1.5); 
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(_currentZoom / 1.5);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RequestNavigateBack?.Invoke(this, EventArgs.Empty);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
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
