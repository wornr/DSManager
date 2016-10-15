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