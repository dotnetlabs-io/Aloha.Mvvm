using Aloha.Mvvm.Services;

namespace Aloha.Mvvm.ViewModels
{
    public abstract class BaseFlyoutViewModel : BaseNavigationViewModel
    {
        public BaseViewModel Flyout { get; set; }

        BaseViewModel _detail;
        public BaseViewModel Detail
        {
            get => _detail;
            set
            {
                if (_detail != null)
                {
                    SetDetail(value);
                }

                _detail = value;
            }
        }

        protected async void SetDetail(BaseViewModel viewModel) => await Navigation?.SetDetailAsync(viewModel);
    }
}

