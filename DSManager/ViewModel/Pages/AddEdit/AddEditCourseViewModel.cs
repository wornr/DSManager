using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditCourseViewModel : BaseViewModel {
        private Student _availableStudent;
        private Student _chosenStudent;
        private ObservableCollection<Student> _availableStudents;
        private ObservableCollection<Student> _chosenStudents;
        private RelayCommand _moveStudentToRight;
        private RelayCommand _moveStudentToLeft;

        public AddEditCourseViewModel() {
            using (var repository = new BaseRepository()) {
                AvailableStudents = new ObservableCollection<Student>(repository.ToList<Student>().ToList());
                ChosenStudents = new ObservableCollection<Student>();
            }
        }

        // TODO wykonać obsługę dla możliwości wyboru kilku wierszy DataGrid na raz
        public Student AvailableStudent {
            get { return _availableStudent; }
            set {
                _availableStudent = value;
                RaisePropertyChanged();
            }
        }

        public Student ChosenStudent {
            get {
                return _chosenStudent;
            }
            set {
                _chosenStudent = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Student> AvailableStudents {
            get { return _availableStudents; }
            set {
                _availableStudents = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Student> ChosenStudents {
            get { return _chosenStudents; }
            set {
                _chosenStudents = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand MoveStudentToRight {
            get {
                return _moveStudentToRight ?? (_moveStudentToRight = new RelayCommand(() => {
                    if (AvailableStudent == null) return;
                    ChosenStudents.Add(AvailableStudent);
                    AvailableStudents.Remove(AvailableStudent);
                }));
            }
        }

        public RelayCommand MoveStudentToLeft {
            get {
                return _moveStudentToLeft ?? (_moveStudentToLeft = new RelayCommand(() => {
                    if (ChosenStudent == null) return;
                    AvailableStudents.Add(ChosenStudent);
                    ChosenStudents.Remove(ChosenStudent);
                }));
            }
        }

        private bool Validate(BaseEntity entity) {
            throw new System.NotImplementedException();
        }
    }
}
