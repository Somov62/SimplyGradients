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
            aboba.GradientStops = GradientStops;
        }

        public ColorModel SliderLeftColor { get; set; } = new ColorModel();
        public ColorModel SliderRightColor { get; set; } = new ColorModel();
        public GradientStop SelectedColor => aboba.SelectGradientStop;


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

        public GradientStopCollection GradientStops { get; } = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(0, 0, 0), 0),
            new GradientStop(Color.FromRgb(255, 255, 255), 1)
        };


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double percent = e.NewValue / (100 / 6);

            var min = percent;
            double max;

            int minIndex;
            int maxIndex;
            if (min - Math.Round(min, 0, MidpointRounding.AwayFromZero) < 0.5)
            {
                max = min + 1;
            }
            else
            {
                max = min;
                min--;
            }

            Color col1 = RainbowStops[(int)Math.Round(min, 0, MidpointRounding.AwayFromZero)].Color;
            Color col2 = RainbowStops[(int)Math.Round(max, 0, MidpointRounding.AwayFromZero)].Color;
            byte a = (byte)(col1.A + percent * (col2.A - col1.A));
            byte r = (byte)(col1.R + percent * (col2.R - col1.R));
            byte g = (byte)(col1.G + percent * (col2.G - col1.G));
            byte b = (byte)(col1.B + percent * (col2.B - col1.B));
            SelectedColor.Color = Color.FromArgb(a, r, g, b);
            
        }


    }
}
