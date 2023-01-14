using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace SimplyGradients
{
    public class ColorModel : INotifyPropertyChanged
    {

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

        private void ToBrush()
        {
            SolidColor = Color.FromArgb(A, R, G, B);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolidColor"));


            var colors = new List<(Color, int)>()
            {
               new ( Color.FromRgb(255, 0, 0), 0 ),
               new ( Color.FromRgb(255, 255, 0), 0 ),
               new ( Color.FromRgb(0, 255, 0), 0 ),
               new ( Color.FromRgb(0, 255, 255), 0 ),
               new ( Color.FromRgb(0, 0, 255), 0 ),
               new ( Color.FromRgb(255, 0, 255), 0 ),
               new ( Color.FromRgb(255, 0, 0), 0 ),
            };

            for (int i = 0; i < colors.Count; i++)
            {
                var tuple = colors[i];
                var color = colors[i].Item1;
                tuple.Item2 = (int)Math.Abs(color.R - R) + Math.Abs(color.G - G) + Math.Abs(color.B - B);
            }

            var nearestAccent = colors.MinBy(p => p.Item2).Item1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
