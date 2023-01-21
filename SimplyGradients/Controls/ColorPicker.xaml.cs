using SimplyGradients.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplyGradients.Controls
{
    /// <summary>
    /// Логика взаимодействия для ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private Point _previewCursorPoint = new Point(-1, -1);
        private Color _previewColor;

        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
          "AccentColor",
          typeof(Color),
          typeof(ColorPicker),
          new FrameworkPropertyMetadata(Color.FromArgb(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(PropertyChangedCallback)));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker control = (ColorPicker)d;
            if (control._previewCursorPoint != new Point(-1, -1))
            {
                Color color = control.CalculateColor(control._previewCursorPoint);
                if (control._previewColor == control.SelectedColor.SolidColor)
                {
                    control.SaveColor(color);
                    return;
                }
            }
            control._previewColor = control.SelectedColor.SolidColor;
            Color selected = control.SelectedColor.SolidColor;
            Color accent = control.SelectedColor.NearestAccentColor;
            List<(double, double)> percentsR = new();
            List<(double, double)> percentsG = new();
            List<(double, double)> percentsB = new();

            for (double i = 0; i <= 1; i += 0.002)
            {
                for (double j = 0; j <= 1; j += 0.002)
                {
                    if ((byte)((255 + accent.R * i - 255 * i) * (1 - j)) == selected.R)
                    {
                        percentsR.Add((Math.Round(i, 6), Math.Round(j, 6)));
                    }

                    if ((byte)((255 + accent.G * i - 255 * i) * (1 - j)) == selected.G)
                    {
                        percentsG.Add((Math.Round(i, 6), Math.Round(j, 6)));
                    }

                    if ((byte)((255 + accent.B * i - 255 * i) * (1 - j)) == selected.B)
                    {
                        percentsB.Add((Math.Round(i, 6), Math.Round(j, 6)));
                    }
                }
            }

            var intersections = percentsR.Intersect(percentsB.Intersect(percentsG)).ToList();
            if (intersections.Count == 0)
            {
                Debug.Fail("Цвет не подобран");
                Debug.WriteLine($"fail accent {accent.R}, {accent.G}, {accent.B}");
                return;
            }
            Debug.WriteLine($"Move to {intersections[0].Item1}  {intersections[0].Item2} ");
            control._previewCursorPoint.X = control.pointCanvas.ActualWidth * intersections[0].Item1;
            control._previewCursorPoint.Y = control.pointCanvas.ActualHeight * intersections[0].Item2;
            Canvas.SetLeft(control.colorPickerPoint, control._previewCursorPoint.X - control.colorPickerPoint.Width / 2);
            Canvas.SetTop(control.colorPickerPoint, control._previewCursorPoint.Y - control.colorPickerPoint.Height / 2);

        }

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
         "SelectedColor",
         typeof(ColorModel),
         typeof(ColorPicker),
         new FrameworkPropertyMetadata(new ColorModel(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public ColorModel SelectedColor
        {
            get { return (ColorModel)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }



        private void PointCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point cursorPosition = e.GetPosition(pointCanvas);
                cursorPosition.X = Math.Round(cursorPosition.X, 1);
                cursorPosition.Y = Math.Round(cursorPosition.Y, 1);
                if (_previewCursorPoint == cursorPosition) return;
                SaveColor(CalculateColor(cursorPosition));
            }
        }

        private void PointCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point cursorPosition = e.GetPosition(pointCanvas);
            cursorPosition.X = Math.Round(cursorPosition.X, 1);
            cursorPosition.Y = Math.Round(cursorPosition.Y, 1);
            if (_previewCursorPoint == cursorPosition) return;
            SaveColor(CalculateColor(cursorPosition));
        }

        private Color CalculateColor(Point cursorPosition)
        {
            _previewCursorPoint = cursorPosition;

            if (cursorPosition.X > pointCanvas.ActualWidth) cursorPosition.X = pointCanvas.ActualWidth;
            if (cursorPosition.Y > pointCanvas.ActualHeight) cursorPosition.Y = pointCanvas.ActualHeight;
            if (cursorPosition.X < 0) cursorPosition.X = colorPickerPoint.Width / 2;
            if (cursorPosition.Y < 0) cursorPosition.Y = colorPickerPoint.Height / 2;
            cursorPosition.X -= colorPickerPoint.Width / 2;
            cursorPosition.Y -= colorPickerPoint.Height / 2;

            Canvas.SetLeft(colorPickerPoint, cursorPosition.X);
            Canvas.SetTop(colorPickerPoint, cursorPosition.Y);

            double offsetX = cursorPosition.X / (pointCanvas.ActualWidth);
            double offsetY = cursorPosition.Y / (pointCanvas.ActualHeight);
            Debug.Write($"Ox = {offsetX} Oy = {offsetY}");
            var color = ColorsInterpolation(Color.FromRgb(255, 255, 255), AccentColor, offsetX);
            color = ColorsInterpolation(color, Color.FromRgb(0, 0, 0), offsetY);
            Debug.WriteLine($"  Color {color.R} {color.G} {color.B}");
            return color;
        }

        private void SaveColor(Color color)
        {
            _previewColor = color;
            SelectedColor.SetColorWithOutComputing(color.A, color.R, color.G, color.B);
        }

        private Color ColorsInterpolation(Color a, Color b, double percent)
        {
            return Color.FromArgb(
                LinearInterpolation(a.A, b.A, percent),
                LinearInterpolation(a.R, b.R, percent),
                LinearInterpolation(a.G, b.G, percent),
                LinearInterpolation(a.B, b.B, percent)
            );

            byte LinearInterpolation(byte a, byte b, double percent)
            {
                return (byte)(a + percent * (b - a));
            }
        }

    }
}
