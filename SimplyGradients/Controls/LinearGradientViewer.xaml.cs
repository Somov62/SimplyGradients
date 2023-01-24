using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SimplyGradients.Mvvm.Behaviors;

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

        private void Slider_Checked(CheckedEventArgs e)
        {
            e.NewValue.Background.Changed += Background_Changed;
            if (e.OldValue != null)
                e.OldValue.Background.Changed -= Background_Changed;
            SelectedGradientStop = e.NewValue.DataContext as GradientStop;
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            CheckedBehavior.SetIsChecked(sender as Slider, true);
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = ((Border)sender).Child as TextBox;
            textbox.Focus();
        }

        private void GradientPresenter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(gradientPresenter);
            double offset = position.X / gradientPresenter.ActualWidth;

            int insertIndex = 0;

            for (int i = 0; i < GradientStops.Count; i++)
            {
                if (GradientStops[i].Offset >= offset)
                {
                    insertIndex = i;
                    break;
                }
            }

            GradientStop left = GradientStops[0];
            GradientStop right = GradientStops[insertIndex];
            if (insertIndex != 0)
                left = GradientStops[insertIndex - 1];
            double percent = (offset - left.Offset) / (right.Offset - left.Offset);
            byte a = (byte)(left.Color.A + percent * (right.Color.A - left.Color.A));
            byte r = (byte)(left.Color.R + percent * (right.Color.R - left.Color.R));
            byte g = (byte)(left.Color.G + percent * (right.Color.G - left.Color.G));
            byte b = (byte)(left.Color.B + percent * (right.Color.B - left.Color.B));
            Color color = Color.FromArgb(a, r, g, b);
            GradientStops.Insert(insertIndex, new GradientStop(color, offset));
            GradientCollectionUpdated?.Invoke(this);
            sliderItemsControl.Items.Refresh();
        }
    }
}
