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
    public class SelectCompetitionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IFootballDataService _dataService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private Competition _selectedCompetition = null;
        private ICommand _navigateToSelectedCompetitionCommand;
        private AsyncCommand<List<Competition>> _getAvailableCompetitionsCommand;

        // Constructor for design time
        //public SelectCompetitionViewModel()
        //{
        //    _dataService = SimpleIoc.Default.GetInstance<IFootballDataService>();
        //    _navigationService = null;
        //    GetAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());
        //    GetAvailableCompetitionsCommand.Execute(null);
        //}

        
        public SelectCompetitionViewModel(IFootballDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogService = null;
            GetAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());

        }

        [PreferredConstructor]
        public SelectCompetitionViewModel(IFootballDataService dataService, INavigationService navigationService, IDialogService dialogService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            //_getAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());
            //GetAvailableCompetitionsCommand.Execution.PropertyChanged += (s, e) => 
            //{ 
            //    if (e.PropertyName.Equals("IsFaulted"))
            //    {
            //        DisplayErrorMessage();
            //    }
            //};

            //if (IsInDesignMode)
            //{
            //    GetAvailableCompetitionsCommand.Execute(null);
            //}
        }

        private void DisplayErrorMessage()
        {
            _dialogService.ShowError(GetAvailableCompetitionsCommand.Execution.ErrorMessage);
        }

        #region Properties

        public AsyncCommand<List<Competition>> GetAvailableCompetitionsCommand
        {
            get
            {
                if (_getAvailableCompetitionsCommand == null)
                {
                    _getAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());
                    _getAvailableCompetitionsCommand.Execution.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName.Equals("IsFaulted"))
                        {
                            DisplayErrorMessage();
                        }
                    };
                }
                return _getAvailableCompetitionsCommand;
            }
            private set
            {
                _getAvailableCompetitionsCommand = value;
            }
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
                SimpleIoc.Default.Register(() => new CompetitionViewModel(SelectedCompetition, _dataService), SelectedCompetition.Id.ToString());                
            }

            _navigationService.NavigateTo(
                new Uri(String.Format("/CompetitionView.xaml?{0}", SelectedCompetition.Id), UriKind.Relative));
        }

        #endregion

    }
}
