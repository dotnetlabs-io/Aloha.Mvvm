using System;
using System.Reflection;
using System.Threading.Tasks;
using Aloha.Mvvm.ViewModels;

namespace Aloha.Mvvm.Services
{
	public interface INavigationService
	{
        void AutoRegister(Assembly asm);
        void Register(Type viewModelType, Type viewType);
        Task PopAsync(bool animated = true);
        Task PopModalAsync(bool animated = true);
        Task PushAsync<T>(bool animated = true) where T : BaseViewModel;
        Task PushAsync(BaseViewModel viewModel, bool animated = true);
        Task PushModalAsync<T>(bool nestedNavigation = false, bool animated = true) where T : BaseViewModel;
        Task PushModalAsync(BaseViewModel viewModel, bool nestedNavigation = false, bool animated = true);
        Task PopToRootAsync(bool animated = true);
        Task SetDetailAsync(BaseViewModel viewModel, bool allowSamePageSet = false);
        void SetRoot<T>(bool withNavigationEnabled = true) where T : BaseViewModel;
        void SetRoot(BaseViewModel viewModel, bool withNavigationEnabled = true);
    }
}

