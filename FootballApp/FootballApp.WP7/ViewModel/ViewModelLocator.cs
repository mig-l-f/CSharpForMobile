using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using FootballApp.Core.ViewModel;
using FootballApp.Core.ViewModel.Services;
using FootballApp.Core.Services;
using FootballApp.Core.Services.DesignTime;
using FootballApp.WP7.Services;

namespace FootballApp.WP7.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IFootballDataService, DesignTimeFootballDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
                SimpleIoc.Default.Register<IDeserializeFootballDataService, DeserializeFootballDataService>();
                SimpleIoc.Default.Register<IFootballDataService, FootballDataService>();
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
            }

            SimpleIoc.Default.Register<SelectCompetitionViewModel>();
        }

        public SelectCompetitionViewModel AvailableCompetitionsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SelectCompetitionViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO: clean the view models
        }
    }
}
