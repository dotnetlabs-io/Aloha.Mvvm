using System.Reflection;
using Aloha.Mvvm.Maui.Services;
using Aloha.Mvvm.Services;
using Aloha.Mvvm.ViewModels;

namespace Aloha.Mvvm.Maui
{
    public static class App
    {
        static INavigationService NavigationService { get; set; }

        public static void Init(Assembly asm)
        {
            NavigationService = new NavigationService();
            NavigationService.AutoRegister(asm);

            ServiceContainer.Register(NavigationService);
        }

        public static void Init<T>(Assembly asm) where T : BaseViewModel
        {
            Init(asm);

            NavigationService?.SetRoot<T>(false);
        }
    }
}

