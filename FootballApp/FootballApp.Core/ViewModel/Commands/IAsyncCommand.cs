using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace FootballApp.Core.ViewModel.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
