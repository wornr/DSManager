/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DSManager"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using DSManager.ViewModel.Windows;
using DSManager.ViewModel.Pages;

namespace DSManager.ViewModel {
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator {
        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator() {
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

            // Windows
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SignInViewModel>();

            // Pages
            SimpleIoc.Default.Register<StudentsViewModel>();
            SimpleIoc.Default.Register<InstructorsViewModel>();
            SimpleIoc.Default.Register<CarsViewModel>();
            SimpleIoc.Default.Register<CoursesViewModel>();

            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();

        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SignInViewModel SignIn => ServiceLocator.Current.GetInstance<SignInViewModel>();
        public StudentsViewModel Students => ServiceLocator.Current.GetInstance<StudentsViewModel>();
        public InstructorsViewModel Instructors => ServiceLocator.Current.GetInstance<InstructorsViewModel>();
        public CarsViewModel Cars => ServiceLocator.Current.GetInstance<CarsViewModel>();
        public CoursesViewModel Courses => ServiceLocator.Current.GetInstance<CoursesViewModel>();

        public UsersViewModel Users => ServiceLocator.Current.GetInstance<UsersViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}