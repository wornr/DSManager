using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Utilities;

namespace DSManager.ViewModel.Windows {
    public sealed class MainViewModel : BaseViewModel {
        private RelayCommand _openStudentsPageCommand;
        private RelayCommand _openInstructorsPageCommand;
        private RelayCommand _openCarsPageCommand;
        private RelayCommand _openCoursesPageCommand;
        private RelayCommand _openUsersPageCommand;
        private RelayCommand _openSettingsPageCommand;

        public MainViewModel() {
            // TODO dodaæ stronê g³ówn¹ aplikacji
            NavigateTo(ViewModelLocator.Instance.Students);
        }

        public RelayCommand OpenStudentsPage => _openStudentsPageCommand ?? (_openStudentsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Students)));
        public RelayCommand OpenInstructorsPage => _openInstructorsPageCommand ?? (_openInstructorsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Instructors)));
        public RelayCommand OpenCarsPage => _openCarsPageCommand ?? (_openCarsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Cars)));
        public RelayCommand OpenCoursesPage => _openCoursesPageCommand ?? (_openCoursesPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Courses)));
        public RelayCommand OpenUsersPage => _openUsersPageCommand ?? (_openUsersPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Users)));
        public RelayCommand OpenSettingsPage => _openSettingsPageCommand ?? (_openSettingsPageCommand = new RelayCommand(() => NavigateTo(ViewModelLocator.Instance.Settings)));

        public User User => UserSignedIn.User;
    }
}