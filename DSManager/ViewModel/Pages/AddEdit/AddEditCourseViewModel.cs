using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using NHibernate.Util;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditCourseViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private Course _course;
        private DateTime? _startDate;
        private DateTime _minimalEndDate;
        private DrivingLicenseCategory? _drivingLicenseCategory;
        private CourseType? _courseType;
        private Student _availableStudent;
        private Participant _chosenStudent;
        private ObservableCollection<Student> _availableStudents;
        private ObservableCollection<Participant> _chosenStudents;
        private ObservableCollection<Instructor> _instructors;

        private bool _isStudentsLoading;
        private bool _isEndDateEnabled;

        private RelayCommand _moveStudentToRight;
        private RelayCommand _moveStudentToLeft;

        public AddEditCourseViewModel() {
            Messenger.Default.Register<AddEditEntityMessage<Course>>(this, HandleMessage);
            FillStudents();
        }

        private void HandleMessage(AddEditEntityMessage<Course> message) {
            if (message.Entity != null) {
                if(message.Entity.GetType() != typeof(Course))
                    return;

                _course = (Course) message.Entity;
                _startDate = _course.StartDate;
                _drivingLicenseCategory = _course.Category;
                _courseType = _course.CourseType;
                _chosenStudents = new ObservableCollection<Participant>(_course.Participants);
                _chosenStudent = null;
                FillStudents();
                FillInstructors();
            } else {
                _course = new Course();
                _startDate = null;
                _drivingLicenseCategory = null;
                _courseType = null;
                _availableStudents = new ObservableCollection<Student>();
                _chosenStudents = new ObservableCollection<Participant>();
                _chosenStudent = null;
                _instructors = new ObservableCollection<Instructor>();
            }
            _isEndDateEnabled = false;
        }

        #region IDataErrorInfo Methods

        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "StartDate":
                    if (_startDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "DrivingLicenseCategory":
                    if (_drivingLicenseCategory == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "CourseType":
                    if (_courseType == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "ChosenStudents":
                    if (_chosenStudents.Count == 0)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "KEOSNr":
                    if (string.IsNullOrEmpty(_chosenStudent?.KEOSNr))
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "PKKNr":
                    if (string.IsNullOrEmpty(_chosenStudent?.PKKNr))
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "EndDate":
                    if (_chosenStudent?.EndDate < _minimalEndDate)
                        validationMessage =
                            "Data zakończenia szkolenia musi być późniejsza niż data zaliczenia ostatniego egzaminu!";
                    break;

                case "Instructor":
                    if (_chosenStudent?.Instructor == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "CoursePrice":
                    if (_chosenStudent?.CoursePrice <= 0)
                        validationMessage = "Cena kursu musi być większa od zera!";
                    break;
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if (_startDate == null)
                return false;
            _course.StartDate = (DateTime) _startDate;

            if (_drivingLicenseCategory == null)
                return false;
            _course.Category = (DrivingLicenseCategory) _drivingLicenseCategory;

            if (_courseType == null)
                return false;
            _course.CourseType = (CourseType) _courseType;

            if (_chosenStudents.Count == 0)
                return false;

            foreach(var participant in _chosenStudents) {
                if (string.IsNullOrEmpty(participant.KEOSNr))
                    return false;

                if (string.IsNullOrEmpty(participant.PKKNr))
                    return false;

                if (participant.CoursePrice <= 0)
                    return false;

                if (participant.Instructor == null)
                    return false;

                if (participant.EndDate != null && participant.EndDate < _minimalEndDate)
                    return false;
            }
            _course.Participants = _chosenStudents;
            #endregion

            return true;
        }

        public override bool Save() {
            if (!Validate())
                return false;

            using (var repository = new BaseRepository()) {
                repository.Save(_course);
            }

            return true;
        }
        #endregion

        #region ViewElements
        public DateTime? StartDate {
            get { return _startDate; }
            set {
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DrivingLicenseCategory> DrivingLicenseCategories => new ObservableCollection<DrivingLicenseCategory>(new EnumToList<DrivingLicenseCategory>().Enums);

        public DrivingLicenseCategory? DrivingLicenseCategory {
            get { return _drivingLicenseCategory; }
            set {
                _drivingLicenseCategory = value;
                ChosenStudents = new ObservableCollection<Participant>();
                FillStudents();
                FillInstructors();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CourseType> CourseTypes => new ObservableCollection<CourseType>(new EnumToList<CourseType>().Enums);

        public CourseType? CourseType {
            get { return _courseType; }
            set {
                _courseType = value;
                ChosenStudents = new ObservableCollection<Participant>();
                FillStudents();
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

        public Student AvailableStudent {
            get { return _availableStudent; }
            set {
                _availableStudent = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Participant> ChosenStudents {
            get { return _chosenStudents; }
            set {
                _chosenStudents = value;
                RaisePropertyChanged();
            }
        }

        public Participant ChosenStudent {
            get { return _chosenStudent; }
            set {
                _chosenStudent = value;
                FillParticipantInfo();
                RaisePropertyChanged();
            }
        }

        public bool IsStudentsLoading {
            get { return _isStudentsLoading; }
            set {
                _isStudentsLoading = value;
                RaisePropertyChanged();
            }
        }

        public string KEOSNr {
            get { return _chosenStudent?.KEOSNr; }
            set {
                _chosenStudent.KEOSNr = value;
                RaisePropertyChanged();
            }
        }

        public string PKKNr {
            get { return _chosenStudent?.PKKNr; }
            set {
                _chosenStudent.PKKNr = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate {
            get { return _chosenStudent?.EndDate; }
            set {
                _chosenStudent.EndDate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEndDateEnabled {
            get { return _isEndDateEnabled; }
            set {
                _isEndDateEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Instructor> Instructors {
            get { return _instructors; }
            set {
                _instructors = value;
                RaisePropertyChanged();
            }
        }

        public Instructor Instructor {
            get { return _chosenStudent?.Instructor; }
            set {
                _chosenStudent.Instructor = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTheory {
            get { return _chosenStudent != null && _chosenStudent.IsTheory; }
            set {
                _chosenStudent.IsTheory = value;
                RaisePropertyChanged();
            }
        }

        public decimal? CoursePrice {
            get { return _chosenStudent?.CoursePrice; }
            set {
                _chosenStudent.CoursePrice = value ?? 0;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand MoveStudentToRight {
            get {
                return _moveStudentToRight ?? (_moveStudentToRight = new RelayCommand(() => {
                    if(AvailableStudent == null)
                        return;
                    ChosenStudents.Add(new Participant {
                        Student = AvailableStudent,
                        Course = _course
                    });
                    AvailableStudents.Remove(AvailableStudent);
                    RefreshTables();
                }));
            }
        }

        public RelayCommand MoveStudentToLeft {
            get {
                return _moveStudentToLeft ?? (_moveStudentToLeft = new RelayCommand(() => {
                    if(ChosenStudent == null)
                        return;
                    AvailableStudents.Add(ChosenStudent.Student);
                    ChosenStudents.Remove(ChosenStudent);
                    RefreshTables();
                }));
            }
        }
        #endregion

        #region Helpers
        private async void FillStudents() {
            // chyba juz zrobione TODO dla przypadku edycji zaimplementować usuwanie z listy osób, które znajdują się już po prawej stronie tabeli
            if (_drivingLicenseCategory == null || _courseType == null)
                return;

            IsStudentsLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    var studentsToEliminate = new ObservableCollection<Student>();
                    // eliminate students with selected category
                    repository.ToList<DrivingLicensePermissions>()
                        .Where(x => x.Category == _drivingLicenseCategory)
                        .ForEach(x => {
                            if (x.DrivingLicense != null)
                                studentsToEliminate.Add(x.DrivingLicense.Student);
                        });
                    // eliminate students who are attending course for selected category
                    repository.ToList<Course>()
                        .Where(x => x.Category == _drivingLicenseCategory)
                        .ForEach(x =>
                            x.Participants
                                .ForEach(y => {
                                    if (y.EndDate == null || y.EndDate > _startDate)
                                        studentsToEliminate.Add(y.Student);
                                }));
                    AvailableStudents = new ObservableCollection<Student>(repository.ToList<Student>().Where(x => !studentsToEliminate.Contains(x)));
                }
            });

            IsStudentsLoading = false;
        }

        private async void FillInstructors() {
            if (_drivingLicenseCategory == null)
                return;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    var instructorsToInclude = new ObservableCollection<Instructor>();
                    // include instructors who does have permission for selected category
                    repository.ToList<InstructorPermissions>()
                        .Where(x => x.Category == _drivingLicenseCategory)
                        .ForEach(x => instructorsToInclude.Add(x.Instructor));
                    Instructors = instructorsToInclude;
                }
            });
        }

        private void FillParticipantInfo() {
            if (_chosenStudent == null)
                return;

            KEOSNr = _chosenStudent.KEOSNr;
            PKKNr = _chosenStudent.PKKNr;
            Instructor = _chosenStudent.Instructor;
            IsTheory = _chosenStudent.IsTheory;
            CoursePrice = _chosenStudent.CoursePrice;
            EndDate = _chosenStudent.EndDate;

            using (var repository = new BaseRepository()) {
                var passedTheory = repository.ToList<ExamsDates>().Where(x => x.Participant == _chosenStudent && x.CourseKind == CourseKind.Theory && x.IsPassed == true).OrderByDescending(x => x.EndDate).ToList();
                var passedPractice = repository.ToList<ExamsDates>().Where(x => x.Participant == _chosenStudent && x.CourseKind == CourseKind.Practice && x.IsPassed == true).OrderByDescending(x => x.EndDate).ToList();

                if (passedTheory.Count != 0 && passedPractice.Count != 0) {
                    IsEndDateEnabled = true;
                    _minimalEndDate = passedTheory.First().EndDate > passedPractice.First().EndDate
                        ? passedTheory.First().EndDate.Date
                        : passedPractice.First().EndDate.Date;
                } else {
                    IsEndDateEnabled = false;
                    _minimalEndDate = DateTime.Now;
                }
            }
        }

        private void RefreshTables() {
            ChosenStudents =
                new ObservableCollection<Participant>(ChosenStudents.OrderBy(x => x.Student.LastName)
                    .ThenBy(x => x.Student.FirstName)
                    .ThenBy(x => x.Student.SecondName)
                    .ThenBy(x => x.Student.BirthDate));
            AvailableStudents =
                new ObservableCollection<Student>(AvailableStudents.OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ThenBy(x => x.SecondName)
                    .ThenBy(x => x.BirthDate));
        }
        #endregion
    }
}
