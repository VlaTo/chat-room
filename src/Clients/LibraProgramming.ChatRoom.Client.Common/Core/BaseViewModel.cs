using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibraProgramming.ChatRoom.Client.Common.Core
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? new MockDataStore();

        protected bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            var comparer = EqualityComparer<T>.Default;

            if (comparer.Equals(field, value))
            {
                return false;
            }

            field = value;

            onChanged?.Invoke();
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
