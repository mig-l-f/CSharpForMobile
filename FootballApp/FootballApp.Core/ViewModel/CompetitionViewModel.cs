using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using System.Linq;
//using System.Text;
using FootballApp.Core.Model;
using FootballApp.Core.Services;
using FootballApp.Core.ViewModel.Commands;
using GalaSoft.MvvmLight.Ioc;

namespace FootballApp.Core.ViewModel
{
    public class CompetitionViewModel : INotifyPropertyChanged
    {
        private Competition _competition;
        private IFootballDataService _dataService;

        // Contructor for DesignTime Only
        public CompetitionViewModel()
        {
            _competition = new Competition() { Id = 11, Name = "MyCompetition" };
            _dataService = SimpleIoc.Default.GetInstance<IFootballDataService>();
            GetCurrentStandingsCommand = new AsyncCommand<List<Team>>(() => _dataService.GetCurrentStandingsAsync(_competition.Id));
            GetTodayFixturesCommand = new AsyncCommand<List<Match>>(() => _dataService.GetFixturesForDateAsync(_competition.Id, DateTime.Today));
            GetTodayFixturesCommand.Execute(null);
            GetCurrentStandingsCommand.Execute(null);
        }

        [PreferredConstructor]
        public CompetitionViewModel(Competition competition, IFootballDataService dataService)
        {
            _competition = competition;
            _dataService = dataService;

            GetCurrentStandingsCommand = new AsyncCommand<List<Team>>(() => _dataService.GetCurrentStandingsAsync(_competition.Id));
            GetTodayFixturesCommand = new AsyncCommand<List<Match>>(() => _dataService.GetFixturesForDateAsync(_competition.Id, DateTime.Today));
        }

        #region Properties

        public Competition Competition
        {
            get
            {
                return _competition;
            }
        }

        public AsyncCommand<List<Team>> GetCurrentStandingsCommand
        {
            get;
            private set;
        }

        public AsyncCommand<List<Match>> GetTodayFixturesCommand
        {
            get;
            private set;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion
    }
}
