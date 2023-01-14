using System;
using System.Windows;
using System.Windows.Media;

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
        public ColorModel PalleteAccentColor { get; set; } = new ColorModel();


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
            int max = (int)Math.Round(e.NewValue / (100.0 / 6), 0, MidpointRounding.ToZero) + 1;
            if (max == 0)
            {
                max++;
            }

            if (max == 7)
            {
                max--;
            }

            maxtxt.Text = max.ToString();
            int min = max - 1;

            double percent = Math.Round(e.NewValue % (100.0 / 6) / 17, 2, MidpointRounding.AwayFromZero);
            txtpercetn.Text = percent.ToString();
            valuetxt.Text = e.NewValue.ToString();
            Color col1 = RainbowStops[min].Color;
            Color col2 = RainbowStops[max].Color;
            PalleteAccentColor.A = (byte)(col1.A + percent * (col2.A - col1.A));
            PalleteAccentColor.R = (byte)(col1.R + percent * (col2.R - col1.R));
            PalleteAccentColor.G = (byte)(col1.G + percent * (col2.G - col1.G));
            PalleteAccentColor.B = (byte)(col1.B + percent * (col2.B - col1.B));
        }

    }
}
