using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Ioc;
using FootballApp.Core.ViewModel;

namespace FootballApp.WP7
{
    public partial class CompetitionView : PhoneApplicationPage
    {
        public CompetitionView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                var url = e.Uri.ToString();
                var itemUrl = url.Substring(url.IndexOf("?") + 1);
                if (!SimpleIoc.Default.IsRegistered<CompetitionViewModel>(itemUrl))
                {
                    return;
                }
                var vm = SimpleIoc.Default.GetInstance<CompetitionViewModel>(itemUrl);
                DataContext = vm;
            }
        }
    }
}