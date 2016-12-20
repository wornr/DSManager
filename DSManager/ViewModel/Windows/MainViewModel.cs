using GalaSoft.MvvmLight.Command;

using DSManager.View.Windows;

namespace DSManager.ViewModel.Windows {
    public sealed class MainViewModel : BaseViewModel {
        private RelayCommand _openHomePageCommand;
        private RelayCommand _openStudentsPageCommand;
        private RelayCommand _openInstructorsPageCommand;
        private RelayCommand _openCarsPageCommand;
        private RelayCommand _openCoursesPageCommand;
        private RelayCommand _openAgendaPageCommand;
        private RelayCommand _openUsersPageCommand;
        private RelayCommand _openSettingsPageCommand;
        private RelayCommand _lockCommand;
        private RelayCommand _signOutCommand;

        public MainViewModel() {
            NavigateTo(ViewModelLocator.Instance.Home);
        }

        public RelayCommand OpenHomePage => _openHomePageCommand ?? (_openHomePageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Home)));

        public RelayCommand OpenStudentsPage => _openStudentsPageCommand ?? (_openStudentsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Students)));
        public RelayCommand OpenInstructorsPage => _openInstructorsPageCommand ?? (_openInstructorsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Instructors)));
        public RelayCommand OpenCarsPage => _openCarsPageCommand ?? (_openCarsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Cars)));
        public RelayCommand OpenCoursesPage => _openCoursesPageCommand ?? (_openCoursesPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Courses)));
        public RelayCommand OpenAgendaPage => _openAgendaPageCommand ?? (_openAgendaPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Agenda)));

        public RelayCommand OpenUsersPage => _openUsersPageCommand ?? (_openUsersPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Users)));
        public RelayCommand OpenSettingsPage => _openSettingsPageCommand ?? (_openSettingsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Settings)));

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
        }));
    }
}