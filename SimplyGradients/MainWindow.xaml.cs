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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double percent = e.NewValue / 100;

            var col1 = SliderLeftColor;
            var col2 = SliderRightColor;

            byte a = (byte)(col1.A + percent * (col2.A - col1.A));
            byte r = (byte)(col1.R + percent * (col2.R - col1.R));
            byte g = (byte)(col1.G + percent * (col2.G - col1.G));
            byte b = (byte)(col1.B + percent * (col2.B - col1.B));
            selectedColorPresenter.Background = new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

    }
}
