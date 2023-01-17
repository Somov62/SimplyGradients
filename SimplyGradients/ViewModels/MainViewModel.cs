using SimplyGradients.Models;
using SimplyGradients.Mvvm;
using System.Diagnostics;
using System.Windows.Media;

namespace SimplyGradients.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel() { }

        public GradientStopCollection GradientStops { get; } = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(0, 0, 0), 0),
            new GradientStop(Color.FromRgb(255, 255, 255), 1)
        };

        private GradientStop _selectedGradientStop;
        public GradientStop SelectedGradientStop
        {
            get => _selectedGradientStop;
            set
            {
                _selectedGradientStop = value;
                SelectedColor = new ColorModel(value.Color);
            }
        }

        private ColorModel _selectedColor;
        public ColorModel SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != null)
                    _selectedColor.PropertyChanged -= SelectedColor_PropertyChanged;
                base.Set(ref _selectedColor, value, nameof(SelectedColor));
                _selectedColor.PropertyChanged += SelectedColor_PropertyChanged;
                OnPropertyChanged(nameof(GradientStops));
            }
        }

        private void SelectedColor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SelectedGradientStop.Color = SelectedColor.SolidColor;
            var color = SelectedGradientStop.Color;
            Debug.WriteLine($" M Color {color.R} {color.G} {color.B} {e.PropertyName}");

        }
    }
}
