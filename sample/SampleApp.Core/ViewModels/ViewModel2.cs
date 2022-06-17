using System.Windows.Input;
using Aloha.Mvvm.Input;
using Aloha.Mvvm.Services;
using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class ViewModel2 : BaseNavigationViewModel
    {
        ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null)
                {
                    _navigationCommand = new Command(async () => await Navigation.PushModalAsync(GetViewModel<ViewModel3>()));
                }

                return _navigationCommand;
            }
        }
    }
}

