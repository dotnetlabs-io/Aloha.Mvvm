using System.Windows.Input;
using Aloha.Mvvm.Input;
using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class CollectionViewModel : BaseCollectionViewModel
    {
        ICommand _switchCommand;
        public ICommand SwitchCommand
        {
            get
            {
                if (_switchCommand == null)
                {
                    _switchCommand = new Command(SwitchSelected);
                }

                return _switchCommand;
            }
        }

        public CollectionViewModel()
        {
            ViewModels.Add(CreateInstance<ViewModel1>());
            ViewModels.Add(CreateInstance<ViewModel2>());
        }

        void SwitchSelected()
        {
            var newIndex = SelectedIndex == 0 ? 1 : 0;

            // You can change the selected item by updating the

            // 1.) SelectedViewModel
            //SelectedViewModel = ViewModels[newIndex];

            // 2.) SelectedIndex
            SelectedIndex = newIndex;
        }
    }
}

