using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using FootballApp.Core.ViewModel.Commands;
using FootballApp.Core.ViewModel.Services;
using FootballApp.Core.Model;
using FootballApp.Core.Services;

namespace FootballApp.Core.ViewModel
{
    public class SelectCompetitionViewModel : INotifyPropertyChanged
    {
        private IFootballDataService _dataService;
        private INavigationService _navigationService;
        private Competition _selectedCompetition = null;
        private ICommand _navigateToSelectedCompetitionCommand;

        public SelectCompetitionViewModel(IFootballDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            GetAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());
            //Initialize();
        }

        public Task Initialize()
        {
            return GetAvailableCompetitionsCommand.ExecuteAsync(null);
        }

        #region Properties

        public AsyncCommand<List<Competition>> GetAvailableCompetitionsCommand
        {
            get;
            private set;
        }

        public Competition SelectedCompetition
        {
            get
            {
                return _selectedCompetition;
            }
            set
            {
                _selectedCompetition = value;
                OnPropertyChanged("SelectedCompetition");
            }
        }

        public ICommand NavigateToSelectedCompetitionCommand
        {
            get
            {
                if (_navigateToSelectedCompetitionCommand == null)
                {
                    _navigateToSelectedCompetitionCommand = new RelayCommand(NavigateToSelectedCompetition);
                }
                return _navigateToSelectedCompetitionCommand;
            }
        }

        #endregion

        #region INotifyPropertyChanged Methods

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Navigation

        private void NavigateToSelectedCompetition()
        {
            if (SelectedCompetition == null) return;

            if (!SimpleIoc.Default.IsRegistered<CompetitionViewModel>(SelectedCompetition.Id.ToString()))
            {
                SimpleIoc.Default.Register(() => new CompetitionViewModel(SelectedCompetition), SelectedCompetition.Id.ToString());                
            }

            _navigationService.NavigateTo(new Uri("/CompetitionView.xaml", UriKind.Relative));
        }

        #endregion

    }
}
