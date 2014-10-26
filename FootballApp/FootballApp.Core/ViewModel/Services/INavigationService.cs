using System;
using System.Windows.Navigation;

namespace FootballApp.Core.ViewModel.Services
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        void NavigateTo(Uri pageUri);
        void GoBack();
    }
}
