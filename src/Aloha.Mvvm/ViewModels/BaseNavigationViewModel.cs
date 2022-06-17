using Aloha.Mvvm.Services;

namespace Aloha.Mvvm.ViewModels
{
	public abstract class BaseNavigationViewModel : BaseViewModel
	{
        INavigationService _navigationService;

        protected INavigationService Navigation
        {
            get
            {
                if (_navigationService == null)
                    _navigationService = ServiceContainer.Resolve<INavigationService>();

                return _navigationService;
            }
        }
	}
}

