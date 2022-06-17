using SampleApp.Core.ViewModels;

namespace SampleApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Initialize Aloha.Mvvm and set the root view via ViewModel navigation
        Aloha.Mvvm.Maui.App.Init<RootViewModel>(GetType().Assembly);
    }
}