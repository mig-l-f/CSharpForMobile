// <copyright file="DialogService.cs" company="Miguel Fernandes">
//     Miguel Fernandes. All rights reserved.
// </copyright>
// <author>Miguel Fernandes</author>using System;
// <date>11/23/2014 10:17:54 AM</date>
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Windows;

using FootballApp.Core.ViewModel.Services;

namespace FootballApp.WP7.Services
{
    public class DialogService : IDialogService
    {

        //public Task ShowError(string message)
        //{
        //    return Task.Factory.StartNew(() => { MessageBox.Show(message); });
        //}
        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }
    }
}
