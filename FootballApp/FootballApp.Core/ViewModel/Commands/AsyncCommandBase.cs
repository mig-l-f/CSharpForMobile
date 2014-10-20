using System;
using System.Threading.Tasks;
using System.Windows.Input;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace FootballApp.Core.ViewModel.Commands
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {

        public abstract Task ExecuteAsync(object parameter);

        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
    }
}
