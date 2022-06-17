using Aloha.Mvvm.Maui.Pages;
using SampleApp.Core.ViewModels;

namespace SampleApp.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MenuPage : BaseContentPage<MenuViewModel>
{
    public MenuPage()
    {
        InitializeComponent();
    }
}

