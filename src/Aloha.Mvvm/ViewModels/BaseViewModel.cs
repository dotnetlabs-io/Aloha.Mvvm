using System;
using System.Threading.Tasks;
using Aloha.Mvvm.Enumerations;

namespace Aloha.Mvvm.ViewModels
{
    public abstract class BaseViewModel : BaseNotify
    {
        public ViewDisplayType ViewDisplay { get; set; } = ViewDisplayType.Default;

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetPropertyChanged(ref _isBusy, value);
        }

        public virtual Task InitAsync() => Task.FromResult(true);

        protected static T CreateInstance<T>() where T : BaseViewModel => Activator.CreateInstance<T>();
    }
}

