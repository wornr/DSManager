using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using NHibernate.Util;

using WpfScheduler;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Pages {
    public class AgendaViewModel : BaseViewModel {
        #region Variables

        #region Permissions
        private bool _agendaDisplayPermission;
        private bool _agendaMgmtPermission;
        #endregion

        #region Selections
        private DateTime _instructorSelectedDate;
        private Instructor _instructor;
        private DateTime _studentSelectedDate;
        private Student _student;
        private DateTime _carSelectedDate;
        private Car _car;
        #endregion

        #region Lists
        private ObservableCollection<Instructor> _instructors;
        private ObservableCollection<Event> _instructorEvents;
        private ObservableCollection<Student> _students;
        private ObservableCollection<Event> _studentEvents;
        private ObservableCollection<Car> _cars;
        private ObservableCollection<Event> _carEvents;
        #endregion

        #region Commands
        private RelayCommand _addInstructorEvent;
        private RelayCommand _addStudentEvent;
        private RelayCommand _addCarEvent;
        private RelayCommand _refreshInstructorEvents;
        private RelayCommand _refreshStudentEvents;
        private RelayCommand _refreshCarEvents;
        private RelayCommand _filterInstructors;
        private RelayCommand _filterStudents;
        private RelayCommand _filterCars;
        #endregion

        #region View Elements
        private string _filter;
        private bool _isInstructorsLoading;
        private bool _isInstructorEventsLoading;
        private bool _isStudentsLoading;
        private bool _isStudentEventsLoading;
        private bool _isCarsLoading;
        private bool _isCarEventsLoading;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        
        public AgendaViewModel() {
            InitializePermissions();

            InstructorSelectedDate = DateTime.Now;
            InstructorEvents = new ObservableCollection<Event>();
            FillInstructors();

            StudentSelectedDate = DateTime.Now;
            StudentEvents = new ObservableCollection<Event>();
            FillStudents();

            CarSelectedDate = DateTime.Now;
            CarEvents = new ObservableCollection<Event>();
            FillCars();
        }

        #region Methods

        #region Selections
        public DateTime InstructorSelectedDate {
            get { return _instructorSelectedDate; }
            set {
                _instructorSelectedDate = value;
                FillInstructorEvents();
                RaisePropertyChanged();
            }
        }

        public Instructor Instructor {
            get { return _instructor; }
            set {
                _instructor = value;
                FillInstructorEvents();
                RaisePropertyChanged();
            }
        }

        public DateTime StudentSelectedDate {
            get { return _studentSelectedDate; }
            set {
                _studentSelectedDate = value;
                FillStudentEvents();
                RaisePropertyChanged();
            }
        }

        public Student Student {
            get { return _student; }
            set {
                _student = value;
                FillStudentEvents();
                RaisePropertyChanged();
            }
        }

        public DateTime CarSelectedDate {
            get { return _carSelectedDate; }
            set {
                _carSelectedDate = value;
                FillCarEvents();
                RaisePropertyChanged();
            }
        }

        public Car Car {
            get { return _car; }
            set {
                _car = value;
                FillCarEvents();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<Instructor> Instructors {
            get { return _instructors; }
            set {
                _instructors = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Event> InstructorEvents {
            get { return _instructorEvents; }
            set {
                _instructorEvents = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Student> Students {
            get { return _students; }
            set {
                _students = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Event> StudentEvents {
            get { return _studentEvents; }
            set {
                _studentEvents = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Car> Cars {
            get { return _cars; }
            set {
                _cars = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Event> CarEvents {
            get { return _carEvents; }
            set {
                _carEvents = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddInstructorEvent {
            get {
                return _addInstructorEvent?? (_addInstructorEvent = new RelayCommand(() => {
                    if(Instructor == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego instruktora!");
                        return;
                    }
                    var addWindow = new AddEditWindow { Title = "Dodaj wydarzenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditAgenda,
                    });
                    Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Instructor> {
                        Entity = null,
                        Owner = _instructor
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand AddStudentEvent {
            get {
                return _addStudentEvent ?? (_addStudentEvent = new RelayCommand(() => {
                    if(Student == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego kursanta!");
                        return;
                    }
                    var addWindow = new AddEditWindow { Title = "Dodaj wydarzenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditAgenda,
                    });
                    Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Student> {
                        Entity = null,
                        Owner = _student
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand AddCarEvent {
            get {
                return _addCarEvent ?? (_addCarEvent = new RelayCommand(() => {
                    if(Car == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego pojazdu!");
                        return;
                    }
                    var addWindow = new AddEditWindow { Title = "Dodaj wydarzenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditAgenda,
                    });
                    Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Car> {
                        Entity = null,
                        Owner = _car
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        /*public RelayCommand EditEvent {
            get {
                return _editEvent ?? (_editEvent = new RelayCommand(() => {
                    if (_event == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wydarzenia!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj wydarzenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditAgenda,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Course> {
                        Entity = _event
                    });
                    editWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand DeleteEvent {
            get {
                return _deleteEvent ?? (_deleteEvent = new RelayCommand(async () => {
                    if(_event == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wydarzenia!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dane wydarzenie?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_event);
                            }
                    }
                    FillEvents();
                }));
            }
        }*/

        public RelayCommand RefreshInstructorEvents => _refreshInstructorEvents ?? (_refreshInstructorEvents = new RelayCommand(() => {
            FillInstructors();
            FillInstructorEvents();
        }));
        public RelayCommand RefreshStudentEvents => _refreshStudentEvents ?? (_refreshStudentEvents = new RelayCommand(() => {
            FillStudents();
            FillStudentEvents();
        }));
        public RelayCommand RefreshCarEvents => _refreshCarEvents ?? (_refreshCarEvents = new RelayCommand(() => {
            FillCars();
            FillCarEvents();
        }));

        #endregion

        #region ViewElements
        public bool IsInstructorsLoading {
            get { return _isInstructorsLoading; }
            set {
                _isInstructorsLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInstructorEventsLoading {
            get { return _isInstructorEventsLoading; }
            set {
                _isInstructorEventsLoading = value;
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

        public bool IsStudentEventsLoading {
            get { return _isStudentEventsLoading; }
            set {
                _isStudentEventsLoading = value;
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

        public bool IsCarEventsLoading {
            get { return _isCarEventsLoading; }
            set {
                _isCarEventsLoading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        // TODO dodać filtrowanie
        private async void FillInstructors() {
            IsInstructorsLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    Instructors = new ObservableCollection<Instructor>(repository.ToList<Instructor>());
                }
            });

            IsInstructorsLoading = false;

            if (!Instructors.Contains(_instructor))
                _instructor = null;
        }

        private async void FillInstructorEvents() {
            if(_instructor == null)
                return;

            IsInstructorEventsLoading = true;

            await Task.Run(() => {
                var events = new ObservableCollection<Event>();

                using(var repository = new BaseRepository()) {
                    repository.ToList<ClassesDates>().Where(x => x.Instructor == _instructor).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ClassesDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Participant.Student?.FirstName + " " + x.Participant.Student?.SecondName + " " + x.Participant.Student?.LastName,
                            Color = x.CourseKind == CourseKind.Theory ? Brushes.Green : Brushes.RoyalBlue
                        });
                    });

                    repository.ToList<ExamsDates>().Where(x => x.Instructor == _instructor).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ExamsDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Participant.Student?.FirstName + " " + x.Participant.Student?.SecondName + " " + x.Participant.Student?.LastName,
                            Color = x.CourseKind == CourseKind.Theory ? Brushes.Coral : Brushes.DarkViolet
                        });
                    });

                    repository.ToList<LockedDates>().Where(x => x.Instructor == _instructor).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            LockedDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Description,
                            Color = Brushes.DarkRed
                        });
                    });
                }

                InstructorEvents = events;
            });

            IsInstructorEventsLoading = false;
        }

        // TODO dodać filtrowanie
        private async void FillStudents() {
            IsStudentsLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    Students = new ObservableCollection<Student>(repository.ToList<Student>().Where(x => x.Participants.Count > 0));
                }
            });

            IsStudentsLoading = false;
        }

        private async void FillStudentEvents() {
            if(_student == null)
                return;

            IsStudentEventsLoading = true;

            await Task.Run(() => {
                var events = new ObservableCollection<Event>();

                using(var repository = new BaseRepository()) {
                    var participants = repository.ToList<Participant>().Where(x => x.Student == _student);
                    repository.ToList<ClassesDates>().Where(x => participants.Contains(x.Participant)).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ClassesDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Participant.Instructor == null ? "Brak instruktora" : x.Participant.Instructor.FirstName + " " + x.Participant.Instructor.SecondName + " " + x.Participant.Instructor.LastName,
                            Color = x.CourseKind == CourseKind.Theory ? Brushes.Green : Brushes.RoyalBlue
                        });
                    });

                    repository.ToList<ExamsDates>().Where(x => participants.Contains(x.Participant)).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ExamsDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Participant.Instructor == null ? "Brak instruktora" : x.Participant.Instructor.FirstName + " " + x.Participant.Instructor.SecondName + " " + x.Participant.Instructor.LastName,
                            Color = x.CourseKind == CourseKind.Theory ? Brushes.Coral : Brushes.DarkViolet
                        });
                    });

                    repository.ToList<LockedDates>().Where(x => participants.Contains(x.Participant)).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            LockedDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Description,
                            Color = Brushes.DarkRed
                        });
                    });
                }

                StudentEvents = events;
            });

            IsStudentEventsLoading = false;
        }

        // TODO dodać filtrowanie
        private async void FillCars() {
            IsCarsLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    Cars = new ObservableCollection<Car>(repository.ToList<Car>());
                }
            });

            IsCarsLoading = false;
        }

        private async void FillCarEvents() {
            if(_car == null)
                return;

            IsCarEventsLoading = true;

            await Task.Run(() => {
                var events = new ObservableCollection<Event>();

                using(var repository = new BaseRepository()) {
                    repository.ToList<ClassesDates>().Where(x => x.Car == _car).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ClassesDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = "Instruktor: " + (x.Participant.Instructor == null ? "brak" : x.Participant.Instructor.FirstName + " " + x.Participant.Instructor.SecondName + " " + x.Participant.Instructor.LastName) + "\nKursant: " + (x.Participant.Student == null ? "brak" : x.Participant.Student.FirstName + " " + x.Participant.Student.SecondName + " " + x.Participant.Student.LastName),
                            Description = x.Car.Brand + " " + x.Car.Model,
                            Color = Brushes.RoyalBlue
                        });
                    });

                    repository.ToList<ExamsDates>().Where(x => x.Car == _car).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            ExamsDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = "Instruktor: " + (x.Participant.Instructor == null ? "brak" : x.Participant.Instructor.FirstName + " " + x.Participant.Instructor.SecondName + " " + x.Participant.Instructor.LastName) + "\nKursant: " + (x.Participant.Student == null ? "brak" : x.Participant.Student.FirstName + " " + x.Participant.Student.SecondName + " " + x.Participant.Student.LastName),
                            Description = x.Car.Brand + " " + x.Car.Model,
                            Color = Brushes.DarkViolet
                        });
                    });

                    repository.ToList<LockedDates>().Where(x => x.Car == _car).ForEach(x => {
                        events.Add(new Event {
                            AllDay = false,
                            Start = x.StartDate,
                            End = x.EndDate,
                            LockedDates = x,
                            Instructor = x.Instructor,
                            Participant = x.Participant,
                            Car = x.Car,
                            Subject = x.Description,
                            Description = x.Description,
                            Color = Brushes.DarkRed
                        });
                    });
                }

                CarEvents = events;
            });

            IsCarEventsLoading = false;
        }
        #endregion

        #region Permissions
        private void InitializePermissions() {
            _agendaMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "AgendaManagement");
        }

        public bool AgendaMgmtPermission {
            get { return _agendaMgmtPermission; }
            set {
                _agendaMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion
    }
}
