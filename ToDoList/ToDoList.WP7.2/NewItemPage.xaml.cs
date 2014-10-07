using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ToDoList.WP7._2
{
    public partial class NewItemPage : PhoneApplicationPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            //this.DataContext = App.NewItemViewModel;
        }
    }
}