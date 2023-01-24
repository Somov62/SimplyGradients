using SimplyGradients.Models;
using SimplyGradients.Mvvm;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplyGradients.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ICommand DeleteGradientStopCommand { get; }

        public MainViewModel() 
        {
            DeleteGradientStopCommand = new RelayCommand(execute => 
            { 
                if (GradientStops.Count > 2) 
                    GradientStops.Remove(execute as GradientStop); 
                OnPropertyChanged(nameof(GradientStops)); 
            });
            SelectedGradientStop = GradientStops.Last();
        }

        public GradientStopCollection GradientStops { get; } = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(0, 255, 0), 0),
            new GradientStop(Color.FromRgb(0, 0, 255), 1)
        };

        private GradientStop _selectedGradientStop;
        public GradientStop SelectedGradientStop
        {
            get => _selectedGradientStop;
            set
            {
                Set(ref _selectedGradientStop, value, nameof(SelectedGradientStop));
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
            if (e.PropertyName == nameof(ColorModel.SolidColor))
            {
                SelectedGradientStop.Color = SelectedColor.SolidColor;
            }
        }
    }
}
