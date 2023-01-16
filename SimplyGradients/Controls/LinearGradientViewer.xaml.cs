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
    /// Логика взаимодействия для LinearGradientViewer.xaml
    /// </summary>
    public partial class LinearGradientViewer : UserControl
    {
        public LinearGradientViewer()
        {
            InitializeComponent();
            CornerRadius = new CornerRadius(5);
        }


        #region Bindable properties

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(LinearGradientViewer),
            new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty GradientStopsProperty = DependencyProperty.Register(
            nameof(GradientStops),
            typeof(GradientStopCollection),
            typeof(LinearGradientViewer),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SelectedGradientStopProperty = DependencyProperty.Register(
            nameof(SelectedGradientStop),
            typeof(GradientStop),
            typeof(LinearGradientViewer),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public GradientStopCollection GradientStops
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }
        public GradientStop SelectedGradientStop
        {
            get { return (GradientStop)GetValue(SelectedGradientStopProperty); }
            set { SetValue(SelectedGradientStopProperty, value); }
        }

        #endregion



        private Slider _selectedSlider;

        private void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Slider slider = sender as Slider;

            if (_selectedSlider != null)
                Panel.SetZIndex(_selectedSlider.TemplatedParent as ContentPresenter, 0);
            Panel.SetZIndex(slider.TemplatedParent as ContentPresenter, 1);
            SelectedGradientStop = slider.DataContext as GradientStop;
            _selectedSlider = slider;
        }
    }
}
