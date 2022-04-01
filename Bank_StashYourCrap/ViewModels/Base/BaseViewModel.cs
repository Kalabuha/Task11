using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bank_StashYourCrap.ViewModels.Base
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnProperyChanged([CallerMemberName] string? propertyName = default!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = default!)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnProperyChanged(propertyName);
            return true;
        }
    }
}
