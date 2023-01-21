using SimplyGradients.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplyGradients.Controls
{
    /// <summary>
    /// Логика взаимодействия для ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private Point _previewCursorPoint = new Point(-1, -1);
        private Color _previewColor;

        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
          "AccentColor",
          typeof(Color),
          typeof(ColorPicker),
          new FrameworkPropertyMetadata(Color.FromArgb(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
         "SelectedColor",
         typeof(ColorModel),
         typeof(ColorPicker),
         new FrameworkPropertyMetadata(new ColorModel(255, 255, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public ColorModel SelectedColor
        {
            get { return (ColorModel)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        private void PointCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point cursorPosition = e.GetPosition(pointCanvas);
                cursorPosition.X = Math.Round(cursorPosition.X, 1);
                cursorPosition.Y = Math.Round(cursorPosition.Y, 1);
                CalculateColor(cursorPosition);
            }
        }

        private void PointCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point cursorPosition = e.GetPosition(pointCanvas);
            cursorPosition.X = Math.Round(cursorPosition.X, 1);
            cursorPosition.Y = Math.Round(cursorPosition.Y, 1);
            CalculateColor(cursorPosition);
        }

        private void CalculateColor(Point cursorPosition)
        {

            if (cursorPosition.X > pointCanvas.ActualWidth) cursorPosition.X = pointCanvas.ActualWidth;
            if (cursorPosition.Y > pointCanvas.ActualHeight) cursorPosition.Y = pointCanvas.ActualHeight;
            if (cursorPosition.X < 0) cursorPosition.X = 0;
            if (cursorPosition.Y < 0) cursorPosition.Y = 0;

            double offsetX = cursorPosition.X / pointCanvas.ActualWidth;
            double offsetY = 1 - cursorPosition.Y / pointCanvas.ActualHeight;
            this.SelectedColor.Saturation = offsetX;
            this.SelectedColor.Brightness = offsetY;
        }
    }
}
