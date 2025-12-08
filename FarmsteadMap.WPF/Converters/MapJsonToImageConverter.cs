using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FarmsteadMap.WPF.Converters
{
    public class MapJsonToImageConverter : IValueConverter
    {
        private const double Margin = 50.0;
        private const double FullCanvasWidth = 3000;
        private const double FullCanvasHeight = 2000;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string json && !string.IsNullOrEmpty(json) && json != "{}")
            {
                try
                {
                    var scene = JsonConvert.DeserializeObject<MapScene>(json);
                    if (scene == null || (scene.Landscape.Count == 0 && scene.Objects.Count == 0))
                        return CreateEmptyPlaceholder();

                    Rect bounds = CalculateBounds(scene);
                    var drawingGroup = new DrawingGroup();

                    // Фон
                    drawingGroup.Children.Add(new GeometryDrawing(
                        GetTextureBrush("background_base"),
                        null,
                        new RectangleGeometry(bounds)));

                    // Ландшафт
                    foreach (var area in scene.Landscape)
                    {
                        if (area.Points == null || area.Points.Count < 2) continue;

                        var geometry = new StreamGeometry();
                        using (var ctx = geometry.Open())
                        {
                            ctx.BeginFigure(area.Points[0], true, !area.IsPath);
                            ctx.PolyLineTo(area.Points.Skip(1).ToList(), true, true);
                        }
                        geometry.Freeze();

                        Brush fill = area.IsPath ? null : GetTextureBrush(area.AreaType);
                        Pen stroke = area.IsPath ? new Pen(GetTextureBrush(area.AreaType), 20) { LineJoin = PenLineJoin.Round, StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round } : null;

                        drawingGroup.Children.Add(new GeometryDrawing(fill, stroke, geometry));
                    }

                    // Об'єкти
                    foreach (var obj in scene.Objects)
                    {
                        double w = obj.Width > 0 ? obj.Width : 100;
                        double h = obj.Height > 0 ? obj.Height : 100;

                        Brush objBrush = obj.Type.Contains("tree")
                            ? GetTextureBrush("tree_top")
                            : GetTextureBrush("veg_top");

                        var ellipse = new EllipseGeometry(new Rect(obj.X, obj.Y, w, h));
                        drawingGroup.Children.Add(new GeometryDrawing(objBrush, new Pen(Brushes.DarkOliveGreen, 1), ellipse));
                    }

                    var drawingImage = new DrawingImage(drawingGroup);
                    drawingImage.Freeze();
                    return drawingImage;
                }
                catch
                {
                    return CreateEmptyPlaceholder();
                }
            }
            return CreateEmptyPlaceholder();
        }

        private Rect CalculateBounds(MapScene scene)
        {
            double minX = double.MaxValue, minY = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue;
            bool hasContent = false;

            foreach (var area in scene.Landscape)
            {
                if (area.Points == null) continue;
                foreach (var p in area.Points)
                {
                    minX = Math.Min(minX, p.X); minY = Math.Min(minY, p.Y);
                    maxX = Math.Max(maxX, p.X); maxY = Math.Max(maxY, p.Y);
                    hasContent = true;
                }
            }

            foreach (var obj in scene.Objects)
            {
                double w = obj.Width > 0 ? obj.Width : 100;
                double h = obj.Height > 0 ? obj.Height : 100;
                minX = Math.Min(minX, obj.X); minY = Math.Min(minY, obj.Y);
                maxX = Math.Max(maxX, obj.X + w); maxY = Math.Max(maxY, obj.Y + h);
                hasContent = true;
            }

            if (!hasContent) return new Rect(0, 0, FullCanvasWidth, FullCanvasHeight);

            minX = Math.Max(0, minX - Margin);
            minY = Math.Max(0, minY - Margin);
            maxX = Math.Min(FullCanvasWidth, maxX + Margin);
            maxY = Math.Min(FullCanvasHeight, maxY + Margin);

            return new Rect(minX, minY, Math.Max(1, maxX - minX), Math.Max(1, maxY - minY));
        }

        private DrawingImage CreateEmptyPlaceholder()
        {
            var dg = new DrawingGroup();
            dg.Children.Add(new GeometryDrawing(GetTextureBrush("background_base"), null, new RectangleGeometry(new Rect(0, 0, 100, 100))));
            var di = new DrawingImage(dg);
            di.Freeze();
            return di;
        }

        private Brush GetTextureBrush(string type)
        {
            var brush = new DrawingBrush { TileMode = TileMode.Tile, Viewport = new Rect(0, 0, 30, 30), ViewportUnits = BrushMappingMode.Absolute };
            var dbGroup = new DrawingGroup();

            switch (type.ToLower())
            {
                case "background_base":
                    return new LinearGradientBrush(Color.FromRgb(235, 230, 220), Color.FromRgb(225, 220, 210), 45);
                case "grass":
                    dbGroup.Children.Add(new GeometryDrawing(new LinearGradientBrush(Color.FromRgb(100, 180, 100), Color.FromRgb(80, 160, 80), 90), null, new RectangleGeometry(new Rect(0, 0, 30, 30))));
                    dbGroup.Children.Add(new GeometryDrawing(Brushes.DarkGreen, null, new GeometryGroup { Children = { new EllipseGeometry(new Rect(5, 5, 2, 2)), new EllipseGeometry(new Rect(20, 20, 3, 3)), new EllipseGeometry(new Rect(15, 8, 2, 2)) } }));
                    brush.Drawing = dbGroup;
                    break;
                case "water":
                    dbGroup.Children.Add(new GeometryDrawing(new LinearGradientBrush(Color.FromRgb(100, 180, 255), Color.FromRgb(80, 160, 240), 90), null, new RectangleGeometry(new Rect(0, 0, 30, 30))));
                    var wave = Geometry.Parse("M 0,15 C 5,10 10,20 15,15 C 20,10 25,20 30,15");
                    // ВИПРАВЛЕНО: Використання Color.FromArgb замість Color.FromRgb(4 аргументи)
                    dbGroup.Children.Add(new GeometryDrawing(null, new Pen(new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)), 2), wave));
                    brush.Drawing = dbGroup;
                    break;
                case "soil":
                    dbGroup.Children.Add(new GeometryDrawing(new LinearGradientBrush(Color.FromRgb(120, 90, 70), Color.FromRgb(100, 70, 50), 45), null, new RectangleGeometry(new Rect(0, 0, 30, 30))));
                    // ВИПРАВЛЕНО: Використання напівпрозорого Brush замість властивості Opacity
                    dbGroup.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromArgb(76, 0, 0, 0)), null, new GeometryGroup { Children = { new EllipseGeometry(new Rect(2, 5, 1, 1)), new EllipseGeometry(new Rect(18, 12, 2, 2)), new EllipseGeometry(new Rect(10, 25, 1.5, 1.5)) } }));
                    brush.Drawing = dbGroup;
                    break;
                case "path":
                    dbGroup.Children.Add(new GeometryDrawing(new LinearGradientBrush(Color.FromRgb(240, 230, 200), Color.FromRgb(220, 210, 180), 0), null, new RectangleGeometry(new Rect(0, 0, 30, 30))));
                    // ВИПРАВЛЕНО: Використання напівпрозорого Brush замість властивості Opacity
                    dbGroup.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromArgb(51, 128, 128, 128)), null, new GeometryGroup { Children = { new EllipseGeometry(new Rect(5, 5, 1, 1)), new EllipseGeometry(new Rect(25, 25, 1, 1)), new EllipseGeometry(new Rect(15, 15, 1, 1)) } }));
                    brush.Drawing = dbGroup;
                    break;
                case "building":
                    return new LinearGradientBrush(Color.FromRgb(180, 100, 80), Color.FromRgb(140, 70, 50), 90);
                case "tree_top":
                    return new RadialGradientBrush(Color.FromRgb(100, 180, 80), Color.FromRgb(60, 120, 50));
                case "veg_top":
                    return new RadialGradientBrush(Color.FromRgb(255, 200, 100), Color.FromRgb(200, 150, 50));
                default:
                    return Brushes.Gray;
            }
            brush.Freeze();
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}