using Aloha.Mvvm.Maui.Pages;
using SampleApp.Core.ViewModels;

namespace SampleApp.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SampleTabbedPage : BaseTabbedPage<CollectionViewModel>
{
    public SampleTabbedPage()
    {
        InitializeComponent();
    }
}
