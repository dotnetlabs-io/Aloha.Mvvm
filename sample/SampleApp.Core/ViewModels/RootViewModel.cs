using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class RootViewModel : BaseFlyoutViewModel
    {
        public RootViewModel() : base() 
        { 
            var menuViewModel = CreateInstance<MenuViewModel>();
            menuViewModel.MenuItemSelected = MenuItemSelected;

            Flyout = menuViewModel;
            Detail = CreateInstance<CollectionViewModel>();
        }

        void MenuItemSelected(BaseViewModel viewModel) => SetDetail(viewModel);
    }
}

