using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Windows {
    public class MainViewModel : BaseViewModel {
        private RelayCommand _openStudentsPageCommand;
        private RelayCommand _openInstructorsPageCommand;
        private RelayCommand _openCarsPageCommand;
        private RelayCommand _openCoursesPageCommand;
        private RelayCommand _openPaymentsPageCommand;
        private RelayCommand _openUsersPageCommand;

        public MainViewModel() {
            
        }

        public RelayCommand OpenStudentsPage {
            get {
                return _openStudentsPageCommand ?? (_openStudentsPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Students);
                }));
            }
        }

        public RelayCommand OpenInstructorsPage {
            get {
                return _openInstructorsPageCommand ?? (_openInstructorsPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Instructors);
                }));
            }
        }

        public RelayCommand OpenCarsPage {
            get {
                return _openCarsPageCommand ?? (_openCarsPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Cars);
                }));
            }
        }

        public RelayCommand OpenCoursesPage {
            get {
                return _openCoursesPageCommand ?? (_openCoursesPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Courses);
                }));
            }
        }

        public RelayCommand OpenPaymentsPage {
            get {
                return _openPaymentsPageCommand ?? (_openPaymentsPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Payments);
                }));
            }
        }

        public RelayCommand OpenUsersPage {
            get {
                return _openUsersPageCommand ?? (_openUsersPageCommand = new RelayCommand(() => {
                    this.NavigateTo(ViewModelLocator.Instance.Users);
                }));
            }
        }

        public List<Course> Courses {
            get {
                using(BaseRepository repository = new BaseRepository()) {
                    try {
                        return repository.ToList<Course>().ToList();
                    } catch {
                        return null;
                    }
                }
            }
        }

        public List<ClassesDates> ClassesDates {
            get {
                using(BaseRepository repository = new BaseRepository()) {
                    try {
                        return repository.ToList<ClassesDates>().ToList();
                    } catch {
                        return null;
                    }
                }
            }
        }

        public List<Payment> Payments {
            get {
                using(BaseRepository repository = new BaseRepository()) {
                    try {
                        return repository.ToList<Payment>().ToList();
                    } catch {
                        return null;
                    }
                }
            }
        }
    }
}