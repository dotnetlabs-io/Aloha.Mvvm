# Aloha.Mvvm

<img align="right" src="https://github.com/dotnetlabs-io/Aloha.Mvvm/blob/main/media/icon.png" />

The purpose of these projects are to simplify the creation simple/prototype/POC .NET [Multi-platform App UI (MAUI)](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/) apps. 

Again, these projects include a **small** set of functionality with the overall function being to create simple `.NET MAUI` apps that adhere to the [Model-View-ViewModel (MVVM)](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) design pattern. 


# Table of Contents
1. [Project Summary](#project-summary)
2. [Getting Started](#getting-started)
    1. [Nuget Packages](#grab-dat-nuget)
    2. [Initializing](#initializing)
3. [Coupling Pages to ViewModels](#coupling-pages-to-viewmodels)
    1. [Pages](#pages)
    2. [ViewModels](#viewmodels)
4. [Navigation](#navigation)
5. [Service Location](#service-location)
6. [Sample](#sample)
7. [Contribute](#contribute)

## Project Summary<a href="project-summary"></a>

* **`Aloha.Mvvm`** is a project that helps quickly spin up a platform agnostic Model-View-ViewModel (MVVM) architectural design approach for .NET MAUI applications. 

* **`Aloha.Mvvm.Maui`** is a project that uses the `Aloha.Mvvm` project, and provides a set of `.NET MAUI` specific class/object extensions that make creating prototype and simple `.NET MAUI` apps much less painful. 

    ### High-level features:

    * View-ViewModel coupling (with automatic setting of BindingContext)
    * ViewModel to ViewModel navigation 
    * Built-in [Service locator pattern](https://en.wikipedia.org/wiki/Service_locator_pattern) functionality (via [ServiceContainer.cs](src/Aloha.Mvvm/ServiceContainer.cs)).
    * Auto-registration of View-ViewModel relationships (via [INavigationService.cs](src/Aloha.Mvvm/Services/INavigationService.cs) and [NavigationService.cs](src/Aloha.Mvvm.Maui/Services/NavigationService.cs)) 

## Getting Started<a href="getting-started"></a>

### NuGet Packages<a href="grab-dat-nuget"></a>
Yes, there are [Nuget](https://nuget.org) packages for this! In fact, there are two:

1. `Aloha.Mvvm` [![GitHub release](https://img.shields.io/nuget/v/Aloha.Mvvm.svg?style=plastic)](https://www.nuget.org/packages/Aloha.Mvvm)

2. `Aloha.Mvvm.Maui` [![GitHub release](https://img.shields.io/nuget/v/Aloha.Mvvm.Maui.svg?style=plastic)](https://www.nuget.org/packages/Aloha.Mvvm.Maui)

**Pro Tip:** Remember that two important functions of the MVVM pattern are

1. Maximize reuse which helps...(see point 2)
2. Remain **completely** oblivious to the anything "View Level".

So, it's best to separate your `MAUI` app/view level code (i.e. ContentPage, ContentView, Button, etc.) from your ViewModels. At the very least, in separate projects. <./rant>

### Initializing<a href="initializing"></a>

Once the Nuget packages have been installed you will need to initialize `Aloha.Mvvm.Maui`. Add the following line to `App.xaml.cs` (ideally in the constructor):

```csharp
public App()
{
    InitializeComponent();

    // Add this line!
    Aloha.Mvvm.Maui.App.Init<RootViewModel>(GetType().Assembly);

    // Where "RootViewModel" is the ViewModel you want to be your MainPage
}
```

This accomplishes two things:

1. Registers and couples the Views to ViewModels
2. The _Generic_ () assigned to the `Init` method establishes the `MainPage` (and coupled ViewModel).

## Coupling Pages to ViewModels<a href="coupling-pages-to-viewmodels"></a>

### Pages<a href="pages"></a>

All Base page perform two main operations upon instantiation:

1. Set the BindingContext to the appropriate _ViewModel_ received via generic.
2. Executes the `InitAsync` method of the instantiated _ViewModel_. This is good for functionality you'd like executed upon page creation. `InitAsync` is optional - it exists as a `virtual` method in the base viewmodel.

#### BaseContentPage

Inherit from `BaseContentPage` from all `ContentPage` implementations.

##### XAML

```xml
<?xml version="1.0" encoding="utf-8" ?>
<pages:BaseContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pages="clr-namespace:Aloha.Mvvm.Maui.Pages;assembly=Aloha.Mvvm.Maui"
    xmlns:vm="clr-namespace:SampleApp.Core.ViewModels;assembly=SampleApp.Core"
    x:TypeArguments="vm:ViewModel1"
    x:Class="SampleApp.Pages.ContentPage1"
    Title="Page 1">
    <pages:BaseContentPage.Content>
           
        <!-- Content here -->
                
    </pages:BaseContentPage.Content>
</pages:BaseContentPage>
```

Key takeaways: <a href="content-page-xaml"></a>

1. Change `ContentPage` to `pages:BaseContentPage` ('pages' can be whatever you name it - see #2.1 below)
2. Include XML namespaces (xmlns) declarations for 
    1. `Aloha.Mvvm.Maui.Pages`
    2. The namespace where the ViewModel that you want to bind to this page.
3. Add the `TypeArgument` for the specific ViewModel you want to bind to this page.

##### CS

The `ContentPage` implementation just needs to inherit from `BaseContentPage` and provide the _ViewModel_ to be bound to the `Page`.

```csharp
public partial class ContentPage1 : BaseContentPage<ViewModel1>
```

#### [BaseFlyoutPage](src/Aloha.Mvvm.Maui/Pages/BaseFlyoutPage.cs) (inherits from [FlyoutPage](https://docs.microsoft.com/en-us/dotnet/maui/user-interface/pages/flyoutpage))

##### XAML

```xml
<?xml version="1.0" encoding="utf-8"?>
<pages:BaseFlyoutPage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pages="clr-namespace:Aloha.Mvvm.Maui.Pages;assembly=Aloha.Mvvm.Maui"
    xmlns:vm="clr-namespace:SampleApp.Core.ViewModels;assembly=SampleApp.Core"
    x:TypeArguments="vm:RootViewModel"
    x:Class="SampleApp.Pages.RootPage"
    Title="RootFlyoutPage">
</pages:BaseFlyoutPage>
```
Key takeaways: _See [ContentPage XAML](#content-page-xaml)_.

##### CS

The `FlyoutPage` implementation just needs to inherit from `BaseFlyoutPage` and provide the _BaseFlyoutViewModel_ to be bound to the `Page`.

```csharp
public partial class RootPage : BaseFlyoutPage<RootViewModel>
```

#### [BaseTabbedPage](src/Aloha.Mvvm.Maui/Pages/BaseTabbedPage.cs) (inherits from [TabbedPage](https://docs.microsoft.com/en-us/dotnet/maui/user-interface/pages/tabbedpage))

##### XAML

```xml
<?xml version="1.0" encoding="utf-8" ?>
<pages:BaseTabbedPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pages="clr-namespace:Aloha.Mvvm.Maui.Pages;assembly=Aloha.Mvvm.Maui"
    xmlns:vm="clr-namespace:SampleApp.Core.ViewModels;assembly=SampleApp.Core"
    x:TypeArguments="vm:CollectionViewModel"
    x:Class="SampleApp.Pages.SampleTabbedPage"
    Title="Tabbed Page">
    <pages:BaseTabbedPage.ToolbarItems>
        <ToolbarItem Text="Switch Tab" Command="{Binding SwitchCommand}" />
    </pages:BaseTabbedPage.ToolbarItems>
</pages:BaseTabbedPage>
```
Key takeaways: _See [ContentPage XAML](#content-page-xaml)_.

##### CS

The `TabbedPge` implementation just needs to inherit from `BaseTabbedPage` and provide the _BaseCollectionViewModel_ to be bound to the `Page`.

```csharp
public partial class SampleTabbedPage : BaseTabbedPage<CollectionViewModel>
```

### ViewModels<a href="viewmodels"></a>

#### BaseNotify

[BaseNotify](src/Aloha.Mvvm/BaseNotify.cs) is an abstract class that implements [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-6.0), and provides implementations for:

PropertyChanged
```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

SetPropertyChanged
```csharp
public void SetPropertyChanged(string propertyName)
{ ... }

protected virtual bool SetPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
{ ... }
```

#### BaseViewModel & BaseNavigationViewModel

[BaseViewModel](src/Aloha.Mvvm/ViewModels/BaseViewModel.cs) inherits from [BaseNotify](src/Aloha.Mvvm/BaseNotify.cs), and [BaseNavigationViewModel](src/Aloha.Mvvm/ViewModels/BaseNavigationViewModel.cs) inherits from `BaseViewModel`.

You can inherit from the abstract class `BaseNavigationViewModel` for all navigation enabled ViewModels, and ViewModels you want to bind to `BaseContentPage`. Note that you are _not_ limited to only binding to `BaseContentPage`

```csharp
public class ViewModel1 : BaseViewModel
```

<a href="base-vm-properties"></a>
Properties available: 

* IsBusy (`bool`)

Methods available:

* InitAsync: a virtual method that returns a task. This method is executed upon page creation.
* GetViewModel: returns an instantiated _ViewModel_ via generic.

#### BaseMasterDetailViewModel

Inherit from abstract class [BaseFlyoutViewModel](src/Aloha.Mvvm/ViewModels/BaseFlyoutViewModel.cs) for ViewModels you want to bind to a [BaseFlyoutPage](src/Aloha.Mvvm.Maui/Pages/BaseFlyoutPage.cs). 

```csharp 
public class RootViewModel : BaseFlyoutViewModel
{
    public RootViewModel() : base() 
    { 
        var menuViewModel = GetViewModel<MenuViewModel>();
        menuViewModel.MenuItemSelected = MenuItemSelected;

        Flyout = menuViewModel;
        Detail = GetViewModel<CollectionViewModel>();
    }

    void MenuItemSelected(BaseViewModel viewModel) => SetDetail(viewModel);
}
```

`BaseFlyoutViewModel` inherits from `BaseNavigationViewModel`, and because of this all of the [properties/methods](#base-vm-properties) available in `BaseNavigationViewModel` and `BaseViewModel` are also available in `BaseFlyoutViewModel`.

Addition properties available: 

* Master (`BaseViewModel`)
* Detail (`BaseViewModel`)

Additional methods available:

* SetDetail: allows you to set the Detail _ViewModel_.

#### BaseCollectionViewModel

Inherit from the abstract class [BaseCollectionViewModel](src/Aloha.Mvvm/ViewModels/BaseCollectionViewModel.cs) for ViewModels you want to bind to a [BaseTabbedPage](src/Aloha.Mvvm.Maui/Pages/BaseTabbedPage.cs). 

```csharp
public class CollectionViewModel : BaseCollectionViewModel
```

`BaseCollectionViewModel` inherits from `BaseNavigationViewModel`, and because of this all of the [properties/methods](#base-vm-properties) available in `BaseNavigationViewModel` and `BaseViewModel` are also available in `BaseCollectionViewModel`.

Addition properties available: 

* EnableNavigation (`bool`) - defaulted to true - determines if the page will exist within navigation stack (page)
* SelectedIndex (`int`)
* SelectedViewModel (`BaseViewModel`)
* ViewModels (`List<BaseViewModel>`)

## Navigation<a href="navigation"></a>

Navigation from one _ViewModel_ to another is very simple. Below are samples, using 'Navigation' for an 'INavigationService' resolution, we are able to perform several actions.

### Push
```csharp
await Navigation.PushAsync<ViewModel>();
await Navigation.PushAsync(GetViewModel<ViewModel>());
```

### Push Modal
```csharp
await Navigation.PushModalAsync<ViewModel>();
await Navigation.PushModalAsync(GetViewModel<ViewModel>());
```

### Pop
```csharp
await Navigation.PopAsync();
```

### Set Root
```csharp
await Navigation.SetRoot<ViewModel>();
await Navigation.SetRoot(GetViewModel<ViewModel>());
```

## Service Location<a href="service-location"></a>

[ServiceContainer.cs](src/Aloha.Mvvm/ServiceContainer.cs) can be used as a Service locator to register and retrieve various services.

### Registration

```csharp
Service.Container.Resolve<IAlertService>();
```

### Resolving Manually

```csharp
var alertService = ServiceContainer.Register<IAlertService>(new AlertService());
```

## Sample<a href="#sample"></a>

Please feel free to clone this repository, and run the sample located [here](sample). The sample app contains a demonstration of all the major features included in `Aloha.Mvvm` and `Aloha.Mvvm.Maui`.

## Contribute<a href="#contribute"></a>
    
Please feel free to contribute to this project by submitting PR's, issues, questions, etc. You can also contact us directly:

* Email us at [info@dotnetlabs.io](mailto:info@dotnetlabs.io)
* Hit us up on [Twitter (@dotnetlabsio)](https://twitter.com/dotnetlabsio)



