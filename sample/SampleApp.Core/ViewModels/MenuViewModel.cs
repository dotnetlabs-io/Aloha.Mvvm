using System;
using System.Collections.Generic;
using System.Windows.Input;
using Aloha.Mvvm.Input;
using Aloha.Mvvm.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public Action<BaseViewModel> MenuItemSelected { get; set; }

        public List<string> MenuItems { get; set; }

        ICommand _menuItemSelectedCommand;
        public ICommand MenuItemSelectedCommand
        {
            get
            {
                if (_menuItemSelectedCommand == null)
                {
                    _menuItemSelectedCommand = new Command<string>(OnMenuItemSelectedAsync);
                }

                return _menuItemSelectedCommand;
            }
        }

        public MenuViewModel() => LoadMenuItems();

        void LoadMenuItems()
        {
            MenuItems = new List<string>(new[]
            {
                "Tabbed Page",
                "Page 1",
                "Page 2",
                "Page 3"
            });
        }

        void OnMenuItemSelectedAsync(string item)
        {
            BaseViewModel viewModel = null;

            switch (item)
            {
                case "Tabbed Page":
                    viewModel = CreateInstance<CollectionViewModel>();
                    break;
                case "Page 1":
                    viewModel = CreateInstance<ViewModel1>();
                    break;
                case "Page 2":
                    viewModel = CreateInstance<ViewModel2>();
                    break;
                case "Page 3":
                    viewModel = CreateInstance<ViewModel3>();
                    break;
            }

            if (viewModel != null)
            {
                MenuItemSelected(viewModel);
            }
        }
    }
}

