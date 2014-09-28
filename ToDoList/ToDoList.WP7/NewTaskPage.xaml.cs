using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ToDoList.Core.Model;

namespace ToDoList.WP7
{
    public partial class NewTaskPage : PhoneApplicationPage
    {
        public NewTaskPage()
        {
            InitializeComponent();

            this.DataContext = App.TodosViewModel;
        }

        private void appBarOkButton_Click(object sender, EventArgs e)
        {
            if (newTaskNameTextBox.Text.Length > 0)
            {
                ToDoItem newToDoItem = new ToDoItem
                {
                    ItemName = newTaskNameTextBox.Text,
                    Category = (ToDoCategory)categoriesListPicker.SelectedItem
                };

                App.TodosViewModel.AddToDoItem(newToDoItem);

                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}