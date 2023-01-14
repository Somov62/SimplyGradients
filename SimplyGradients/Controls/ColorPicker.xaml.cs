using System;
using System.Collections.Generic;
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


        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
          "AccentColor",
          typeof(ColorModel),
          typeof(ColorPicker),
          new FrameworkPropertyMetadata(new ColorModel(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public ColorModel AccentColor
        {
            get { return (ColorModel)GetValue(AccentColorProperty); }
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
                Point cursorPosition = e.GetPosition(sender as Canvas);
                if (cursorPosition.X > pointCanvas.ActualWidth)
                {
                    cursorPosition.X -= cursorPosition.X - pointCanvas.ActualWidth;
                }
                if (cursorPosition.X < 0)
                {
                    cursorPosition.X = 0;
                }

                if (cursorPosition.Y > pointCanvas.ActualHeight)
                {                  
                    cursorPosition.Y -= cursorPosition.Y - pointCanvas.ActualHeight;
                }                  
                if (cursorPosition.Y < 0)
                {                  
                    cursorPosition.Y = 0;
                }

                cursorPosition.Y -= colorPickerPoint.Height / 2;
                cursorPosition.X -= colorPickerPoint.Width / 2;

                Canvas.SetTop(colorPickerPoint, cursorPosition.Y);
                Canvas.SetLeft(colorPickerPoint, cursorPosition.X);

                double offsetX = cursorPosition.X / (pointCanvas.ActualWidth / 100);
                double offsetY = cursorPosition.Y / (pointCanvas.ActualHeight / 100);
                SaveColor(CalculateColor(offsetX, offsetY));
            }
        }

        private void pointCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point cursorPosition = e.GetPosition(sender as Canvas);

            cursorPosition.Y -= colorPickerPoint.Height / 2;
            cursorPosition.X -= colorPickerPoint.Width / 2;


            Canvas.SetLeft(colorPickerPoint, cursorPosition.X);
            Canvas.SetTop(colorPickerPoint, cursorPosition.Y);

            double offsetX = cursorPosition.X / (pointCanvas.ActualWidth / 100);
            double offsetY = cursorPosition.Y / (pointCanvas.ActualHeight/ 100);
            SaveColor(CalculateColor(offsetX, offsetY));

        }

        private void SaveColor(Color color)
        {
            SelectedColor.A = color.A;
            SelectedColor.R = color.R;
            SelectedColor.G = color.G;
            SelectedColor.B = color.B;
        }

        private Color CalculateColor(double offsetX, double offsetY)
        {
            var color = ColorsInterpolation(Color.FromRgb(255, 255, 255), AccentColor.SolidColor, offsetX);
            return ColorsInterpolation(Color.FromRgb(0, 0, 0), color, offsetY);
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
