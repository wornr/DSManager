using System;
using System.Linq;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Pages {
    public class StudentsViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Student _student;
        private Participant _participant;

        #endregion

        #region Lists
        private ObservableCollection<Student> _students;
        private ObservableCollection<Participant> _participants;
        private ObservableCollection<ClassesDates> _classesDates;
        private ObservableCollection<Payment> _payments;
        #endregion

        #region Commands
        private RelayCommand _addStudent;
        private RelayCommand _editStudent;
        private RelayCommand _deleteStudent;
        private RelayCommand _refreshStudents;
        private RelayCommand _filterStudents;
        private RelayCommand _addPayment;
        private RelayCommand _editPayment;
        private RelayCommand _deletePayment;
        private RelayCommand _printPayment;
        #endregion

        #region View Elements
        private string _filter;
        private bool _duringCourse;
        private bool _overduePayment;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        
        public StudentsViewModel() {
            FillStudents(_filter);
        }

        #region Methods

        #region Selections
        public Student Student {
            get { return _student; }
            set {
                _student = value;
                FillParticipant(value);
                ResolveCheckboxes();
                RaisePropertyChanged();
            }
        }

        public Participant Participant {
            get { return _participant; }
            set {
                _participant = value;
                FillClassesDates(value);
                FillPayment(value);
            }
        }

        public Payment Payment { get; set; }
        #endregion

        #region Lists
        public ObservableCollection<Student> Students {
            get {
                return _students;
            }
            set {
                _students = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Participant> Participants {
            get {
                return _participants;
            }
            set {
                _participants = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ClassesDates> ClassesDates {
            get {
                return _classesDates;
            }
            set {
                _classesDates = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Payment> Payments {
            get {
                return _payments;
            }
            set {
                _payments = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddStudent {
            get {
                return _addStudent ?? (_addStudent = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj kursanta" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditStudent,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = null
                    });
                    addWindow.Show();
                }));
            }
        }

        public RelayCommand EditStudent {
            get {
                return _editStudent ?? (_editStudent = new RelayCommand(() => {
                    if(Student == null)
                        // TODO wyrzucić komunikat "Nie wybrano żadnego kursanta"
                        return;
                    var editWindow = new AddEditWindow { Title = "Edytuj kursanta" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditStudent,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = _student
                    });
                    editWindow.Show();
                }));
            }
        }

        public RelayCommand DeleteStudent {
            get {
                return _deleteStudent ?? (_deleteStudent = new RelayCommand(() => {
                    if(_student == null) {
                        // TODO wyrzucić komunikat "Nie wybrano żadnego kursanta"
                    } else {
                        // TODO wyrzucić dialog z zapytaniem "Czy jesteś pewien, że chcesz usunąć danego kursanta?"
                        using(var repository = new BaseRepository()) {
                            repository.Delete(_student);
                        }
                    }
                    FillStudents(_filter);
                }));
            }
        }

        public RelayCommand RefreshStudents {
            get {
                return _refreshStudents ?? (_refreshStudents = new RelayCommand(() => {
                    FillStudents(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterStudents {
            get {
                return _filterStudents ?? (_filterStudents = new RelayCommand(() => {
                    FillStudents(_filter);
                    _prevFilter = _filter;
                }));
            }
        }

        public RelayCommand AddPayment {
            get {
                return _addPayment ?? (_addPayment = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj wpłatę" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditPayment,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = null
                    });
                    addWindow.Show();
                }));
            }
        }

        public RelayCommand EditPayment {
            get {
                return _editPayment ?? (_editPayment = new RelayCommand(() => {
                    if(Payment == null)
                        // TODO wyrzucić komunikat "Nie wybrano żadnej wpłaty"
                        return;
                    var editWindow = new AddEditWindow { Title = "Edytuj wpłatę" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditPayment,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = Payment
                    });
                    editWindow.Show();
                }));
            }
        }

        public RelayCommand DeletePayment {
            get {
                return _deletePayment ?? (_deletePayment = new RelayCommand(() => {
                    if(Payment == null) {
                        // TODO wyrzucić komunikat "Nie wybrano żadnej wpłaty"
                    } else {
                        // TODO wyrzucić dialog z zapytaniem "Czy jesteś pewien, że chcesz usunąć daną wpłatę?"
                        using(var repository = new BaseRepository()) {
                            repository.Delete(Payment);
                        }
                    }
                    FillPayment(_participant);
                }));
            }
        }

        public RelayCommand PrintPayment {
            get {
                return _printPayment ?? (_printPayment = new RelayCommand(() => {
                    // TODO dodać obsługę generowania wydruków potwierdzenia wpłat
                    Console.WriteLine(Payment.Date); // TODO DELETE ME
                }));
            }
        }
        #endregion

        #region View Elements
        public string Filter {
            get { return _filter; }
            set {
                if(_filter == value)
                    return;
                _filter = value;
                RaisePropertyChanged();
            }
        }

        public bool DuringCourse {
            get { return _duringCourse; }
            set {
                _duringCourse = value;
                RaisePropertyChanged();
            }
        }

        public bool OverduePayment {
            get { return _overduePayment; }
            set {
                _overduePayment = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private void FillStudents(string filter) {
            if(string.IsNullOrEmpty(filter)) {
                using(var repository = new BaseRepository()) {
                    Students =
                        new ObservableCollection<Student>(
                            repository.ToList<Student>()
                            .OrderBy(student => student.LastName)
                            .ThenBy(student => student.FirstName)
                            .ThenBy(student => student.SecondName)
                            .ThenBy(student => student.BirthDate)
                            .ToList());
                }
            } else {
                if(filter.Contains(" ")) {
                    var filters = filter.Split(' ');

                    if(!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                        using(var repository = new BaseRepository()) {
                            Students =
                                new ObservableCollection<Student>(
                                    repository.ToList<Student>()
                                        .Where(x => x.FirstName.Contains(filters[0]) && x.LastName.Contains(filters[1]) || x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                        .OrderBy(student => student.LastName)
                                        .ThenBy(student => student.FirstName)
                                        .ThenBy(student => student.SecondName)
                                        .ThenBy(student => student.BirthDate)
                                        .ToList());
                        }
                    } else {
                        using(var repository = new BaseRepository()) {
                            Students =
                                new ObservableCollection<Student>(
                                    repository.ToList<Student>()
                                        .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                        .OrderBy(student => student.LastName)
                                        .ThenBy(student => student.FirstName)
                                        .ThenBy(student => student.SecondName)
                                        .ThenBy(student => student.BirthDate)
                                        .ToList());
                        }
                    }
                } else {
                    using(var repository = new BaseRepository()) {
                        Students =
                            new ObservableCollection<Student>(
                                repository.ToList<Student>()
                                    .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                    .OrderBy(student => student.LastName)
                                    .ThenBy(student => student.FirstName)
                                    .ThenBy(student => student.SecondName)
                                    .ThenBy(student => student.BirthDate)
                                    .ToList());
                    }
                }
            }
        }

        private void FillParticipant(Student student) {
            using(var repository = new BaseRepository()) {
                Participants = new ObservableCollection<Participant>(repository.ToList<Participant>().Where(x => x.Student == student && x.Student != null).ToList());
            }

            // This is working LazyLoading?!
            /*using(var session = NHibernateConfiguration.SessionFactory.OpenSession()) {
                if(this.Participants != null && this.Participants.Any())
                    this.Participants.Clear();
                var list = session.QueryOver<Participant>().Where(p => p.Student == student).List<Participant>().ToList();
                list.ForEach(l => this.Participants.Add(l));
                foreach(var p in Participants) {
                        p.Course = session.QueryOver<Course>().Where(c => c.Id == p.Course.Id).SingleOrDefault<Course>();
                }
            }*/
        }

        private void FillClassesDates(Participant participant) {
            using(BaseRepository repository = new BaseRepository()) {
                ClassesDates = new ObservableCollection<ClassesDates>(repository.ToList<ClassesDates>().Where(x => x.Participant == participant && x.Participant != null).ToList());
            }
        }

        private void FillPayment(Participant participant) {
            using(BaseRepository repository = new BaseRepository()) {
                Payments = new ObservableCollection<Payment>(repository.ToList<Payment>().Where(x => x.Participant == participant && x.Participant != null).ToList());
            }
        }

        private void ResolveCheckboxes() {
            using(var repository = new BaseRepository()) {
                // DuringCourse checkbox
                DuringCourse = repository.ToList<Participant>().Where(x => x.Student == _student && x.EndDate == null).ToList().Count != 0;

                // OverduePayment checkbox
                decimal coursesPrice = 0;
                decimal paidCourses = 0;
                repository.ToList<Participant>().Where(x => x.Student == _student).ToList().ForEach(e => {
                    coursesPrice += e.CoursePrice;
                    repository.ToList<Payment>().Where(y => y.Participant == e).ToList().ForEach(f => paidCourses += f.Amount);
                });

                OverduePayment = coursesPrice > paidCourses;
            }
        }
        #endregion

        #endregion
    }
}