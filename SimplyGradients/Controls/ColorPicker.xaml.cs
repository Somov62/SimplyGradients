using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimplyGradients.Models;

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

        private Point _previewCursorPoint = new Point(0, 0);

        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
          "AccentColor",
          typeof(Color),
          typeof(ColorPicker),
          new FrameworkPropertyMetadata(Color.FromArgb(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(PropertyChangedCallback)));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ColorPicker)d;
            control.CalculateColor(control._previewCursorPoint);
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
                CalculateColor(e.GetPosition(pointCanvas));
            }
        }

        private void pointCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateColor(e.GetPosition(pointCanvas));
        }

        private void CalculateColor(Point cursorPosition)
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
