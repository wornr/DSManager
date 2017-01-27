using GalaSoft.MvvmLight.Command;

using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Windows {
    public sealed class MainViewModel : BaseViewModel {
        #region Permissions
        private bool _studentsMgmtPermission;
        private bool _instructorsMgmtPermission;
        private bool _carsMgmtPermission;
        private bool _coursesMgmtPermission;
        private bool _usersMgmtPermission;
        private bool _settingsMgmtPermission;

        private bool _studentsExplorePermission;
        private bool _instructorsExplorePermission;
        private bool _carsExplorePermission;
        private bool _coursesExplorePermission;
        private bool _statisticsExplorePermission;
        #endregion

        private RelayCommand _openHomePageCommand;
        private RelayCommand _openStudentsPageCommand;
        private RelayCommand _openInstructorsPageCommand;
        private RelayCommand _openCarsPageCommand;
        private RelayCommand _openCoursesPageCommand;
        private RelayCommand _openAgendaPageCommand;
        private RelayCommand _openStatisticsPageCommand;
        private RelayCommand _openUsersPageCommand;
        private RelayCommand _openSettingsPageCommand;
        private RelayCommand _openAboutPageCommand;
        private RelayCommand _lockCommand;
        private RelayCommand _signOutCommand;

        public MainViewModel() {
            NavigateTo(ViewModelLocator.Instance.Home);
            InitializePermissions();
        }

        public RelayCommand OpenHomePage => _openHomePageCommand ?? (_openHomePageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Home)));

        public RelayCommand OpenStudentsPage => _openStudentsPageCommand ?? (_openStudentsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Students)));
        public RelayCommand OpenInstructorsPage => _openInstructorsPageCommand ?? (_openInstructorsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Instructors)));
        public RelayCommand OpenCarsPage => _openCarsPageCommand ?? (_openCarsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Cars)));
        public RelayCommand OpenCoursesPage => _openCoursesPageCommand ?? (_openCoursesPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Courses)));
        public RelayCommand OpenAgendaPage => _openAgendaPageCommand ?? (_openAgendaPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Agenda)));

        public RelayCommand OpenStatisticsPage => _openStatisticsPageCommand ?? (_openStatisticsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Statistics)));
        public RelayCommand OpenUsersPage => _openUsersPageCommand ?? (_openUsersPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Users)));
        public RelayCommand OpenSettingsPage => _openSettingsPageCommand ?? (_openSettingsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Settings)));

        public RelayCommand OpenAboutPage => _openAboutPageCommand ?? (_openAboutPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.About)));

        public RelayCommand Lock => _lockCommand ?? (_lockCommand = new RelayCommand(() => {
            NavigateTo(ViewModelLocator.Instance.Home);

            Locked = true;

            var signInWindow = new SignInWindow();
            signInWindow.ShowDialog();
        }));
        public RelayCommand SignOut => _signOutCommand ?? (_signOutCommand = new RelayCommand(() => {
            NavigateTo(ViewModelLocator.Instance.Home);

            SignedUser = null;
            Locked = false;

            var signInWindow = new SignInWindow();
            signInWindow.Show();

            MainWindow.Close();

            ViewModelLocator.ReinitializeViewModels();
        }));

        #region Permissions
        private void InitializePermissions() {
            _studentsMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "StudentsManagement");
            _instructorsMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "InstructorsManagement");
            _carsMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "CarsManagement");
            _coursesMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "CoursesManagement");
            _usersMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "UsersManagement");
            _statisticsExplorePermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "StatisticsExplore");
            _settingsMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "SettingsManagement");

            _studentsExplorePermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "StudentsExplore");
            _instructorsExplorePermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "InstructorsExplore");
            _carsExplorePermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "CarsExplore");
            _coursesExplorePermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "CoursesExplore");
        }

        public bool StudentsMgmtPermission {
            get { return _studentsMgmtPermission || _studentsExplorePermission; }
            set {
                _studentsMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        public bool InstructorsMgmtPermission {
            get { return _instructorsMgmtPermission || _instructorsExplorePermission; }
            set {
                _instructorsMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        public bool CarsMgmtPermission {
            get { return _carsMgmtPermission || _carsExplorePermission; }
            set {
                _carsMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        public bool CoursesMgmtPermission {
            get { return _coursesMgmtPermission || _coursesExplorePermission; }
            set {
                _coursesMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        public bool UsersMgmtPermission {
            get { return _usersMgmtPermission; }
            set {
                _usersMgmtPermission = value;
                RaisePropertyChanged();
            }
        }

        public bool StatisticsExplorePermission {
            get { return _statisticsExplorePermission; }
            set {
                _statisticsExplorePermission = value;
                RaisePropertyChanged();
            }
        }

        public bool SettingsMgmtPermission {
            get { return _settingsMgmtPermission; }
            set {
                _settingsMgmtPermission = value;
                RaisePropertyChanged();
            }
        }/*

        public bool StudentsExplorePermission {
            get { return _studentsExplorePermission; }
            set {
                _studentsExplorePermission = value;
                RaisePropertyChanged();
            }
        }

        public bool InstructorsExplorePermission {
            get { return _instructorsExplorePermission; }
            set {
                _instructorsExplorePermission = value;
                RaisePropertyChanged();
            }
        }

        public bool CarsExplorePermission {
            get { return _carsExplorePermission; }
            set {
                _carsExplorePermission = value;
                RaisePropertyChanged();
            }
        }

        public bool CoursesExplorePermission {
            get { return _coursesExplorePermission; }
            set {
                _coursesExplorePermission = value;
                RaisePropertyChanged();
            }
        }*/
        #endregion
    }
}