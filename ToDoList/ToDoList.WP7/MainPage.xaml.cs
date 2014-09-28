using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using ToDoList.Core.Model;

namespace ToDoList.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.TodosViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                ToDoItem toDoForDelete = button.DataContext as ToDoItem;
                App.TodosViewModel.DeleteToDoItem(toDoForDelete);
            }
            this.Focus();
        }

        private void newTaskAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewTaskPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.TodosViewModel.SaveChangesToDb();
        }
        // Load data for the ViewModel Items
        //private void MainPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (!App.ViewModel.IsDataLoaded)
        //    {
        //        App.ViewModel.LoadData();
        //    }
        //}
    }
}