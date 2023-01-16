﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimplyGradients.Mvvm
{
    public class ObservableObject : INotifyPropertyChanged
    {

        public bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string property = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
