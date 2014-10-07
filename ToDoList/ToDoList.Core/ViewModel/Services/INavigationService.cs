using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoList.Core.ViewModel.Services
{
    public interface INavigationService
    {
        void NavigateTo(Uri pageUri);
        void GoBack();
    }
}
