using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Messaging;

using NHibernate.Util;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditAgendaViewModel : AddEditBaseViewModel, IDataErrorInfo {
        private int _defaultTab;
        private int _selectedTab;
        private bool _editMode;
        private ClassesDates _classDate;
        private ExamsDates _examDate;
        private LockedDates _lockedDate;
        private Instructor _instructor;
        private Student _student;
        private Participant _participant;
        private Car _car;
        private DateTime? _startDate;
        private string _startTime;
        private DateTime? _endDate;
        private string _endTime;
        private CourseKind? _courseKind;
        private decimal? _distance;
        private string _description;
        private bool _isCourseKindChooserEnabled;
        private bool _isPractice;
        private bool _isCarChooserEnabled;
        private bool _isInstructorChooserEnabled;

        private Instructor _chosenInstructor;
        private Student _chosenStudent;
        private Participant _chosenParticipant;
        private Car _chosenCar;
        private ObservableCollection<Instructor> _availableInstructors;
        private ObservableCollection<Student> _availableStudents;
        private ObservableCollection<Participant> _availableParticipants;
        private ObservableCollection<Car> _availableCars;

        private bool _isInstructorsLoading;
        private bool _isStudentsLoading;
        private bool _isParticipantsLoading;
        private bool _isCarsLoading;

        public AddEditAgendaViewModel() {
            Messenger.Default.Register<AddEditAgendaMessage<ClassesDates, Instructor>>(this, HandleInstructorClassesDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<ExamsDates, Instructor>>(this, HandleInstructorExamsDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<LockedDates, Instructor>>(this, HandleInstructorLockedDatesMessage);

            Messenger.Default.Register<AddEditAgendaMessage<ClassesDates, Student>>(this, HandleStudentClassesDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<ExamsDates, Student>>(this, HandleStudentExamsDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<LockedDates, Student>>(this, HandleStudentLockedDatesMessage);

            Messenger.Default.Register<AddEditAgendaMessage<ClassesDates, Participant>>(this, HandleParticipantClassesDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<ExamsDates, Participant>>(this, HandleParticipantExamsDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<LockedDates, Participant>>(this, HandleParticipantLockedDatesMessage);

            Messenger.Default.Register<AddEditAgendaMessage<ClassesDates, Car>>(this, HandleCarClassesDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<ExamsDates, Car>>(this, HandleCarExamsDatesMessage);
            Messenger.Default.Register<AddEditAgendaMessage<LockedDates, Car>>(this, HandleCarLockedDatesMessage);
        }

        #region Messengers

        #region Instructors
        private void HandleInstructorClassesDatesMessage(AddEditAgendaMessage<ClassesDates, Instructor> message) {
            ClearAllValues();
            FillStudents();
            if (message.Entity != null) {
                _editMode = true;
                _classDate = (ClassesDates) message.Entity;
                _instructor = (Instructor) message.Owner;
                CourseKind = _classDate.CourseKind;
                _startDate = _classDate.StartDate;
                _startTime = _classDate.StartDate.ToShortTimeString();
                _endDate = _classDate.EndDate;
                _endTime = _classDate.EndDate.ToShortTimeString();
                ChosenStudent = _classDate.Participant.Student;
                ChosenParticipant = _classDate.Participant;
                ChosenCar = _classDate.Car;

            } else {
                _instructor = (Instructor) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
        }

        private void HandleInstructorExamsDatesMessage(AddEditAgendaMessage<ExamsDates, Instructor> message) {
            ClearAllValues();
            FillStudents();
            if (message.Entity != null) {
                _editMode = true;
                _examDate = (ExamsDates) message.Entity;
                _instructor = (Instructor) message.Owner;
                CourseKind = _examDate.CourseKind;
                _startDate = _examDate.StartDate;
                _startTime = _examDate.StartDate.ToShortTimeString();
                _endDate = _examDate.EndDate;
                _endTime = _examDate.EndDate.ToShortTimeString();
                ChosenStudent = _examDate.Participant.Student;
                ChosenParticipant = _examDate.Participant;
                ChosenCar = _examDate.Car;
            } else {
                _instructor = (Instructor) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
            _defaultTab = _selectedTab = 1;
        }

        private void HandleInstructorLockedDatesMessage(AddEditAgendaMessage<LockedDates, Instructor> message) {
            ClearAllValues();
            FillStudents();
            if (message.Entity != null) {
                _editMode = true;
                _lockedDate = (LockedDates) message.Entity;
                _instructor = (Instructor) message.Owner;
                _startDate = _lockedDate.StartDate;
                _startTime = _lockedDate.StartDate.ToShortTimeString();
                _endDate = _lockedDate.EndDate;
                _endTime = _lockedDate.EndDate.ToShortTimeString();
                _description = _lockedDate.Description;
            } else {
                _instructor = (Instructor) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
            _defaultTab = _selectedTab = 2;
        }
        #endregion

        #region Students/Participants

        #region Add
        private void HandleStudentClassesDatesMessage(AddEditAgendaMessage<ClassesDates, Student> message) {
            ClearAllValues();
            _student = (Student) message.Owner;
            _startDate = message.StartDate;
            _startTime = message.StartDate?.ToShortTimeString();
            _endDate = _startDate?.AddHours(1);
            _endTime = _endDate?.ToShortTimeString();
            FillParticipants();
        }

        private void HandleStudentExamsDatesMessage(AddEditAgendaMessage<ExamsDates, Student> message) {
            ClearAllValues();
            _student = (Student) message.Owner;
            _startDate = message.StartDate;
            _startTime = message.StartDate?.ToShortTimeString();
            _endDate = _startDate?.AddHours(1);
            _endTime = _endDate?.ToShortTimeString();
            FillParticipants();
            _defaultTab = _selectedTab = 1;
        }

        private void HandleStudentLockedDatesMessage(AddEditAgendaMessage<LockedDates, Student> message) {
            ClearAllValues();
            _student = (Student) message.Owner;
            _startDate = message.StartDate;
            _startTime = message.StartDate?.ToShortTimeString();
            _endDate = _startDate?.AddHours(1);
            _endTime = _endDate?.ToShortTimeString();
            _defaultTab = _selectedTab = 2;
        }
        #endregion

        #region Edit
        private void HandleParticipantClassesDatesMessage(AddEditAgendaMessage<ClassesDates, Participant> message) {
            ClearAllValues();
            _editMode = true;
            _classDate = (ClassesDates) message.Entity;
            _participant = (Participant) message.Owner;
            _student = _participant.Student;
            CourseKind = _classDate.CourseKind;
            _startDate = _classDate.StartDate;
            _startTime = _classDate.StartDate.ToShortTimeString();
            _endDate = _classDate.EndDate;
            _endTime = _classDate.EndDate.ToShortTimeString();
            FillParticipants();
            ChosenParticipant = _classDate.Participant;
            ChosenInstructor = _classDate.Instructor;
            ChosenCar = _classDate.Car;
        }

        private void HandleParticipantExamsDatesMessage(AddEditAgendaMessage<ExamsDates, Participant> message) {
            ClearAllValues();
            _editMode = true;
            _examDate = (ExamsDates) message.Entity;
            _participant = (Participant) message.Owner;
            _student = _participant.Student;
            CourseKind = _examDate.CourseKind;
            _startDate = _examDate.StartDate;
            _startTime = _examDate.StartDate.ToShortTimeString();
            _endDate = _examDate.EndDate;
            _endTime = _examDate.EndDate.ToShortTimeString();
            FillParticipants();
            ChosenParticipant = _examDate.Participant;
            ChosenInstructor = _examDate.Instructor;
            ChosenCar = _examDate.Car;
            _defaultTab = _selectedTab = 1;
        }

        private void HandleParticipantLockedDatesMessage(AddEditAgendaMessage<LockedDates, Participant> message) {
            ClearAllValues();
            _editMode = true;
            _lockedDate = (LockedDates) message.Entity;
            _participant = (Participant) message.Owner;
            _student = _participant.Student;
            _startDate = _lockedDate.StartDate;
            _startTime = _lockedDate.StartDate.ToShortTimeString();
            _endDate = _lockedDate.EndDate;
            _endTime = _lockedDate.EndDate.ToShortTimeString();
            _description = _lockedDate.Description;
            _defaultTab = _selectedTab = 2;
        }
        #endregion

        #endregion

        #region Car
        private void HandleCarClassesDatesMessage(AddEditAgendaMessage<ClassesDates, Car> message) {
            ClearAllValues();
            if (message.Entity != null) {
                _editMode = true;
                _classDate = (ClassesDates) message.Entity;
                _car = (Car) message.Owner;
                _startDate = _classDate.StartDate;
                _startTime = _classDate.StartDate.ToShortTimeString();
                _endDate = _classDate.EndDate;
                _endTime = _classDate.EndDate.ToShortTimeString();
                ChosenStudent = _classDate.Participant.Student;
                ChosenParticipant = _classDate.Participant;
                ChosenInstructor = _classDate.Instructor;
            } else {
                _car = (Car) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
            CourseKind = Model.Enums.CourseKind.Practice;
            FillStudents();
        }

        private void HandleCarExamsDatesMessage(AddEditAgendaMessage<ExamsDates, Car> message) {
            ClearAllValues();
            if (message.Entity != null) {
                _editMode = true;
                _examDate = (ExamsDates) message.Entity;
                _car = (Car) message.Owner;
                _startDate = _examDate.StartDate;
                _startTime = _examDate.StartDate.ToShortTimeString();
                _endDate = _examDate.EndDate;
                _endTime = _examDate.EndDate.ToShortTimeString();
                ChosenStudent = _examDate.Participant.Student;
                ChosenParticipant = _examDate.Participant;
                ChosenInstructor = _examDate.Instructor;
            } else {
                _car = (Car) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
            CourseKind = Model.Enums.CourseKind.Practice;
            FillStudents();
            _defaultTab = _selectedTab = 1;
        }

        private void HandleCarLockedDatesMessage(AddEditAgendaMessage<LockedDates, Car> message) {
            ClearAllValues();
            if (message.Entity != null) {
                _editMode = true;
                _lockedDate = (LockedDates) message.Entity;
                _car = (Car) message.Owner;
                _startDate = _lockedDate.StartDate;
                _startTime = _lockedDate.StartDate.ToShortTimeString();
                _endDate = _lockedDate.EndDate;
                _endTime = _lockedDate.EndDate.ToShortTimeString();
                _description = _lockedDate.Description;
            } else {
                _car = (Car) message.Owner;
                _startDate = message.StartDate;
                _startTime = message.StartDate?.ToShortTimeString();
                _endDate = _startDate?.AddHours(1);
                _endTime = _endDate?.ToShortTimeString();
            }
            _defaultTab = _selectedTab = 2;
        }
        #endregion

        #endregion

        #region IDataErrorInfo Methods
        public string this[string columnName] => Validate(columnName);

        public string Error => "Błąd";

        private string Validate(string propertyName) {
            var validationMessage = string.Empty;

            switch(propertyName) {
                case "StartDate":
                    if(_startDate == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "StartTime":
                    if(_startTime == null)
                        validationMessage = "Pole nie może być puste!";
                    else if (!Regex.IsMatch(_startTime, @"^[0-9]{2}:[0-9]{2}$"))
                        validationMessage = "Wprowadź godzinę w formacie GG:MM!";
                    else if (int.Parse(_startTime.Substring(0, 2)) < 0 || int.Parse(_startTime.Substring(0, 2)) > 23 ||
                             int.Parse(_startTime.Substring(3, 2)) < 0 || int.Parse(_startTime.Substring(3, 2)) > 59)
                        validationMessage = "Podana godzina jest niepoprawna!";
                    break;

                case "EndDate":
                    if(_endDate == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(_endDate <= _startDate)
                        validationMessage = "Data zak. musi być późniejsza od daty rozp.!";
                    else if (_endDate - _startDate < new TimeSpan(0, 0, 15, 0))
                        validationMessage = "Wydarzenie musi trwać co najmniej 15 minut!";
                    break;

                case "EndTime":
                    if(_endTime == null)
                        validationMessage = "Pole nie może być puste!";
                    else if(!Regex.IsMatch(_endTime, @"^[0-9]{2}:[0-9]{2}$"))
                        validationMessage = "Wprowadź godzinę w formacie GG:MM!";
                    else if(int.Parse(_startTime.Substring(0, 2)) < 0 || int.Parse(_startTime.Substring(0, 2)) > 23 ||
                             int.Parse(_startTime.Substring(3, 2)) < 0 || int.Parse(_startTime.Substring(3, 2)) > 59)
                        validationMessage = "Podana godzina jest niepoprawna!";
                    else if (_endDate <= _startDate)
                        validationMessage = "Data zak. musi być późniejsza od daty rozp.!";
                    else if(_endDate - _startDate < new TimeSpan(0, 0, 15, 0))
                        validationMessage = "Wydarzenie musi trwać co najmniej 15 minut!";
                    break;

                case "CourseKind":
                    if(_courseKind == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "Distance":
                    if(_distance != null && (_distance < 0m || _distance > 999.99m))
                        validationMessage = "Podano dystans spoza zakresu 0 - 999,99";
                    break;

                case "Car":
                    if (_car == null)
                        validationMessage = "Pole nie może być puste";
                    break;

                case "ChosenInstructor":
                    if(_chosenInstructor == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                case "ChosenParticipant":
                    if(_chosenParticipant == null)
                        validationMessage = "Pole nie może być puste!";
                    break;

                /*case "Price":
                    if(_chosenStudent?.CoursePrice <= 0)
                        validationMessage = "Cena zajęć musi być większa od zera!";
                    break;*/
            }

            return validationMessage;
        }
        #endregion

        #region Save Methods
        public override bool Validate() {
            #region Required
            if(_startDate == null)
                return false;

            if(_startTime == null || !Regex.IsMatch(_startTime, @"^[0-9]{2}:[0-9]{2}$") ||
                int.Parse(_startTime.Substring(0, 2)) < 0 || int.Parse(_startTime.Substring(0, 2)) > 23 ||
                int.Parse(_startTime.Substring(3, 2)) < 0 || int.Parse(_startTime.Substring(3, 2)) > 59)
                return false;

            if(_endDate == null)
                return false;

            if(_endTime == null || !Regex.IsMatch(_endTime, @"^[0-9]{2}:[0-9]{2}$") ||
                int.Parse(_endTime.Substring(0, 2)) < 0 || int.Parse(_endTime.Substring(0, 2)) > 23 ||
                int.Parse(_endTime.Substring(3, 2)) < 0 || int.Parse(_endTime.Substring(3, 2)) > 59)
                return false;

            if (_endDate <= _startDate)
                return false;

            if (_endDate - _startDate < new TimeSpan(0, 0, 15, 0))
                return false;

            if(_selectedTab == 0 || _selectedTab == 1) {
                if(_instructor == null && _chosenInstructor == null)
                    return false;
                _chosenInstructor = _chosenInstructor ?? _instructor;

                if(_participant == null && _chosenParticipant == null)
                    return false;
                _chosenParticipant = _chosenParticipant ?? _participant;

                if(_courseKind == null)
                    return false;

                if (_isPractice) {
                    if (_car == null && _chosenCar == null)
                        return false;
                    _chosenCar = _chosenCar ?? _car;
                } else
                    _chosenCar = null;
            } else if (string.IsNullOrEmpty(_description))
                return false;
            #endregion

            if (_selectedTab == 0) {
                _classDate.StartDate = (DateTime) _startDate;
                _classDate.EndDate = (DateTime) _endDate;
                _classDate.Instructor = _chosenInstructor;
                _classDate.Participant = _chosenParticipant;
                _classDate.CourseKind = (CourseKind) _courseKind;
                _classDate.Car = _chosenCar;
                _classDate.Distance = _isPractice ? _distance : null;
                _classDate.IsAdditional = false; // TODO zmienić
                _classDate.Price = null; // TODO zmienić
            } else if(_selectedTab == 1) {
                _examDate.StartDate = (DateTime)_startDate;
                _examDate.EndDate = (DateTime)_endDate;
                _examDate.Instructor = _chosenInstructor;
                _examDate.Participant = _chosenParticipant;
                _examDate.CourseKind = (CourseKind)_courseKind;
                _examDate.Car = _chosenCar;
                _examDate.IsPassed = false; // TODO zmienić
            } else if(_selectedTab == 2) {
                _lockedDate.StartDate = (DateTime)_startDate;
                _lockedDate.EndDate = (DateTime)_endDate;
                _lockedDate.Description = _description;

                if(_instructor != null)
                    _lockedDate.Instructor = _instructor;
                else if(_student != null || _participant != null)
                    _lockedDate.Participant = _participant;
                else if(_car != null)
                    _lockedDate.Car = _car;
            }

            using (var repository = new BaseRepository()) {
                if (
                    repository
                        .ToList<ClassesDates>()
                        .Count(
                            x =>
                                _classDate.Id != x.Id &&
                                ((_chosenInstructor != null && x.Instructor == _chosenInstructor) ||
                                    (_chosenParticipant != null && x.Participant == _chosenParticipant) ||
                                    (_chosenCar != null && x.Car == _chosenCar)) &&
                                ((_startDate < x.StartDate && _endDate > x.StartDate) ||
                                    (_endDate > x.EndDate && _startDate < x.EndDate) ||
                                    (_startDate > x.StartDate && _endDate < x.EndDate) ||
                                    _startDate == x.StartDate ||
                                    _endDate == x.EndDate)
                                )
                    +
                    repository
                        .ToList<ExamsDates>()
                        .Count(
                            x =>
                                _classDate.Id != x.Id &&
                                ((_chosenInstructor != null && x.Instructor == _chosenInstructor) ||
                                    (_chosenParticipant != null && x.Participant == _chosenParticipant) ||
                                    (_chosenCar != null && x.Car == _chosenCar)) &&
                                ((_startDate < x.StartDate && _endDate > x.StartDate) ||
                                    (_endDate > x.EndDate && _startDate < x.EndDate) ||
                                    (_startDate > x.StartDate && _endDate < x.EndDate) ||
                                    _startDate == x.StartDate ||
                                    _endDate == x.EndDate)
                                )
                    +
                    repository
                        .ToList<LockedDates>()
                        .Count(
                            x =>
                                _classDate.Id != x.Id &&
                                ((_chosenInstructor != null && x.Instructor == _chosenInstructor) ||
                                    (_chosenParticipant != null && x.Participant == _chosenParticipant) ||
                                    (_chosenCar != null && x.Car == _chosenCar)) &&
                                ((_startDate < x.StartDate && _endDate > x.StartDate) ||
                                    (_endDate > x.EndDate && _startDate < x.EndDate) ||
                                    (_startDate > x.StartDate && _endDate < x.EndDate) ||
                                    _startDate == x.StartDate ||
                                    _endDate == x.EndDate)
                                )
                    > 0)
                    return false;
            }

            return true;
        }

        public override bool Save() {
            if(!Validate())
                return false;

            using(var repository = new BaseRepository()) {
                if (_editMode && _defaultTab != _selectedTab) {
                    if(_defaultTab == 0)
                        repository.Delete(_classDate);
                    else if(_defaultTab == 1)
                        repository.Delete(_examDate);
                    else if(_defaultTab == 2)
                        repository.Delete(_lockedDate);
                }

                if (_selectedTab == 0)
                    repository.Save(_classDate);
                else if (_selectedTab == 1)
                    repository.Save(_examDate);
                else if (_selectedTab == 2)
                    repository.Save(_lockedDate);
            }

            return true;
        }
        #endregion

        #region ViewElements
        public int SelectedTab {
            get { return _selectedTab; }
            set {
                _selectedTab = value;
                RaisePropertyChanged();
            }
        }

        public Instructor Instructor {
            get { return _instructor; }
            set {
                _instructor = value;
                RaisePropertyChanged();
            }
        }

        public Student Student {
            get { return _student; }
            set {
                _student = value;
                RaisePropertyChanged();
            }
        }

        public Car Car {
            get { return _car; }
            set {
                _car = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? StartDate {
            get { return _startDate; }
            set {
                _startDate = value;
                StartTime = value?.ToShortTimeString();
                RaisePropertyChanged();
            }
        }

        public string StartTime {
            get {
                if (_startDate != null && _startTime != null && Regex.IsMatch(_startTime, @"^[0-9]{2}:[0-9]{2}$")) {
                    var hour = int.Parse(_startTime.Substring(0, 2));
                    var minute = int.Parse(_startTime.Substring(3, 2));

                    if(hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59)
                        return _startDate.Value.ToShortTimeString();
                }

                return _startTime;
            }
            set {
                _startTime = value;
                ParseTime(ref _startDate, _startTime);
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate {
            get { return _endDate; }
            set {
                _endDate = value;
                EndTime = value?.ToShortTimeString();
                RaisePropertyChanged();
            }
        }

        public string EndTime {
            get {
                if (_endDate != null && _endTime != null && Regex.IsMatch(_endTime, @"^[0-9]{2}:[0-9]{2}$")) {
                    var hour = int.Parse(_endTime.Substring(0, 2));
                    var minute = int.Parse(_endTime.Substring(3, 2));

                    if(hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59)
                        return _endDate.Value.ToShortTimeString();
                }

                return _endTime;
            }
            set {
                _endTime = value;
                ParseTime(ref _endDate, _endTime);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CourseKind> CourseKinds => new ObservableCollection<CourseKind>(new EnumToList<CourseKind>().Enums);

        public CourseKind? CourseKind {
            get { return _courseKind; }
            set {
                _courseKind = value;
                IsCarChooserEnabled = IsPractice = _courseKind != null && _courseKind.Value.Equals(Model.Enums.CourseKind.Practice);
                RaisePropertyChanged();
            }
        }

        public bool IsCourseKindChooserEnabled {
            get { return _isCourseKindChooserEnabled; }
            set {
                _isCourseKindChooserEnabled = value;
                RaisePropertyChanged();
            }
        }

        public decimal? Distance {
            get { return _distance; }
            set {
                _distance = value;
                RaisePropertyChanged();
            }
        }

        public string Description {
            get { return _description; }
            set {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPractice {
            get { return _isPractice; }
            set {
                _isPractice = value;
                RaisePropertyChanged(); }
        }

        public ObservableCollection<Student> AvailableStudents {
            get { return _availableStudents; }
            set {
                _availableStudents = value;
                RaisePropertyChanged();
            }
        }

        public Student ChosenStudent {
            get { return _chosenStudent; }
            set {
                _chosenStudent = value;
                FillParticipants();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Participant> AvailableParticipants {
            get { return _availableParticipants; }
            set {
                _availableParticipants = value;
                RaisePropertyChanged();
            }
        }

        public Participant ChosenParticipant {
            get { return _chosenParticipant; }
            set {
                _chosenParticipant = value;
                IsInstructorChooserEnabled = _chosenParticipant != null;
                if (_chosenParticipant != null && _chosenParticipant.IsTheory) {
                    IsCourseKindChooserEnabled = true;
                } else {
                    IsCourseKindChooserEnabled = false;
                    CourseKind = Model.Enums.CourseKind.Practice;
                }
                FillInstructors();
                FillCars();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Car> AvailableCars {
            get { return _availableCars; }
            set {
                _availableCars = value;
                RaisePropertyChanged();
            }
        }

        public Car ChosenCar {
            get { return _chosenCar; }
            set {
                _chosenCar = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Instructor> AvailableInstructors {
            get { return _availableInstructors; }
            set {
                _availableInstructors = value;
                RaisePropertyChanged();
            }
        }

        public Instructor ChosenInstructor {
            get { return _chosenInstructor; }
            set {
                _chosenInstructor = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInstructorsLoading {
            get { return _isInstructorsLoading; }
            set {
                _isInstructorsLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInstructorChooserEnabled {
            get { return _isInstructorChooserEnabled; }
            set {
                _isInstructorChooserEnabled = value;
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

        public bool IsParticipantsLoading {
            get { return _isParticipantsLoading; }
            set {
                _isParticipantsLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCarsLoading {
            get { return _isCarsLoading; }
            set {
                _isCarsLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCarChooserEnabled{
            get { return _isCarChooserEnabled; }
            set {
                _isCarChooserEnabled = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands

        #endregion

        #region Helpers
        private async void FillStudents() {
            IsStudentsLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    var studentsToEliminate = new ObservableCollection<Student>();
                    // eliminate students who are not attending any course
                    repository.ToList<Student>().Where(x => x.Participants.Count == 0).ForEach(x => studentsToEliminate.Add(x));
                    AvailableStudents = new ObservableCollection<Student>(repository.ToList<Student>().Where(x => !studentsToEliminate.Contains(x)));
                }
            });

            IsStudentsLoading = false;
        }

        private async void FillParticipants() {
            var student = _chosenStudent ?? _student;
            if (student == null)
                return;

            IsParticipantsLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    AvailableParticipants =
                        new ObservableCollection<Participant>(
                            repository.ToList<Participant>().Where(x => x.Student == student));
                }
            });

            IsParticipantsLoading = false;
        }

        private async void FillCars() {
            var participant = _chosenParticipant ?? _participant;
            if (participant == null)
                return;

            var isCarChooserEnabledTemp = _isCarChooserEnabled;

            IsCarsLoading = true;
            IsCarChooserEnabled = false;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    var carsToInclude = new ObservableCollection<Car>();
                    repository.ToList<CarPermissions>().Where(x => x.Category == participant.Course.Category && x.Car != null).ForEach(x => carsToInclude.Add(x.Car));
                    AvailableCars = carsToInclude;
                }
            });

            IsCarChooserEnabled = isCarChooserEnabledTemp;
            IsCarsLoading = false;
        }

        private async void FillInstructors() {
            var participant = _chosenParticipant ?? _participant;
            if (participant == null)
                return;

            var isInstructorChooserEnabledTemp = _isInstructorChooserEnabled;

            IsInstructorsLoading = true;
            IsInstructorChooserEnabled = false;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    var instructorsToInclude = new ObservableCollection<Instructor>();
                    // include instructors who does have permission for selected category
                    repository.ToList<InstructorPermissions>()
                        .Where(x => x.Category == participant.Course.Category && x.Instructor != null)
                        .ForEach(x => instructorsToInclude.Add(x.Instructor));
                    AvailableInstructors = instructorsToInclude;
                }
            });

            IsInstructorChooserEnabled = isInstructorChooserEnabledTemp;
            IsInstructorsLoading = false;
        }

        /*private void RefreshTables() {
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
        }*/

        private void ClearAllValues() {
            _editMode = false;
            _defaultTab = _selectedTab = 0;

            _classDate = new ClassesDates();
            _examDate = new ExamsDates();
            _lockedDate = new LockedDates();

            _instructor = null;
            _student = null;
            _participant = null;
            _car = null;

            _startDate = null;
            _endDate = null;
            _courseKind = null;

            _chosenInstructor = null;
            _chosenStudent = null;
            _chosenParticipant = null;
            _chosenCar = null;

            _availableInstructors = null;
            _availableStudents = null;
            _availableParticipants = null;
            _availableCars = null;

            _isCourseKindChooserEnabled = true;
        }

        private void ParseTime(ref DateTime? date, string time) {
            if (date != null && Regex.IsMatch(time, @"^[0-9]{2}:[0-9]{2}$")) {
                var hour = int.Parse(time.Substring(0, 2));
                var minute = int.Parse(time.Substring(3, 2));

                if (hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59)
                    date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, hour, minute, 0);
            }
        }
        #endregion
    }
}