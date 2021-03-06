/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ToDoList.WP7._2"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ToDoList.Core.ViewModel;
using ToDoList.Core.Services;
using ToDoList.Core.Model;

namespace ToDoList.WP7._2.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            //SimpleIoc.Default.Register<MainViewModel>();

            // Registering AllToDoItemsiewModel with IToDoItemDataService
            SimpleIoc.Default.Register<ToDoDataContext>();
            if (!SimpleIoc.Default.IsRegistered<IToDoItemDataService>())
            {
                SimpleIoc.Default.Register<IToDoItemDataService, ToDoItemDataService>();
            }
            
            SimpleIoc.Default.Register<AllToDoItemsViewModel>();
            SimpleIoc.Default.Register<NewToDoItemViewModel>();

        }

        public AllToDoItemsViewModel AllToDoItems
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AllToDoItemsViewModel>();
            }
        }

        public NewToDoItemViewModel NewToDoItem
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewToDoItemViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}