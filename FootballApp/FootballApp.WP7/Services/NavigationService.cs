using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows.Navigation;
using FootballApp.Core.ViewModel.Services;

namespace FootballApp.WP7.Services
{
    public class NavigationService : INavigationService
    {
        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(Uri pageUri)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }
    }
}
