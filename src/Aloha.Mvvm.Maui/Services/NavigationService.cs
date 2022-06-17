using Aloha.Mvvm.Maui.Pages;
using Aloha.Mvvm.Enumerations;
using Aloha.Mvvm.Services;
using Aloha.Mvvm.ViewModels;
using System.Reflection;

namespace Aloha.Mvvm.Maui.Services
{
    public interface IViewFor
    {
        object ViewModel { get; set; }
    }

    public interface IViewFor<T> : IViewFor where T : BaseViewModel
    {
        new T ViewModel { get; set; }
    }

    public class NavigationService : INavigationService
    {
        INavigation MauiNavigation
        {
            get
            {
                var tabView = Application.Current.MainPage as TabbedPage;
                var flyoutView = Application.Current.MainPage as FlyoutPage;

                // First check to see if we're on a tabbed page, then master detail, finally go to overall fallback
                return tabView?.CurrentPage?.Navigation ??
                                     (flyoutView?.Detail as TabbedPage)?.CurrentPage?.Navigation ?? // special consideration for a tabbed page inside master/detail
                                     flyoutView?.Detail?.Navigation ??
                                     Application.Current.MainPage.Navigation;
            }
        }

        // View model to view lookup - making the assumption that view model to view will always be 1:1
        readonly Dictionary<Type, Type> _viewModelViewDictionary = new Dictionary<Type, Type>();

        #region Registration

        public void AutoRegister(Assembly asm)
        {
            // Loop through everything in the assembly that implements IViewFor<T>
            foreach (var type in asm.DefinedTypes.Where(dt => !dt.IsAbstract &&
                        dt.ImplementedInterfaces.Any(ii => ii == typeof(IViewFor))))
            {
                // Get the IViewFor<T> portion of the type that implements it
                var viewForType = type.ImplementedInterfaces.FirstOrDefault(
                    ii => ii.IsConstructedGenericType &&
                    ii.GetGenericTypeDefinition() == typeof(IViewFor<>));

                // Register it, using the generic (T) type as the key and the view as the value
                Register(viewForType.GenericTypeArguments[0], type.AsType());
            }
        }

        public void Register(Type viewModelType, Type viewType)
        {
            if (!_viewModelViewDictionary.ContainsKey(viewModelType))
            {
                _viewModelViewDictionary.Add(viewModelType, viewType);
            }
        }

        #endregion

        #region Replace

        // Because we're going to do a hard switch of the page, either return
        // the detail page, or if that's null, then the current main page       
        Page DetailPage
        {
            get
            {
                var flyoutView = Application.Current.MainPage as FlyoutPage;
                return flyoutView?.Detail;
            }
            set
            {
                if (Application.Current.MainPage is FlyoutPage flyoutView)
                {
                    flyoutView.Detail = value;
                    flyoutView.IsPresented = false;
                }
                else
                {
                    Application.Current.MainPage = value;
                }
            }
        }

        public async Task SetDetailAsync(BaseViewModel viewModel, bool allowSamePageSet = false)
        {
            if (DetailPage != null)
            {
                // Ensure that we're not pushing a new page if the DetailPage is already set to this type
                if (!allowSamePageSet)
                {
                    IViewFor page;

                    if (DetailPage is NavigationPage)
                    {
                        page = ((NavigationPage)DetailPage).RootPage as IViewFor;
                    }
                    else
                    {
                        page = DetailPage as IViewFor;
                    }

                    if (page?.ViewModel?.GetType() == viewModel.GetType())
                    {
                        var flyoutView = Application.Current.MainPage as FlyoutPage;

                        flyoutView.IsPresented = false;

                        return;
                    }
                }
            }

            Page newDetailPage = await Task.Run(() =>
            {
                var view = InstantiateView(viewModel);

                // Tab pages shouldn't go into navigation pages
                if (view is TabbedPage)
                {
                    newDetailPage = (Page)view;
                }
                else
                {
                    newDetailPage = new NavigationPage((Page)view);
                }

                return newDetailPage;
            });

            DetailPage = newDetailPage;
        }

