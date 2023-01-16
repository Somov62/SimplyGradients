using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimplyGradients.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            SelectedGradientStop = GradientStops[0];
        }

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
                _selectedColor = value;
                _selectedColor.PropertyChanged += SelectedColor_PropertyChanged;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedColor)));
            }
        }

        private void SelectedColor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SelectedGradientStop.Color = SelectedColor.SolidColor;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
