using SimplyGradients.Mvvm;
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace SimplyGradients.Models
{
    public class ColorModel : ObservableObject
    {
        public ColorModel()
        {
            A = 255;
        }

        public ColorModel(Color color)
        {
            _a = color.A;
            _r = color.R;
            _g = color.G;
            _b = color.B;
            RGBtoHSV(_a, _r, _g, _b);
            SolidColor = Color.FromArgb(A, R, G, B);
        }

        public ColorModel(byte a, byte r, byte g, byte b)
        {
            _a = a;
            _r = r;
            _g = g;
            _b = b;
            RGBtoHSV(_a, _r, _g, _b);
            SolidColor = Color.FromArgb(A, R, G, B);
        }

        public Color _solidColor;
        public Color SolidColor 
        {
            get => _solidColor;
            private set
            {
                Set(ref _solidColor, value, nameof(SolidColor));
                string hex = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
                Set(ref _hex, hex, nameof(Hex));
            }
        }

        public Color _hueColor;
        public Color HueColor
        {
            get => _hueColor;
            private set => Set(ref _hueColor, value, nameof(HueColor));
        }

        private byte _a;
        public byte A
        {
            get => _a;
            set
            {
                Set(ref _a, value, nameof(A));
                SolidColor = Color.FromArgb(A, R, G, B);
            }
        }

        private byte _r;
        public byte R
        {
            get => _r;
            set
            {
                Set(ref _r, value, nameof(R));
                RGBtoHSV(A, R, G, B);
                SolidColor = Color.FromArgb(A, R, G, B);
            }
        }

        private byte _g;
        public byte G
        {
            get => _g;
            set
            {
                Set(ref _g, value, nameof(G));
                RGBtoHSV(A, R, G, B);
                SolidColor = Color.FromArgb(A, R, G, B);
            }
        }

        private byte _b;
        public byte B
        {
            get => _b;
            set
            {
                Set(ref _b, value, nameof(B));
                RGBtoHSV(A, R, G, B);
                SolidColor = Color.FromArgb(A, R, G, B);
            }
        }


        private string _hex;

        public string Hex
        {
            get => _hex;
            set
            {
                Set(ref _hex, value, nameof(Hex));
                Color color;
                try
                {
                    color = (Color)ColorConverter.ConvertFromString(value);
                }
                catch { return; }
                Set(ref _r, color.R, nameof(R));
                Set(ref _g, color.G, nameof(G));
                Set(ref _b, color.B, nameof(B));
                Set(ref _solidColor, color, nameof(SolidColor));
                RGBtoHSV(_a, _r, _g, _b);
            }
        }


        private double _hue;
        public double Hue
        {
            get => _hue;
            set
            {
                Set(ref _hue, value, nameof(Hue));
                Set(ref _r, SolidColor.R, nameof(R));
                Set(ref _g, SolidColor.G, nameof(G));
                Set(ref _b, SolidColor.B, nameof(B));
                SolidColor = HSBtoRGB(Hue, Saturation, Brightness, A);
                HueColor = HSBtoRGB(Hue, 1, 1, 255);
            }
        }

        private double _saturation;
        public double Saturation
        {
            get => _saturation;
            set
            {
                Set(ref _saturation, value, nameof(Saturation));
                SolidColor = HSBtoRGB(Hue, Saturation, Brightness, A);
                Set(ref _r, SolidColor.R, nameof(R));
                Set(ref _g, SolidColor.G, nameof(G));
                Set(ref _b, SolidColor.B, nameof(B));
            }
        }

        private double _brightness;
        public double Brightness
        {
            get => _brightness;
            set
            {
                Set(ref _brightness, value, nameof(Brightness));
                SolidColor = HSBtoRGB(Hue, Saturation, Brightness, A);
                Set(ref _r, SolidColor.R, nameof(R));
                Set(ref _g, SolidColor.G, nameof(G));
                Set(ref _b, SolidColor.B, nameof(B));
            }
        }

        private Color HSBtoRGB(double hue, double saturation, double brigthness, double alpha)
        {
            double red = 0;
            double green = 0;
            double blue = 0;

            if (saturation == 0)
            {
                red = green = blue = brigthness * 255;
                return Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
            }

            // цветовой круг состоит из 6 секторов. Выяснить, в каком секторе
            // находится.
            double sectorPos = hue / 60.0;
            int sectorNumber = (int)(Math.Floor(sectorPos));
            // получить дробную часть сектора
            double fractionalSector = sectorPos - sectorNumber;

            // вычислить значения для трех осей цвета.
            double p = brigthness * (1.0 - saturation);
            double q = brigthness * (1.0 - (saturation * fractionalSector));
            double t = brigthness * (1.0 - (saturation * (1 - fractionalSector)));

            // присвоить дробные цвета r, g и b на основании сектора
            // угол равняется.
            switch (sectorNumber)
            {
                case 0:
                case 6:
                    red = brigthness;
                    green = t;
                    blue = p;
                    break;
                case 1:
                    red = q;
                    green = brigthness;
                    blue = p;
                    break;
                case 2:
                    red = p;
                    green = brigthness;
                    blue = t;
                    break;
                case 3:
                    red = p;
                    green = q;
                    blue = brigthness;
                    break;
                case 4:
                    red = t;
                    green = p;
                    blue = brigthness;
                    break;
                case 5:
                    red = brigthness;
                    green = p;
                    blue = q;
                    break;
            }
            red *= 255;
            green *= 255;
            blue *= 255;
            
            return Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        private void RGBtoHSV(byte alpha, byte red, byte green, byte blue)
        {
            // нормализовать значения красного, зеленого и синего
            double r = ((double)red / 255.0);
            double g = ((double)green / 255.0);
            double b = ((double)blue / 255.0);

            // начало преобразования
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0.0;
            if (max == r && g >= b)
            {
                h = 60 * (g - b) / (max - min);
            }
            else if (max == r && g < b)
            {
                h = 60 * (g - b) / (max - min) + 360;
            }
            else if (max == g)
            {
                h = 60 * (b - r) / (max - min) + 120;
            }
            else if (max == b)
            {
                h = 60 * (r - g) / (max - min) + 240;
            }
            if (double.IsNaN(h)) h = 0;
            double s = (max == 0) ? 0.0 : (1.0 - (min / max));

            Set(ref _hue, h, nameof(Hue));
            Set(ref _saturation, s, nameof(Saturation));
            Set(ref _brightness, max, nameof(Brightness));
            HueColor = HSBtoRGB(Hue, 1, 1, 255);
        }

    }
}
