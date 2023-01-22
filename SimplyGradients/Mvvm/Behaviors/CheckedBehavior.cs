using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SimplyGradients.Mvvm.Behaviors
{
    public class CheckedBehavior : Behavior<Control>
    {
        private static readonly List<Control> _elements = new List<Control>();

        public static readonly DependencyProperty IsCheckedProperty =
           DependencyProperty.RegisterAttached("IsChecked",
           typeof(bool), typeof(CheckedBehavior),
           new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(IsCheckedCallback)));

        public static bool GetIsChecked(Control target) => 
            (bool)target.GetValue(IsCheckedProperty);

        public static void SetIsChecked(Control target, bool value) =>
            target.SetValue(IsCheckedProperty, value);

        private static void IsCheckedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue != true) 
                return;

            Control element = d as Control;
            Control old = null;

            if (!_elements.Contains(element))
                _elements.Add(element);

            foreach (var item in _elements)
            {
                if (item == element) continue;
                if (GetIsChecked(item)) old = item;
                SetIsChecked(item, false);
            }
            var behavior = Interaction.GetBehaviors(element)[0] as CheckedBehavior;
            behavior.CheckedChanged?.Invoke(new CheckedEventArgs() { NewValue = element, OldValue = old });
        }

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }


        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Control element = sender as Control;
            if (GetIsChecked(element) == false)
                SetIsChecked(element, true);
        }

        public delegate void CheckedEventHandler(CheckedEventArgs e);
        public event CheckedEventHandler CheckedChanged;
    }

    public class CheckedEventArgs
    {
        public Control NewValue { get; set; }
        public Control? OldValue { get; set; }
    }
}
