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

namespace SimplyGradients
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public ColorModel SliderLeftColor { get; set; } = new ColorModel();
        public ColorModel SliderRightColor { get; set; } = new ColorModel();
        public ColorModel SelectedColor { get; set; } = new ColorModel();


        public GradientStopCollection RainbowStops { get; } = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(255, 0, 0), 0),
            new GradientStop(Color.FromRgb(255, 255, 0), 0.17),
            new GradientStop(Color.FromRgb(0, 255, 0), 0.33),
            new GradientStop(Color.FromRgb(0, 255, 255), 0.50),
            new GradientStop(Color.FromRgb(0, 0, 255), 0.66),
            new GradientStop(Color.FromRgb(255, 0, 255), 0.83),
            new GradientStop(Color.FromRgb(255, 0, 0), 1),
        };

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double percent = e.NewValue;

            var min = percent / 7;
            double max;

            int minIndex;
            int maxIndex;
            if (min - Math.Round(min, 0, MidpointRounding.ToZero) < 0.5)
            {
                max = min + 1;
            }
            else
            {
                max = min;
                min--;
            }

            percent = min;
            var col1 = RainbowStops[(int)min].Color;
            var col2 = RainbowStops[(int)max].Color;

            SelectedColor.A = (byte)(col1.A + percent * (col2.A - col1.A));
            SelectedColor.R = (byte)(col1.R + percent * (col2.R - col1.R));
            SelectedColor.G = (byte)(col1.G + percent * (col2.G - col1.G));
            SelectedColor.B = (byte)(col1.B + percent * (col2.B - col1.B));
            
        }


    }
}
