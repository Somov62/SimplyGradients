using System.ComponentModel;
using System.Windows.Media;

namespace SimplyGradients
{
    public class ColorModel : INotifyPropertyChanged
    {

        public ColorModel()
        {
            A = 255;
        }
        public Color SolidColor { get; set; }


        private byte _a;
        public byte A
        {
            get => _a;
            set
            {
                _a = value;
                ToBrush();
            }
        }

        private byte _r;
        public byte R
        {
            get => _r;
            set
            {
                _r = value;
                ToBrush();
            }
        }

        private byte _g;
        public byte G
        {
            get => _g;
            set
            {
                _g = value;
                ToBrush();
            }
        }

        private byte _b;
        public byte B
        {
            get => _b;
            set
            {
                _b = value;
                ToBrush();
            }
        }

        private void ToBrush()
        {
            SolidColor = Color.FromArgb(A, R, G, B);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolidColor"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
