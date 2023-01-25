using SimplyGradients.Models;
using SimplyGradients.Mvvm;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplyGradients.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ICommand DeleteGradientStopCommand { get; }
        public ICommand ConvertToXamlCommand { get; }


        public MainViewModel()
        {
            DeleteGradientStopCommand = new RelayCommand(gradientStop =>
            {
                if (GradientStops.Count > 2)
                {
                    GradientStops.Remove(gradientStop as GradientStop);
                    GradientStops = new GradientStopCollection(GradientStops);
                    OnPropertyChanged(nameof(GradientStops));
                }
            });
            ConvertToXamlCommand = new RelayCommand(o => { XamlGradient = ConvertToXaml(o); });
            SelectedGradientStop = GradientStops.Last();
        }

        private string ConvertToXaml(object _)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append("<LinearGradientBrush>\n");
            stringBuilder.Append("\t<LinearGradientBrush.RelativeTransform>\n");
            stringBuilder.Append($"\t\t<RotateTransform Angle=\"{Angle}\" CenterX=\"0.5\" CenterY=\"0.5\" />\n");
            stringBuilder.Append("\t</LinearGradientBrush.RelativeTransform>\n");
            stringBuilder.Append("\t<LinearGradientBrush.GradientStops>\n");
            foreach (var item in GradientStops)
            {
                stringBuilder.Append($"\t\t<GradientStop Color=\"{item.Color}\" Offset=\"{item.Offset}\" />\n");
            }
            stringBuilder.Append("\t</LinearGradientBrush.GradientStops>\n");
            stringBuilder.Append("</LinearGradientBrush>");
            return stringBuilder.ToString();
        }

        public GradientStopCollection GradientStops { get; private set; } = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(0, 255, 0), 0),
            new GradientStop(Color.FromRgb(0, 0, 255), 1)
        };

        private double _angle;

        public double Angle
        {
            get => _angle;
            set => Set(ref _angle, value, nameof(Angle));
        }


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

        private string _xamlGradient;

        public string XamlGradient
        {
            get => _xamlGradient;
            set => Set(ref _xamlGradient, value, nameof(XamlGradient));
        }

    }
}
