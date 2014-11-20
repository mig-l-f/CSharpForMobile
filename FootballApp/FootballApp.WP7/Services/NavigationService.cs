using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows.Navigation;
using System.Windows;
using Microsoft.Phone.Controls;
using FootballApp.Core.ViewModel.Services;

namespace FootballApp.WP7.Services
{
    public class NavigationService : INavigationService
    {

        private PhoneApplicationFrame _mainFrame;

        #region INavigationService Methods

        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(Uri pageUri)
        {
            if (EnsureMainFrame())
            {
                _mainFrame.Navigate(pageUri);
            }
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }
        #endregion

        private bool EnsureMainFrame()
        {
            if (_mainFrame != null)
            {
                return true;
            }

            _mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (_mainFrame != null)
            {
                // could be null if application is running inside a design tool
                _mainFrame.Navigating += (s, e) =>
                    {
                        if (Navigating != null)
                        {
                            Navigating(s, e);
                        }
                    };
                return true;
            }
            return false;
        }

    }
}
