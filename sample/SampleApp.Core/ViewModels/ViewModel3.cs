using System.Threading.Tasks;
using System.Windows.Input;
using Aloha.Mvvm.Input;
using Aloha.Mvvm.Services;
using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class ViewModel3 : BaseNavigationViewModel
    {
        ICommand _dismissCommand;
        public ICommand DismissCommand
        {
            get
            {
                if (_dismissCommand == null)
                {
                    _dismissCommand = new Command(async () => await DismissAsync());
                }

                return _dismissCommand;
            }
        }

        ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null)
                {
                    _navigationCommand = new Command(async () => await Navigation.PushAsync(CreateInstance<CollectionViewModel>()));
                }

                return _navigationCommand;
            }
        }

        Task DismissAsync()
        {
            if (ViewDisplay == Aloha.Mvvm.Enumerations.ViewDisplayType.Modal)
            {
                return Navigation.PopModalAsync();
            }

            return Navigation.PopAsync();
        }
    }
}

