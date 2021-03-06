using Aloha.Mvvm.Maui.Services;
using Aloha.Mvvm.ViewModels;

namespace Aloha.Mvvm.Maui.Pages
{
    public class BaseFlyoutPage : FlyoutPage
    { }

    public class BaseFlyoutPage<T> : BaseFlyoutPage, IViewFor<T> where T : BaseViewModel
    {
        T _viewModel;

        public T ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                BindingContext = _viewModel = value;
                Init();
            }
        }

        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => ViewModel = (T)value;
        }

        async void Init() => await ViewModel?.InitAsync();
    }
}