        public void SetRoot<T>(bool withNavigationEnabled = true) where T : BaseViewModel
        {
            SetRoot(Activator.CreateInstance<T>(), withNavigationEnabled);
        }

        public void SetRoot(BaseViewModel viewModel, bool withNavigationEnabled = true)
        {
            if (InstantiateView(viewModel) is Page view)
            {
                if (withNavigationEnabled)
                {
                    Application.Current.MainPage = new NavigationPage(view);
                }
                else
                {
                    Application.Current.MainPage = view;
                }
            }
        }

        #endregion

        #region Pop

        public Task PopAsync(bool animated = true) => MauiNavigation.PopAsync(animated);

        public Task PopModalAsync(bool animated = true) => MauiNavigation.PopModalAsync(animated);

        public Task PopToRootAsync(bool animated = true) => MauiNavigation.PopToRootAsync(animated);

        #endregion

        #region Push

        public Task PushAsync<T>(bool animated = true) where T : BaseViewModel
        {
            return PushAsync(ServiceContainer.Resolve<T>(), animated);
        }

        public Task PushAsync(BaseViewModel viewModel, bool animated) => MauiNavigation.PushAsync((Page)InstantiateView(viewModel), animated);

        public Task PushModalAsync<T>(bool nestedNavigation = false, bool animated = true) where T : BaseViewModel
        {
            return PushModalAsync(ServiceContainer.Resolve<T>(), nestedNavigation, animated);
        }

        public Task PushModalAsync(BaseViewModel viewModel, bool nestedNavigation = false, bool animated = true)
        {
            viewModel.ViewDisplay = ViewDisplayType.Modal;

            var view = InstantiateView(viewModel);

            Page page;

            if (nestedNavigation)
            {
                page = new NavigationPage((Page)view);
            }
            else
            {
                page = (Page)view;
            }

            return MauiNavigation.PushModalAsync(page, animated);
        }

        #endregion

        IViewFor InstantiateView(BaseViewModel viewModel)
        {
            // Figure out what type the view model is
            var viewModelType = viewModel.GetType();

            // Look up what type of view it corresponds to
            var viewType = _viewModelViewDictionary[viewModelType];

            // Instantiate it
            var view = (IViewFor)Activator.CreateInstance(viewType);

            if (view != null)
            {
                view.ViewModel = viewModel;

                if (view.GetType().IsSubclassOf(typeof(BaseTabbedPage)) &&
                         view is BaseTabbedPage tabbedView &&
                         viewModel is BaseCollectionViewModel collectionViewModel)
                {
                    foreach (var childViewModel in collectionViewModel.ViewModels)
                    {
                        if (InstantiateView(childViewModel) is BaseContentPage childView)
                        {
                            if (collectionViewModel.EnableNavigation)
                            {
                                var navPage = new NavigationPage(childView);
                                navPage.Title = childView.Title;
                                tabbedView.Children.Add(navPage);
                            }
                            else
                            {
                                tabbedView.Children.Add(childView);
                            }
                        }
                    }
                }
                else if (view.GetType().IsSubclassOf(typeof(BaseFlyoutPage)) &&
                         view is BaseFlyoutPage flyoutView &&
                         viewModel is BaseFlyoutViewModel flyoutViewModel)
                {
                    if (InstantiateView(flyoutViewModel.Flyout) is BaseContentPage masterView)
                    {
                        flyoutView.Flyout = masterView;

                        if (InstantiateView(flyoutViewModel.Detail) is Page detailView)
                        {
                            if (detailView is TabbedPage)
                            {
                                flyoutView.Detail = detailView;
                            }
                            else
                            {
                                flyoutView.Detail = new NavigationPage(detailView);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Detail page cannot be null.");
                        }

                        flyoutView.IsPresented = false;
                    }
                    else
                    {
                        throw new InvalidOperationException("Master page must derive from BaseContentPage.");
                    }
                }
            }

            return view;
        }
    }
}

