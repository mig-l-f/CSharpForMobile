using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using FootballApp.Core.ViewModel.Commands;
using FootballApp.Core.Model;
using FootballApp.Core.Services;

namespace FootballApp.Core.ViewModel
{
    public class CompetitionsViewModel : INotifyPropertyChanged
    {
        private IFootballDataService _dataService;

        public CompetitionsViewModel(IFootballDataService dataService)
        {
            _dataService = dataService;
            GetAvailableCompetitionsCommand = new AsyncCommand<List<Competition>>(() => _dataService.GetAvailableCompetitionsAsync());
            Initialize();
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
        
    }
}
