using System.Windows.Input;
using Aloha.Mvvm.Input;
using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class ViewModel1 : BaseNavigationViewModel
    {
        ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null)
                {
                    _navigationCommand = new Command(async () => await Navigation.PushAsync(CreateInstance<ViewModel3>()));
                }

                return _navigationCommand;
            }
        }
    }
}

