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
using DSManager.ViewModel.Pages.AddEdit;

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

            Initialize();
        }

        private static void Initialize() {
            #region Windows
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SignInViewModel>();
            SimpleIoc.Default.Register<AddEditViewModel>();
            #endregion

            #region Pages
            SimpleIoc.Default.Register<HomeViewModel>();

            SimpleIoc.Default.Register<StudentsViewModel>();
            SimpleIoc.Default.Register<InstructorsViewModel>();
            SimpleIoc.Default.Register<CarsViewModel>();
            SimpleIoc.Default.Register<CoursesViewModel>();
            SimpleIoc.Default.Register<AgendaViewModel>();

            SimpleIoc.Default.Register<StatisticsViewModel>();
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();

            #region AddEdit
            SimpleIoc.Default.Register<AddEditStudentViewModel>();
            SimpleIoc.Default.Register<AddEditInstructorViewModel>();
            SimpleIoc.Default.Register<AddEditCarViewModel>();
            SimpleIoc.Default.Register<AddEditCourseViewModel>();
            SimpleIoc.Default.Register<AddEditPaymentViewModel>();
            SimpleIoc.Default.Register<AddEditAgendaViewModel>();

            SimpleIoc.Default.Register<AddEditUserViewModel>();

            SimpleIoc.Default.Register<AddEditPricesViewModel>();
            SimpleIoc.Default.Register<AddEditDurationTimeViewModel>();
            SimpleIoc.Default.Register<AddEditMinimalAgeViewModel>();
            #endregion

            #endregion
        }

        #region Windows
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SignInViewModel SignIn => ServiceLocator.Current.GetInstance<SignInViewModel>();
        public AddEditViewModel AddEdit => ServiceLocator.Current.GetInstance<AddEditViewModel>();
        #endregion

        #region Pages
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();

        public StudentsViewModel Students => ServiceLocator.Current.GetInstance<StudentsViewModel>();
        public InstructorsViewModel Instructors => ServiceLocator.Current.GetInstance<InstructorsViewModel>();
        public CarsViewModel Cars => ServiceLocator.Current.GetInstance<CarsViewModel>();
        public CoursesViewModel Courses => ServiceLocator.Current.GetInstance<CoursesViewModel>();
        public AgendaViewModel Agenda => ServiceLocator.Current.GetInstance<AgendaViewModel>();

        public StatisticsViewModel Statistics => ServiceLocator.Current.GetInstance<StatisticsViewModel>();
        public UsersViewModel Users => ServiceLocator.Current.GetInstance<UsersViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        #region AddEdit
        public AddEditStudentViewModel AddEditStudent => ServiceLocator.Current.GetInstance<AddEditStudentViewModel>();
        public AddEditInstructorViewModel AddEditInstructor => ServiceLocator.Current.GetInstance<AddEditInstructorViewModel>();
        public AddEditCarViewModel AddEditCar => ServiceLocator.Current.GetInstance<AddEditCarViewModel>();
        public AddEditCourseViewModel AddEditCourse => ServiceLocator.Current.GetInstance<AddEditCourseViewModel>();
        public AddEditAgendaViewModel AddEditAgenda => ServiceLocator.Current.GetInstance<AddEditAgendaViewModel>();
        public AddEditPaymentViewModel AddEditPayment => ServiceLocator.Current.GetInstance<AddEditPaymentViewModel>();

        public AddEditUserViewModel AddEditUser => ServiceLocator.Current.GetInstance<AddEditUserViewModel>();

        public AddEditPricesViewModel AddEditPrices => ServiceLocator.Current.GetInstance<AddEditPricesViewModel>();
        public AddEditDurationTimeViewModel AddEditDurationTime => ServiceLocator.Current.GetInstance<AddEditDurationTimeViewModel>();
        public AddEditMinimalAgeViewModel AddEditMinimalAge => ServiceLocator.Current.GetInstance<AddEditMinimalAgeViewModel>();
        #endregion

        #endregion

        private static void Cleanup() {
            #region Windows
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<SignInViewModel>();
            SimpleIoc.Default.Unregister<AddEditViewModel>();
            #endregion

            #region Pages
            SimpleIoc.Default.Unregister<HomeViewModel>();

            SimpleIoc.Default.Unregister<StudentsViewModel>();
            SimpleIoc.Default.Unregister<InstructorsViewModel>();
            SimpleIoc.Default.Unregister<CarsViewModel>();
            SimpleIoc.Default.Unregister<CoursesViewModel>();
            SimpleIoc.Default.Unregister<AgendaViewModel>();

            SimpleIoc.Default.Unregister<StatisticsViewModel>();
            SimpleIoc.Default.Unregister<UsersViewModel>();
            SimpleIoc.Default.Unregister<SettingsViewModel>();

            #region AddEdit
            SimpleIoc.Default.Unregister<AddEditStudentViewModel>();
            SimpleIoc.Default.Unregister<AddEditInstructorViewModel>();
            SimpleIoc.Default.Unregister<AddEditCarViewModel>();
            SimpleIoc.Default.Unregister<AddEditCourseViewModel>();
            SimpleIoc.Default.Unregister<AddEditPaymentViewModel>();
            SimpleIoc.Default.Unregister<AddEditAgendaViewModel>();

            SimpleIoc.Default.Unregister<AddEditUserViewModel>();

            SimpleIoc.Default.Unregister<AddEditPricesViewModel>();
            SimpleIoc.Default.Unregister<AddEditDurationTimeViewModel>();
            SimpleIoc.Default.Unregister<AddEditMinimalAgeViewModel>();
            #endregion

            #endregion
        }

        public static void ReinitializeViewModels() {
            Cleanup();
            Initialize();
        }
    }
}