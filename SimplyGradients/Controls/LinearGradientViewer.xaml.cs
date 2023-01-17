using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        }

        #region Bindable properties

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(LinearGradientViewer),
            new FrameworkPropertyMetadata(new CornerRadius(5), FrameworkPropertyMetadataOptions.AffectsRender));

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

        public delegate void GradientCollectionUpdatedEventHandler(object sender);

        public event GradientCollectionUpdatedEventHandler GradientCollectionUpdated;

        private Slider _selectedSlider;

        private void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Slider slider = sender as Slider;
            if (_selectedSlider == slider) return;
            if (_selectedSlider != null)
            {
                Panel.SetZIndex(_selectedSlider.TemplatedParent as ContentPresenter, 0);
                _selectedSlider.Background.Changed -= Background_Changed;
            }
            slider.Background.Changed += Background_Changed;
            Panel.SetZIndex(slider.TemplatedParent as ContentPresenter, 1);
            SelectedGradientStop = slider.DataContext as GradientStop;
            _selectedSlider = slider;
        }

        private void Background_Changed(object? sender, System.EventArgs e)
        {
            gradientPresenter.InvalidateVisual();
            GradientCollectionUpdated?.Invoke(this);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            gradientPresenter.InvalidateVisual();
            GradientCollectionUpdated?.Invoke(this);
        }

    }
}
