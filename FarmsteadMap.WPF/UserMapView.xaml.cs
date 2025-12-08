using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FarmsteadMap.WPF
{
    public partial class UserMapView : UserControl
    {
        // =========================================================
        //                 КОНСТАНТИ
        // =========================================================
        private const double PixelsPerMeter = 100.0;
        private const double CanvasWidth = 3000;
        private const double CanvasHeight = 2000;
        private const double MinZoom = 0.2;
        private const double MaxZoom = 5.0;

        // =========================================================
        //                 СТАН РЕДАКТОРА
        // =========================================================
        private double _currentZoom = 1.0;
        private EditorTool _currentTool = EditorTool.Select;
        private string _activeBrushType = "";

        // Використовується для рослин та інших ассетів (Перейменовано клас)
        private EditorAsset? _selectedAssetToPlace = null;

        private bool _isDirty = false;
        private bool _isInitialized = false;

        // =========================================================
        //                 НАВІГАЦІЯ
        // =========================================================
        private Point _lastMousePos;
        private bool _isDraggingCanvas = false;

        // =========================================================
        //                 МАЛЮВАННЯ (Ландшафт/Будівлі)
        // =========================================================
        private List<Point> _drawPoints = new List<Point>();
        private Polyline? _tempPolyline;
        private Line? _rubberLine;
        private bool _isDrawing = false;

        // =========================================================
        //                 ВИДІЛЕННЯ ТА ПЕРЕМІЩЕННЯ
        // =========================================================
        private UIElement? _selectedElement;
        private Rectangle? _selectionAdorner;
        private bool _isDraggingElement = false;
        private Point _dragOffset;

        internal Dictionary<UIElement, TextBlock> _labels = new Dictionary<UIElement, TextBlock>();

        // =========================================================
        //                 СЕРВІСИ ТА ДАНІ
        // =========================================================
        private readonly HistoryManager _history;
        private long _mapId;
        private string? _username;

        // ДАНІ РОСЛИН (Завантажуються з БД)
        private MapElementsDTO _mapElements = new();

        // =========================================================
        //                 GHOST OBJECTS & TOOLS
        // =========================================================
        private Image? _ghostImage;
        private Ellipse? _rangeCircle;
        private Line? _rulerLine;
        private TextBlock? _rulerText;

        // =========================================================
        //                 ТИМЧАСОВІ ЗМІННІ ДЛЯ РОСЛИН
        // =========================================================
        private Point _pendingPlantPos;
        private long _pendingPlantId;
        private string _pendingPlantType; // "tree" або "veg"

        public UserMapView()
        {
            InitializeComponent();
            _history = new HistoryManager(this);

            this.Loaded += async (s, e) =>
            {
                this.Focus();
                await LoadMapElements(); // Завантажуємо типи рослин та сумісність
            };
            this.ViewportContainer.SizeChanged += OnViewportSizeChanged;
        }

        private async Task LoadMapElements()
        {
            try
            {
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IMapService>();
                    var result = await service.GetMapElementsAsync();
                    if (result.Success && result.Data != null)
                    {
                        _mapElements = result.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося завантажити каталог рослин: {ex.Message}");
            }
        }

        private void OnViewportSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_isInitialized && e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                CenterView();
                _isInitialized = true;
                this.ViewportContainer.SizeChanged -= OnViewportSizeChanged;
            }
        }

        public event EventHandler? RequestNavigateBack;
        public event EventHandler? RequestLogout;

        // =========================================================
        //                 ЗАВАНТАЖЕННЯ МАПИ (Load)
        // =========================================================
        public async void LoadMap(long mapId, string username, string? json = null)
        {
            _mapId = mapId;
            _username = username;
            _isDirty = false;

            ClearCanvas();

            if (string.IsNullOrEmpty(json))
            {
                try
                {
                    using (var scope = App.AppHost!.Services.CreateScope())
                    {
                        var mapService = scope.ServiceProvider.GetRequiredService<IMapService>();
                        var response = await mapService.GetMapAsync(mapId);

                        if (response.Success && response.Data != null)
                        {
                            json = response.Data.MapJson;
                        }
                        else
                        {
                            MessageBox.Show($"Не вдалося завантажити мапу: {response.Error}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка підключення до БД: {ex.Message}");
                }
            }

            if (string.IsNullOrEmpty(json) || json == "{}")
                json = "{\"viewport\":{\"zoom\":1.0},\"landscape\":[],\"objects\":[]}";

            try
            {
                var scene = JsonConvert.DeserializeObject<MapScene>(json);
                if (scene != null) RestoreScene(scene);
            }
            catch (Exception ex) { MessageBox.Show("Помилка обробки файлу мапи: " + ex.Message); }

            SaveButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));
        }

        // =========================================================
        //                 ЗБЕРЕЖЕННЯ МАПИ (Save)
        // =========================================================
        private async void SaveMapToDb()
        {
            var scene = new MapScene
            {
                Viewport = new Viewport
                {
                    Zoom = _currentZoom,
                    OffsetX = EditorScrollViewer.HorizontalOffset,
                    OffsetY = EditorScrollViewer.VerticalOffset
                }
            };

            // Ландшафт
            foreach (var child in LandscapeCanvas.Children)
            {
                if (child is Polygon poly)
                {
                    scene.Landscape.Add(new LandscapeArea
                    {
                        AreaType = poly.Tag?.ToString() ?? "grass",
                        IsPath = false,
                        Points = new List<Point>(poly.Points)
                    });
                }
                else if (child is Polyline line)
                {
                    scene.Landscape.Add(new LandscapeArea
                    {
                        AreaType = line.Tag?.ToString() ?? "path",
                        IsPath = true,
                        Points = new List<Point>(line.Points)
                    });
                }
            }

            // Будівлі
            foreach (var child in BuildingsCanvas.Children)
            {
                if (child is Polygon poly && poly.Tag is BuildingMetadata meta)
                {
                    var area = new LandscapeArea
                    {
                        AreaType = "building",
                        IsPath = false,
                        Points = new List<Point>(poly.Points)
                    };
                    area.Properties["Name"] = meta.Name;
                    area.Properties["Height"] = meta.Height;
                    scene.Landscape.Add(area);
                }
            }

            // Об'єкти (Рослини)
            foreach (var child in ObjectCanvas.Children)
            {
                if (child is Image img && img.Tag is MapElementDTO dto)
                {
                    scene.Objects.Add(new MapObject
                    {
                        DbId = dto.ElementId, // Тут ID типу рослини
                        Type = dto.Type,
                        X = Canvas.GetLeft(img),
                        Y = Canvas.GetTop(img),
                        Width = img.Width,
                        Height = img.Height,
                        Rotation = dto.Rotation
                    });
                }
            }

            string json = JsonConvert.SerializeObject(scene);

            try
            {
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var mapService = scope.ServiceProvider.GetRequiredService<IMapService>();
                    var updateDto = new UpdateMapDTO { Id = _mapId, MapJson = json };
                    var result = await mapService.UpdateMapAsync(updateDto);

                    if (result.Success)
                    {
                        MessageBox.Show("Мапу збережено успішно!");
                        _isDirty = false;
                        SaveButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));
                    }
                    else
                    {
                        MessageBox.Show($"Помилка збереження: {result.Error}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критична помилка: {ex.Message}");
            }
        }

        private void RestoreScene(MapScene scene)
        {
            // 1. Відновлення ландшафту (Без змін)
            foreach (var item in scene.Landscape)
            {
                var points = new PointCollection(item.Points);
                Brush brush = GetBrush(item.AreaType);

                if (item.AreaType == "building")
                {
                    var poly = new Polygon { Points = points, Stroke = Brushes.Black, StrokeThickness = 2, Fill = brush };
                    string name = item.Properties.GetValueOrDefault("Name", "Building");
                    string height = item.Properties.GetValueOrDefault("Height", "");

                    poly.Tag = new BuildingMetadata { Name = name, Height = height };
                    poly.ToolTip = $"{name} ({height})";

                    BuildingsCanvas.Children.Add(poly);
                    CreateLabelForBuilding(poly, name, height);
                }
                else if (item.IsPath)
                {
                    var line = new Polyline
                    {
                        Points = points,
                        Stroke = brush,
                        StrokeThickness = 20,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        Tag = item.AreaType
                    };
                    LandscapeCanvas.Children.Add(line);
                }
                else
                {
                    var poly = new Polygon { Points = points, Stroke = null, Fill = brush, Tag = item.AreaType };
                    LandscapeCanvas.Children.Add(poly);
                }
            }

            // 2. Відновлення об'єктів (ВИПРАВЛЕНО: Жорстка прив'язка до файлів)
            foreach (var obj in scene.Objects)
            {
                // Визначаємо файл за типом, ігноруючи базу
                string fixedImageName;
                switch (obj.Type)
                {
                    case "tree":
                        fixedImageName = "tree.png";
                        break;
                    case "flower":
                        fixedImageName = "flower.png";
                        break;
                    case "veg":
                        fixedImageName = "vegetables.png";
                        break;
                    default:
                        fixedImageName = "logo.png";
                        break;
                }

                // Формуємо абсолютний шлях
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = System.IO.Path.Combine(baseDir, "Images", "Plants", fixedImageName);

                var img = new Image
                {
                    Width = obj.Width > 0 ? obj.Width : PixelsPerMeter,
                    Height = obj.Height > 0 ? obj.Height : PixelsPerMeter,
                };

                // Пробуємо завантажити картинку
                try
                {
                    if (File.Exists(fullPath))
                    {
                        img.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                    }
                    else
                    {
                        // Якщо файлу немає, пробуємо логотип або пустишку
                        string logoPath = System.IO.Path.Combine(baseDir, "Images", "logo.png");
                        if (File.Exists(logoPath))
                            img.Source = new BitmapImage(new Uri(logoPath, UriKind.Absolute));
                    }
                }
                catch { }

                // Відновлюємо DTO для логіки
                img.Tag = new MapElementDTO
                {
                    ElementId = obj.DbId,
                    Type = obj.Type,
                    X = obj.X,
                    Y = obj.Y,
                    Rotation = obj.Rotation,
                    Width = img.Width,
                    Height = img.Height
                };

                Canvas.SetLeft(img, obj.X);
                Canvas.SetTop(img, obj.Y);
                ObjectCanvas.Children.Add(img);
            }

            // 3. Відновлення камери (Без змін)
            if (scene.Viewport != null)
            {
                _currentZoom = scene.Viewport.Zoom;
                if (_currentZoom < MinZoom) _currentZoom = MinZoom;

                CanvasScaleTransform.ScaleX = _currentZoom;
                CanvasScaleTransform.ScaleY = _currentZoom;
                UpdateScaleUI();

                EditorScrollViewer.UpdateLayout();
                EditorScrollViewer.ScrollToHorizontalOffset(scene.Viewport.OffsetX);
                EditorScrollViewer.ScrollToVerticalOffset(scene.Viewport.OffsetY);
            }
        }

        // =========================================================
        //                 МЕНЮ (DRAWER)
        // =========================================================
        private void FillDrawer(string category)
        {
            DrawerContent.Children.Clear();
            DrawerTitle.Text = category;

            switch (category)
            {
                case "Landscape":
                    AddTextureButton("Трава (Пензлик)", "GrassTexture", "Малювати траву довільно", () => SetDrawMode(EditorTool.DrawLine, "grass"));
                    AddTextureButton("Трава (Зона)", "GrassTexture", "Малювати газон (полігон)", () => SetDrawMode(EditorTool.DrawArea, "grass"));
                    AddTextureButton("Вода (Пензлик)", "WaterTexture", "Малювати воду довільно", () => SetDrawMode(EditorTool.DrawLine, "water"));
                    AddTextureButton("Вода (Зона)", "WaterTexture", "Малювати озеро (полігон)", () => SetDrawMode(EditorTool.DrawArea, "water"));
                    AddTextureButton("Доріжка", "PathTexture", "Малювати стежку", () => SetDrawMode(EditorTool.DrawLine, "path"));
                    AddTextureButton("Грядка", "SoilTexture", "Малювати грядку", () => SetDrawMode(EditorTool.DrawArea, "soil"));
                    break;
                case "Buildings":
                    AddTextureButton("Будинок", "BuildingTexture", "Малювати споруду", () => SetDrawMode(EditorTool.DrawArea, "building"));
                    break;
                case "Vegetation":
                    // ДИНАМІЧНЕ ЗАВАНТАЖЕННЯ З DTO
                    if (_mapElements.Trees != null)
                    {
                        foreach (var tree in _mapElements.Trees)
                            AddPlantButton(tree.Id, tree.Name, tree.Image, "tree");
                    }
                    if (_mapElements.Vegetables != null)
                    {
                        foreach (var veg in _mapElements.Vegetables)
                            AddPlantButton(veg.Id, veg.Name, veg.Image, "veg");
                    }
                    if (_mapElements.Flowers != null)
                    {
                        foreach (var flower in _mapElements.Flowers)
                            AddPlantButton(flower.Id, flower.Name, flower.Image, "flower");
                    }
                    break;
            }
        }

        // Змінив аргумент з BasePlantDTO на прості типи, щоб уникнути помилки CS1503
        private void AddPlantButton(long id, string name, string dbImageName, string type)
        {
            var btn = new Button { Style = (Style)FindResource("TextureButtonStyle"), ToolTip = name };

            // 1. ЖОРСТКА ПРИВ'ЯЗКА (Hardcode)
            string fixedImageName;
            switch (type)
            {
                case "tree":
                    fixedImageName = "tree.png";
                    break;
                case "flower":
                    fixedImageName = "flower.png";
                    break;
                case "veg":
                    fixedImageName = "vegetables.png"; // Перевірте, чи у вас .png чи .jpg
                    break;
                default:
                    fixedImageName = "logo.png";
                    break;
            }

            // 2. Формуємо повний шлях
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = System.IO.Path.Combine("Images", "Plants", fixedImageName);
            string fullPath = System.IO.Path.Combine(baseDir, relativePath);

            ImageBrush? imgBrush = null;

            // 3. Завантажуємо картинку для кнопки
            if (File.Exists(fullPath))
            {
                try
                {
                    imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
                catch { }
            }
            else
            {
                // Фолбек на логотип
                try
                {
                    string logoPath = System.IO.Path.Combine(baseDir, "Images", "logo.png");
                    if (File.Exists(logoPath))
                    {
                        imgBrush = new ImageBrush();
                        imgBrush.ImageSource = new BitmapImage(new Uri(logoPath, UriKind.Absolute));
                    }
                }
                catch { }
            }

            // 4. Створюємо візуал кнопки
            var rect = new Rectangle
            {
                Width = 32,
                Height = 32,
                Fill = (Brush?)imgBrush ?? Brushes.LightGray,
                Stroke = Brushes.Gray,
                StrokeThickness = 1,
                RadiusX = 3,
                RadiusY = 3
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(rect, 0); grid.Children.Add(rect);

            var sp = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            sp.Children.Add(new TextBlock { Text = name, FontWeight = FontWeights.SemiBold, FontSize = 12 });
            sp.Children.Add(new TextBlock { Text = type, FontSize = 10, Foreground = Brushes.Gray });
            Grid.SetColumn(sp, 1); grid.Children.Add(sp);

            btn.Content = grid;

            // 5. При кліку передаємо ПРАВИЛЬНИЙ шлях (fullPath) далі в інструмент розміщення
            btn.Click += (s, e) => {
                string finalImagePath = (imgBrush != null && File.Exists(fullPath)) ? fullPath : "/Images/logo.png";

                _selectedAssetToPlace = new EditorAsset
                {
                    Id = (int)id,
                    Name = name,
                    Image = finalImagePath, // <-- Тут тепер буде локальний шлях
                    Type = type
                };
                SetTool(EditorTool.PlaceAsset);
            };
            DrawerContent.Children.Add(btn);
        }

        private void AddTextureButton(string text, string resourceKey, string tooltip, Action onClick)
        {
            var btn = new Button { Style = (Style)FindResource("TextureButtonStyle"), ToolTip = tooltip };

            string typeKey = "grass";
            if (text.ToLower().Contains("вода")) typeKey = "water";
            else if (text.ToLower().Contains("грядка")) typeKey = "soil";
            else if (text.ToLower().Contains("доріжка")) typeKey = "path";
            else if (text.ToLower().Contains("будинок")) typeKey = "building";

            Brush fillBrush = GetBrush(typeKey);
            var rect = new Rectangle { Width = 32, Height = 32, Fill = fillBrush, Stroke = Brushes.Gray, StrokeThickness = 1, RadiusX = 5, RadiusY = 5 };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(rect, 0); grid.Children.Add(rect);

            var sp = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            sp.Children.Add(new TextBlock { Text = text, FontWeight = FontWeights.SemiBold, FontSize = 12, Foreground = Brushes.Black });
            sp.Children.Add(new TextBlock { Text = tooltip, FontSize = 10, Foreground = Brushes.Gray });
            Grid.SetColumn(sp, 1); grid.Children.Add(sp);

            btn.Content = grid;
            btn.Click += (s, e) => onClick();
            DrawerContent.Children.Add(btn);
        }

        private Brush GetBrush(string type)
        {
            try
            {
                string texturesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Textures");
                string fileJpg = System.IO.Path.Combine(texturesPath, $"{type}.jpg");
                string filePng = System.IO.Path.Combine(texturesPath, $"{type}.png");
                string? foundFile = File.Exists(fileJpg) ? fileJpg : (File.Exists(filePng) ? filePng : null);
                if (foundFile != null)
                {
                    var imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(foundFile, UriKind.Absolute));
                    imgBrush.TileMode = TileMode.Tile;
                    imgBrush.Stretch = Stretch.Fill;
                    imgBrush.Viewport = new Rect(0, 0, PixelsPerMeter, PixelsPerMeter);
                    imgBrush.ViewportUnits = BrushMappingMode.Absolute;
                    return imgBrush;
                }
            }
            catch { }

            try
            {
                string resKey = type switch
                {
                    "grass" => "GrassTexture",
                    "water" => "WaterTexture",
                    "soil" => "SoilTexture",
                    "path" => "PathTexture",
                    "building" => "BuildingTexture",
                    _ => "GrassTexture"
                };
                return (Brush)FindResource(resKey);
            }
            catch { return Brushes.Gray; }
        }

        // =========================================================
        //                 ZOOM & SCROLL
        // =========================================================
        private void EditorScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                Point mouseInViewer = e.GetPosition(EditorScrollViewer);
                Point contentCenter = new Point(EditorScrollViewer.HorizontalOffset + mouseInViewer.X, EditorScrollViewer.VerticalOffset + mouseInViewer.Y);
                Point logicalCenter = new Point(contentCenter.X / _currentZoom, contentCenter.Y / _currentZoom);
                double factor = e.Delta > 0 ? 1.2 : 1.0 / 1.2;
                ApplyZoom(factor, logicalCenter, mouseInViewer);
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                EditorScrollViewer.ScrollToHorizontalOffset(EditorScrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e) => ApplyZoomCenter(1.2);
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e) => ApplyZoomCenter(1.0 / 1.2);

        private void ApplyZoomCenter(double factor)
        {
            Point centerScreen = new Point(EditorScrollViewer.ViewportWidth / 2, EditorScrollViewer.ViewportHeight / 2);
            Point centerOfView = new Point(EditorScrollViewer.HorizontalOffset + centerScreen.X, EditorScrollViewer.VerticalOffset + centerScreen.Y);
            Point logicalCenter = new Point(centerOfView.X / _currentZoom, centerOfView.Y / _currentZoom);
            ApplyZoom(factor, logicalCenter, centerScreen);
        }

        private void ApplyZoom(double factor, Point logicalCenter, Point screenOffset)
        {
            double newZoom = _currentZoom * factor;
            if (newZoom < MinZoom) newZoom = MinZoom;
            if (newZoom > MaxZoom) newZoom = MaxZoom;
            if (Math.Abs(newZoom - _currentZoom) < 0.001) return;

            _currentZoom = newZoom;
            CanvasScaleTransform.ScaleX = _currentZoom;
            CanvasScaleTransform.ScaleY = _currentZoom;
            UpdateScaleUI();
            EditorScrollViewer.UpdateLayout();

            double newH = logicalCenter.X * newZoom - screenOffset.X;
            double newV = logicalCenter.Y * newZoom - screenOffset.Y;
            EditorScrollViewer.ScrollToHorizontalOffset(newH);
            EditorScrollViewer.ScrollToVerticalOffset(newV);
        }

        private void CenterView()
        {
            _currentZoom = 0.5;
            CanvasScaleTransform.ScaleX = _currentZoom;
            CanvasScaleTransform.ScaleY = _currentZoom;
            EditorScrollViewer.UpdateLayout();
            EditorScrollViewer.ScrollToHorizontalOffset((MainViewport.ActualWidth * _currentZoom - EditorScrollViewer.ViewportWidth) / 2);
            EditorScrollViewer.ScrollToVerticalOffset((MainViewport.ActualHeight * _currentZoom - EditorScrollViewer.ViewportHeight) / 2);
            UpdateScaleUI();
        }

        private void UpdateScaleUI()
        {
            ZoomLevelText.Text = $"{(_currentZoom * 100):F0}%";
            ScaleBar.Width = PixelsPerMeter * _currentZoom;
            if (_selectionAdorner != null) _selectionAdorner.StrokeThickness = 2 / _currentZoom;
        }

        // =========================================================
        //                 MOUSE INTERACTION (MAIN)
        // =========================================================

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = ClampPoint(e.GetPosition(MainViewport));

            // PAN
            if (_currentTool == EditorTool.Pan || e.MiddleButton == MouseButtonState.Pressed)
            {
                _isDraggingCanvas = true;
                _lastMousePos = e.GetPosition(EditorScrollViewer);
                MainViewport.CaptureMouse();
                return;
            }

            // SELECT
            if (_currentTool == EditorTool.Select && e.LeftButton == MouseButtonState.Pressed)
            {
                var hit = HitTest(pos);
                if (hit != null)
                {
                    SelectElement(hit); _isDraggingElement = true; _lastMousePos = pos;
                    double l = Canvas.GetLeft(hit); double t = Canvas.GetTop(hit);
                    if (double.IsNaN(l)) l = 0; if (double.IsNaN(t)) t = 0;
                    _dragOffset = new Point(pos.X - l, pos.Y - t);
                    MainViewport.CaptureMouse();
                }
                else Deselect();
                return;
            }

            // DRAW LANDSCAPE/BUILDING
            if ((_currentTool == EditorTool.DrawArea || _currentTool == EditorTool.DrawLine) && e.LeftButton == MouseButtonState.Pressed)
            {
                if (_currentTool == EditorTool.DrawLine)
                {
                    _isDrawing = true; _drawPoints.Clear(); _drawPoints.Add(pos);
                    _tempPolyline = new Polyline
                    {
                        Stroke = GetBrush(_activeBrushType),
                        StrokeThickness = 20,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        Points = new PointCollection { pos },
                        Tag = _activeBrushType
                    };
                    LandscapeCanvas.Children.Add(_tempPolyline);
                    MainViewport.CaptureMouse();
                }
                else
                {
                    if (!_isDrawing)
                    {
                        _isDrawing = true; _drawPoints.Clear();
                        _tempPolyline = new Polyline { Stroke = Brushes.Blue, StrokeThickness = 2 / _currentZoom, StrokeDashArray = new DoubleCollection { 4, 2 } };
                        ToolCanvas.Children.Add(_tempPolyline);
                        _rubberLine = new Line { Stroke = Brushes.Blue, Opacity = 0.5 };
                        ToolCanvas.Children.Add(_rubberLine);
                    }

                    Point p = pos;
                    if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && _drawPoints.Count > 0) p = GetOrthoPoint(_drawPoints.Last(), pos);

                    if (_drawPoints.Count > 2 && Distance(p, _drawPoints[0]) < 10 / _currentZoom) FinishDrawing();
                    else { _drawPoints.Add(p); _tempPolyline!.Points.Add(p); }
                }
            }
            // PLACE ASSET (PLANT)
            else if (_currentTool == EditorTool.PlaceAsset && e.LeftButton == MouseButtonState.Pressed)
            {
                PlaceAsset(pos);
            }
            // RULER
            else if (_currentTool == EditorTool.Ruler && e.LeftButton == MouseButtonState.Pressed)
            {
                _rulerLine = new Line { X1 = pos.X, Y1 = pos.Y, X2 = pos.X, Y2 = pos.Y, Stroke = Brushes.Red, StrokeThickness = 2 / _currentZoom };
                _rulerText = new TextBlock { Text = "0.0 m", Background = Brushes.White, Foreground = Brushes.Red, FontSize = 24, FontWeight = FontWeights.Bold, Padding = new Thickness(2) };
                ToolCanvas.Children.Add(_rulerLine); ToolCanvas.Children.Add(_rulerText);
            }
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var pos = ClampPoint(e.GetPosition(MainViewport));
            CoordinatesText.Text = $"X: {pos.X / PixelsPerMeter:F2} m, Y: {pos.Y / PixelsPerMeter:F2} m";

            if (_isDraggingCanvas)
            {
                Point currentPos = e.GetPosition(EditorScrollViewer);
                Vector delta = _lastMousePos - currentPos;
                EditorScrollViewer.ScrollToHorizontalOffset(EditorScrollViewer.HorizontalOffset + delta.X);
                EditorScrollViewer.ScrollToVerticalOffset(EditorScrollViewer.VerticalOffset + delta.Y);
                _lastMousePos = currentPos;
                return;
            }

            if (_isDraggingElement && _selectedElement != null)
            {
                double l = pos.X - _dragOffset.X; double t = pos.Y - _dragOffset.Y;
                l = Math.Clamp(l, 0, CanvasWidth); t = Math.Clamp(t, 0, CanvasHeight);
                if (_selectedElement is Image img) { Canvas.SetLeft(img, l); Canvas.SetTop(img, t); }
                UpdateAdorner(); MarkDirty(); _lastMousePos = pos;
            }

            if (_isDrawing && _currentTool == EditorTool.DrawLine && e.LeftButton == MouseButtonState.Pressed && _tempPolyline != null)
            {
                if (Distance(pos, _drawPoints.Last()) > 5) { _drawPoints.Add(pos); _tempPolyline.Points.Add(pos); }
            }
            else if (_isDrawing && _currentTool == EditorTool.DrawArea && _rubberLine != null && _drawPoints.Any())
            {
                Point target = pos;
                bool snap = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                if (snap) { target = GetOrthoPoint(_drawPoints.Last(), pos); SnappingIndicator.Visibility = Visibility.Visible; } else SnappingIndicator.Visibility = Visibility.Collapsed;
                _rubberLine.X1 = _drawPoints.Last().X; _rubberLine.Y1 = _drawPoints.Last().Y; _rubberLine.X2 = target.X; _rubberLine.Y2 = target.Y;
            }

            if (_currentTool == EditorTool.PlaceAsset)
            {
                UpdateGhostObject(pos);
                CheckCompatibility(pos);
            }

            if (_currentTool == EditorTool.Ruler && e.LeftButton == MouseButtonState.Pressed && _rulerLine != null)
            {
                _rulerLine.X2 = pos.X; _rulerLine.Y2 = pos.Y;
                double d = Distance(new Point(_rulerLine.X1, _rulerLine.Y1), pos) / PixelsPerMeter;
                _rulerText!.Text = $"{d:F1} m";
                Canvas.SetLeft(_rulerText, pos.X + 10); Canvas.SetTop(_rulerText, pos.Y);
            }
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDraggingCanvas) { _isDraggingCanvas = false; MainViewport.ReleaseMouseCapture(); }
            if (_isDraggingElement) { _isDraggingElement = false; MainViewport.ReleaseMouseCapture(); }
            if (_currentTool == EditorTool.DrawLine && _isDrawing)
            {
                _isDrawing = false; MainViewport.ReleaseMouseCapture();
                _history.PushAction(new AddDeleteAction(new List<UIElement> { _tempPolyline! }, LandscapeCanvas, true));
                _tempPolyline = null; MarkDirty();
            }
            if (_currentTool == EditorTool.Ruler) ToolCanvas.Children.Clear();
        }

        // =========================================================
        //                 GHOST & COMPATIBILITY
        // =========================================================

        private void UpdateGhostObject(Point pos)
        {
            if (_ghostImage == null && _selectedAssetToPlace != null)
            {
                _ghostImage = new Image
                {
                    Width = PixelsPerMeter,
                    Height = PixelsPerMeter,
                    Opacity = 0.5,
                };

                // Виправлена ініціалізація
                try
                {
                    _ghostImage.Source = new BitmapImage(new Uri(_selectedAssetToPlace.Image, UriKind.RelativeOrAbsolute));
                }
                catch { }

                ToolCanvas.Children.Add(_ghostImage);
            }
            if (_ghostImage != null)
            {
                Canvas.SetLeft(_ghostImage, pos.X - PixelsPerMeter / 2);
                Canvas.SetTop(_ghostImage, pos.Y - PixelsPerMeter / 2);
            }
        }

        private void CheckCompatibility(Point pos)
        {
            if (_selectedAssetToPlace == null) return;

            bool conflict = false;
            long currentId = _selectedAssetToPlace.Id;
            string currentType = _selectedAssetToPlace.Type;

            foreach (UIElement el in ObjectCanvas.Children)
            {
                if (el is Image existingImg && existingImg.Tag is MapElementDTO dto)
                {
                    double x = Canvas.GetLeft(existingImg);
                    double y = Canvas.GetTop(existingImg);
                    double dist = Distance(pos, new Point(x + existingImg.Width / 2, y + existingImg.Height / 2));

                    // Перевіряємо, якщо відстань менше 3 метрів (300 пікселів)
                    if (dist < 3 * PixelsPerMeter)
                    {
                        if (currentType == "tree" && dto.Type == "tree")
                        {
                            if (IsTreeIncompatible(currentId, dto.ElementId)) conflict = true;
                        }
                        else if (currentType == "veg" && dto.Type == "veg")
                        {
                            if (IsVegIncompatible(currentId, dto.ElementId)) conflict = true;
                        }
                    }
                }
            }

            CompatibilityWarning.Visibility = conflict ? Visibility.Visible : Visibility.Collapsed;

            if (_ghostImage != null)
            {
                _ghostImage.Opacity = conflict ? 0.8 : 0.5;
            }
        }

        private bool IsTreeIncompatible(long id1, long id2)
        {
            return _mapElements.TreeIncompatibilities.Any(x => (x.Id1 == id1 && x.Id2 == id2) || (x.Id1 == id2 && x.Id2 == id1));
        }
        private bool IsVegIncompatible(long id1, long id2)
        {
            return _mapElements.VegIncompatibilities.Any(x => (x.Id1 == id1 && x.Id2 == id2) || (x.Id1 == id2 && x.Id2 == id1));
        }

        // =========================================================
        //                 PLACEMENT LOGIC (MODALS)
        // =========================================================

        private void PlaceAsset(Point pos)
        {
            if (_selectedAssetToPlace == null) return;

            _pendingPlantPos = pos;
            _pendingPlantId = _selectedAssetToPlace.Id;
            _pendingPlantType = _selectedAssetToPlace.Type;

            // Якщо квітка - ставимо одразу
            if (_pendingPlantType == "flower")
            {
                FinalizePlacement(0, "");
                return;
            }

            // Якщо дерево або овоч - відкриваємо модалку сортів
            OpenSortModal(_pendingPlantId, _pendingPlantType);
        }

        private async void OpenSortModal(long id, string type)
        {
            // Очищаємо перед завантаженням
            PlantSortComboBox.ItemsSource = null;
            PlantSortTitle.Text = type == "tree" ? "Оберіть сорт дерева" : "Оберіть сорт овоча";

            // Показуємо модалку одразу (поки вантажиться)
            PlantSortModal.Visibility = Visibility.Visible;

            try
            {
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IMapService>();

                    BaseResponseDTO<List<SortDTO>> res;

                    if (type == "tree")
                    {
                        // MessageBox.Show($"Запит сортів для дерева ID: {id}"); // Розкоментуй для тесту
                        res = await service.GetTreeSortsAsync(id);
                    }
                    else
                    {
                        res = await service.GetVegSortsAsync(id);
                    }

                    if (res.Success)
                    {
                        if (res.Data != null && res.Data.Count > 0)
                        {
                            PlantSortComboBox.ItemsSource = res.Data;
                            // Автоматично обираємо перший елемент
                            PlantSortComboBox.SelectedIndex = 0;
                        }
                        else
                        {
                            MessageBox.Show($"Сортів не знайдено для {type} з ID {id}.\nПеревірте таблицю {(type == "tree" ? "tree_sorts" : "veg_sorts")} в БД.");
                            PlantSortModal.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Помилка завантаження сортів: {res.Error}");
                        PlantSortModal.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критична помилка: {ex.Message}");
                PlantSortModal.Visibility = Visibility.Collapsed;
            }
        }

        private void SavePlantSort_Click(object sender, RoutedEventArgs e)
        {
            if (PlantSortComboBox.SelectedItem is SortDTO sort)
            {
                FinalizePlacement(sort.Id, sort.Name);
                PlantSortModal.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть сорт.");
            }
        }

        private void CancelPlantSort_Click(object sender, RoutedEventArgs e)
        {
            PlantSortModal.Visibility = Visibility.Collapsed;
            ResetTempVisuals();
        }

        private void FinalizePlacement(long sortId, string sortName)
        {
            var img = new Image
            {
                Width = PixelsPerMeter,
                Height = PixelsPerMeter,
            };

            try
            {
                img.Source = new BitmapImage(new Uri(_selectedAssetToPlace!.Image, UriKind.RelativeOrAbsolute));
            }
            catch { }

            img.Tag = new MapElementDTO
            {
                ElementId = _pendingPlantId, // ID типу (напр. Дуб)
                Type = _pendingPlantType,
                X = _pendingPlantPos.X,
                Y = _pendingPlantPos.Y,
                Rotation = 0,
                Width = PixelsPerMeter,
                Height = PixelsPerMeter
            };

            if (!string.IsNullOrEmpty(sortName)) img.ToolTip = sortName;

            Canvas.SetLeft(img, _pendingPlantPos.X - PixelsPerMeter / 2);
            Canvas.SetTop(img, _pendingPlantPos.Y - PixelsPerMeter / 2);

            ObjectCanvas.Children.Add(img);
            _history.PushAction(new AddDeleteAction(new List<UIElement> { img }, ObjectCanvas, true));
            MarkDirty();
            ResetTempVisuals();
        }

        // =========================================================
        //                 BUILDING MODAL LOGIC
        // =========================================================
        private void FinishDrawing()
        {
            if (_drawPoints.Count < 2) { ResetTempVisuals(); return; }
            if (_activeBrushType == "building")
            {
                double area = CalculatePolygonArea(_drawPoints) / (PixelsPerMeter * PixelsPerMeter);
                BuildingHeightInput.Text = $"{area:F1} м²";
                BuildingModal.Visibility = Visibility.Visible;
                return;
            }
            if (_currentTool == EditorTool.DrawArea)
            {
                var poly = new Polygon { Points = new PointCollection(_drawPoints), Stroke = null, Fill = GetBrush(_activeBrushType), Tag = _activeBrushType };
                LandscapeCanvas.Children.Add(poly); _history.PushAction(new AddDeleteAction(new List<UIElement> { poly }, LandscapeCanvas, true));
            }
            MarkDirty(); ResetTempVisuals();
        }

        private void SaveBuilding_Click(object sender, RoutedEventArgs e)
        {
            var poly = new Polygon { Points = new PointCollection(_drawPoints), Stroke = Brushes.Black, StrokeThickness = 2, Fill = (Brush)FindResource("BuildingTexture"), Tag = new BuildingMetadata { Name = BuildingNameInput.Text, Height = BuildingHeightInput.Text } };
            BuildingsCanvas.Children.Add(poly);
            CreateLabelForBuilding(poly, BuildingNameInput.Text, BuildingHeightInput.Text);

            var label = _labels[poly];
            _history.PushAction(new AddDeleteAction(new List<UIElement> { poly, label }, BuildingsCanvas, true));

            BuildingModal.Visibility = Visibility.Collapsed; MarkDirty(); ResetTempVisuals();
        }
        private void CancelBuilding_Click(object sender, RoutedEventArgs e) { BuildingModal.Visibility = Visibility.Collapsed; ResetTempVisuals(); }

        private void CreateLabelForBuilding(Polygon poly, string name, string height)
        {
            var center = GetCentroid(poly.Points.ToList());
            var textBlock = new TextBlock { Text = $"{name}\n{height}", Foreground = Brushes.Black, FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Center, IsHitTestVisible = false, Background = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255)) };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Canvas.SetLeft(textBlock, center.X - textBlock.DesiredSize.Width / 2); Canvas.SetTop(textBlock, center.Y - textBlock.DesiredSize.Height / 2);
            BuildingsCanvas.Children.Add(textBlock); _labels[poly] = textBlock;
        }

        // =========================================================
        //                 HELPERS & UTILS
        // =========================================================
        private void ClearCanvas()
        {
            LandscapeCanvas.Children.Clear();
            BuildingsCanvas.Children.Clear();
            ObjectCanvas.Children.Clear();
            ResetTempVisuals();
            _labels.Clear();
        }

        private void ResetTempVisuals()
        {
            _isDrawing = false;
            _drawPoints.Clear();
            ToolCanvas.Children.Clear();
            _ghostImage = null;
            _rangeCircle = null;
            CompatibilityWarning.Visibility = Visibility.Collapsed;
            SnappingIndicator.Visibility = Visibility.Collapsed;
        }

        private double Distance(Point p1, Point p2) => Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

        private void DeleteSelected()
        {
            if (_selectedElement != null)
            {
                List<UIElement> list = new List<UIElement> { _selectedElement };
                Canvas parent = null;
                if (ObjectCanvas.Children.Contains(_selectedElement)) parent = ObjectCanvas;
                else if (LandscapeCanvas.Children.Contains(_selectedElement)) parent = LandscapeCanvas;
                else if (BuildingsCanvas.Children.Contains(_selectedElement))
                {
                    parent = BuildingsCanvas;
                    if (_labels.TryGetValue(_selectedElement, out var label))
                    {
                        list.Add(label);
                        _labels.Remove(_selectedElement);
                        if (parent.Children.Contains(label)) parent.Children.Remove(label);
                    }
                }

                if (parent != null)
                {
                    if (parent.Children.Contains(_selectedElement)) parent.Children.Remove(_selectedElement);
                    _history.PushAction(new AddDeleteAction(list, parent, false));
                    MarkDirty();
                }
                Deselect();
            }
        }

        private Point GetCentroid(List<Point> points) { double x = 0, y = 0; foreach (var p in points) { x += p.X; y += p.Y; } return new Point(x / points.Count, y / points.Count); }
        private double CalculatePolygonArea(List<Point> points) { double area = 0; int j = points.Count - 1; for (int i = 0; i < points.Count; i++) { area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y); j = i; } return Math.Abs(area / 2.0); }
        private Point ClampPoint(Point p) => new Point(Math.Clamp(p.X, 0, CanvasWidth), Math.Clamp(p.Y, 0, CanvasHeight));
        private void ToolButton_Click(object sender, RoutedEventArgs e) { if (sender is Button b && Enum.TryParse(b.Tag.ToString(), out EditorTool t)) SetTool(t); }
        private void CategoryButton_Click(object sender, RoutedEventArgs e) { if (sender is Button b) { SideDrawer.Visibility = Visibility.Visible; FillDrawer(b.Tag.ToString()); } }
        private void CloseDrawer_Click(object sender, RoutedEventArgs e) => SideDrawer.Visibility = Visibility.Collapsed;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewport.ReleaseMouseCapture();
            if (_isDirty && MessageBox.Show("Save changes?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SaveMapToDb();
            RequestNavigateBack?.Invoke(this, EventArgs.Empty);
        }

        private void SetTool(EditorTool tool) { _currentTool = tool; ResetTempVisuals(); Deselect(); if (tool == EditorTool.Pan) MainViewport.Cursor = Cursors.SizeAll; else if (tool == EditorTool.Select) MainViewport.Cursor = Cursors.Arrow; else MainViewport.Cursor = Cursors.Cross; }
        private void SetDrawMode(EditorTool tool, string type) { _activeBrushType = type; SetTool(tool); }
        private void MarkDirty() { _isDirty = true; }
        private void SaveButton_Click(object sender, RoutedEventArgs e) => SaveMapToDb();
        private void UndoButton_Click(object sender, RoutedEventArgs e) => _history.Undo();
        private void RedoButton_Click(object sender, RoutedEventArgs e) => _history.Redo();
        private void DeleteCommand(object sender, ExecutedRoutedEventArgs e) => DeleteSelected();
        private void UserControl_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Delete) DeleteSelected(); }
        private void SelectElement(UIElement e) { Deselect(); _selectedElement = e; if (e is Image img) { double l = Canvas.GetLeft(img), t = Canvas.GetTop(img); _selectionAdorner = new Rectangle { Width = img.Width, Height = img.Height, Stroke = Brushes.Cyan, StrokeThickness = 2 / _currentZoom, StrokeDashArray = new DoubleCollection { 4, 2 } }; Canvas.SetLeft(_selectionAdorner, l); Canvas.SetTop(_selectionAdorner, t); ToolCanvas.Children.Add(_selectionAdorner); } }
        private void Deselect() { _selectedElement = null; ToolCanvas.Children.Clear(); }
        private void UpdateAdorner() { if (_selectedElement is Image img && _selectionAdorner != null) { Canvas.SetLeft(_selectionAdorner, Canvas.GetLeft(img)); Canvas.SetTop(_selectionAdorner, Canvas.GetTop(img)); } }
        private UIElement? HitTest(Point pos) { foreach (UIElement c in ObjectCanvas.Children) { if (c is Image img) { double l = Canvas.GetLeft(img), t = Canvas.GetTop(img); if (pos.X >= l && pos.X <= l + img.Width && pos.Y >= t && pos.Y <= t + img.Height) return img; } } return null; }
        public void UpdateHistoryButtonsState() { }
        private Point GetOrthoPoint(Point start, Point current) { double dx = Math.Abs(current.X - start.X); double dy = Math.Abs(current.Y - start.Y); return dx > dy ? new Point(current.X, start.Y) : new Point(start.X, current.Y); }
    }

    // =========================================================
    //                 DTO ТА ДОПОМІЖНІ КЛАСИ
    // =========================================================

    public class BuildingMetadata { public string Name { get; set; } = ""; public string Height { get; set; } = ""; }

    // Перейменовано з Asset в EditorAsset, щоб уникнути конфлікту CS0229
    public class EditorAsset
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // "tree", "veg", "flower"
        public string Image { get; set; } = "";
    }

    public interface IEditorAction { void Undo(UserMapView editor); void Redo(UserMapView editor); }
    public class AddDeleteAction : IEditorAction
    {
        private readonly List<UIElement> _elements; private readonly Canvas _c; private readonly bool _add;
        public AddDeleteAction(List<UIElement> elements, Canvas c, bool add) { _elements = elements; _c = c; _add = add; }
        public void Undo(UserMapView ed)
        {
            foreach (var e in _elements) { if (_add) _c.Children.Remove(e); else { _c.Children.Add(e); if (e is FrameworkElement fe && ed._labels.ContainsKey(e)) ed.BuildingsCanvas.Children.Add(ed._labels[e]); } }
        }
        public void Redo(UserMapView ed)
        {
            foreach (var e in _elements) { if (_add) { _c.Children.Add(e); if (e is FrameworkElement fe && ed._labels.ContainsKey(e)) ed.BuildingsCanvas.Children.Add(ed._labels[e]); } else _c.Children.Remove(e); }
        }
    }
    public class HistoryManager { private readonly Stack<IEditorAction> _u = new Stack<IEditorAction>(); private readonly Stack<IEditorAction> _r = new Stack<IEditorAction>(); private readonly UserMapView _ed; public HistoryManager(UserMapView ed) => _ed = ed; public void PushAction(IEditorAction a) { _u.Push(a); _r.Clear(); _ed.UpdateHistoryButtonsState(); } public void Undo() { if (_u.Count > 0) { var a = _u.Pop(); a.Undo(_ed); _r.Push(a); _ed.UpdateHistoryButtonsState(); } } public void Redo() { if (_r.Count > 0) { var a = _r.Pop(); a.Redo(_ed); _u.Push(a); _ed.UpdateHistoryButtonsState(); } } public bool CanUndo => _u.Count > 0; public bool CanRedo => _r.Count > 0; }
}