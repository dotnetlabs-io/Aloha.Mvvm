using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class RootViewModel : BaseFlyoutViewModel
    {
        public RootViewModel() : base() 
        { 
            var menuViewModel = GetViewModel<MenuViewModel>();
            menuViewModel.MenuItemSelected = MenuItemSelected;

            Flyout = menuViewModel;
            Detail = GetViewModel<CollectionViewModel>();
        }

        void MenuItemSelected(BaseViewModel viewModel) => SetDetail(viewModel);
    }
}

