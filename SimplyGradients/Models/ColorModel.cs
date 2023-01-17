using SimplyGradients.Mvvm;
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace SimplyGradients.Models
{
    public class ColorModel : ObservableObject
    {
        public readonly Color[] _spectrum = new Color[]
        {
            Color.FromRgb(255, 0, 0),
            Color.FromRgb(255, 255, 0),
            Color.FromRgb(0, 255, 0),
            Color.FromRgb(0, 255, 255),
            Color.FromRgb(0, 0, 255),
            Color.FromRgb(255, 0, 255),
            Color.FromRgb(255, 0, 0)
        };

        public ColorModel()
        {
            A = 255;
        }

        public ColorModel(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public ColorModel(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }


        private double _offsetX;

        public double OffsetX
        {
            get => _offsetX;
            set
            {
                if (value == _offsetX) return;
                Set(ref _offsetX, value, nameof(OffsetX));


            }
        }


        public Color SolidColor { get; private set; }

        public Color NearestAccentColor { get; private set; }

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

        private double _accentPercent;

        public double AccentPercent
        {
            get => _accentPercent;
            set
            {
                base.Set(ref _accentPercent, value);
                int max = (int)Math.Round(value / (100.0 / 6), 0, MidpointRounding.ToZero) + 1;

                if (max == 0)
                    max++;

                if (max == 7)
                    max--;

                int min = max - 1;

                double percent = Math.Round(value % (100.0 / 6) / 17, 2, MidpointRounding.AwayFromZero);
                Color col1 = _spectrum[min];
                Color col2 = _spectrum[max];
                byte r = (byte)(col1.R + percent * (col2.R - col1.R));
                byte g = (byte)(col1.G + percent * (col2.G - col1.G));
                byte b = (byte)(col1.B + percent * (col2.B - col1.B));
                NearestAccentColor = Color.FromRgb(r, g, b);
                base.OnPropertyChanged(nameof(NearestAccentColor));

            }
        }



        private void ToBrush()
        {
            SolidColor = Color.FromArgb(A, R, G, B);
            base.OnPropertyChanged(nameof(SolidColor));

            double x = (_g - _b) * Math.Sqrt(3.0) / 2.0;
            double y = _r - _g / 2.0 - _b / 2.0;
            double angle = 0;
            if (x <= 0 && y >= 0)
            {
                angle = 270 + Math.Atan(Math.Abs(y / x)) * 180.0 / Math.PI;
            }
            if (x <= 0 && y <= 0)
            {
                angle = 180 + Math.Atan(Math.Abs(x / y)) * 180.0 / Math.PI;
            }

            if (x >= 0 && y <= 0)
            {
                angle = 90 + Math.Atan(Math.Abs(y / x)) * 180.0 / Math.PI;
            }

            if (x >= 0 && y >= 0)
            {
                angle = Math.Atan(Math.Abs(x / y)) * 180.0 / Math.PI;
            }
            if (double.IsNaN(angle)) angle = 0;
            AccentPercent = angle / (360 / 100.0);
        }

        public void SetColorWithOutComputing(byte a, byte r, byte g, byte b)
        {
            SolidColor = Color.FromArgb(a, r, g, b);
            Debug.WriteLine($"C  Color {SolidColor.R} {SolidColor.G} {SolidColor.B}");

            base.OnPropertyChanged(nameof(SolidColor));
        }
    }
}
