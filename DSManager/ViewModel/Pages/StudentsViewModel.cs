using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.PDF.Templates;
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
        private RelayCommand _printParticipant;
        private RelayCommand _addPayment;
        private RelayCommand _editPayment;
        private RelayCommand _deletePayment;
        private RelayCommand _printPayment;
        #endregion

        #region View Elements
        private string _filter;
        private bool _duringCourse;
        private bool _overduePayment;
        private bool _isStudentsLoading;
        private bool _isParticipantsLoading;
        private bool _isClassesDatesLoading;
        private bool _isPaymentsLoading;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        
        public StudentsViewModel() {
            _filter = _prevFilter = string.Empty;
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
                RaisePropertyChanged();
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
                    Messenger.Default.Send(new AddEditEntityMessage<Student> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand EditStudent {
            get {
                return _editStudent ?? (_editStudent = new RelayCommand(() => {
                    if (Student == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego kursanta!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj kursanta" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditStudent,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Student> {
                        Entity = _student
                    });
                    editWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand DeleteStudent {
            get {
                return _deleteStudent ?? (_deleteStudent = new RelayCommand(async () => {
                    if(_student == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego kursanta!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć danego kursanta?"))
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
                    Student = null;
                    FillStudents(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterStudents {
            get {
                return _filterStudents ?? (_filterStudents = new RelayCommand(() => {
                    if(_filter.Equals(_prevFilter))
                        return;

                    FillStudents(_filter);
                    _prevFilter = _filter;
                }));
            }
        }

        public RelayCommand AddPayment {
            get {
                return _addPayment ?? (_addPayment = new RelayCommand(() => {
                    if(Participant == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego szkolenia!");
                    } else {
                        var addWindow = new AddEditWindow { Title = "Dodaj wpłatę" };
                        Messenger.Default.Send(new AddEditPageMessage {
                            Page = ViewModelLocator.Instance.AddEditPayment,
                        });
                        Messenger.Default.Send(new AddEditPaymentMessage<Payment> {
                            Entity = null,
                            Participant = _participant
                        });
                        addWindow.Width = 500;
                        addWindow.Height = 200;
                        addWindow.ShowDialog();
                    }
                }));
            }
        }

        public RelayCommand EditPayment {
            get {
                return _editPayment ?? (_editPayment = new RelayCommand(() => {
                    if(Payment == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnej wpłaty!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj wpłatę" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditPayment,
                    });
                    Messenger.Default.Send(new AddEditPaymentMessage<Payment> {
                        Entity = Payment,
                        Participant = _participant
                    });
                    editWindow.Width = 500;
                    editWindow.Height = 200;
                    editWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand DeletePayment {
            get {
                return _deletePayment ?? (_deletePayment = new RelayCommand(async () => {
                    if(Payment == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnej wpłaty!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć daną wpłatę?"))
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
                return _printPayment ?? (_printPayment = new RelayCommand(async () => {
                    Exception error = null;
                    await Task.Run(() => {
                        try {
                            new PaymentPdf(Payment);
                        } catch (Exception ex) {
                            error = ex;
                        }
                    });
                    if (error == null)
                        return;
                    if(error is IOException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nPlik jest w użyciu, zamknij plik i ponów próbę");
                    if(error is DirectoryNotFoundException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nŚcieżka zapisu nie istnieje.");
                    if(error is NotSupportedException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nBłędna nazwa pliku.");
                    if(error is Win32Exception)
                        ShowDialog("Błąd", "Nie można otworzyć pliku!\nPlik nie istnieje.");
                }));
            }
        }

        public RelayCommand PrintParticipant {
            get {
                return _printParticipant ?? (_printParticipant = new RelayCommand(async () => {
                    Exception error = null;
                    await Task.Run(() => {
                        try {
                            new CoursePdf(_participant);
                        } catch (Exception ex) {
                            error = ex;
                        }
                    });
                    if(error == null)
                        return;
                    if(error is IOException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nPlik jest w użyciu, zamknij plik i ponów próbę");
                    if(error is DirectoryNotFoundException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nŚcieżka zapisu nie istnieje.");
                    if(error is NotSupportedException)
                        ShowDialog("Błąd", "Nie można zapisać pliku!\nBłędna nazwa pliku.");
                    if(error is Win32Exception)
                        ShowDialog("Błąd", "Nie można otworzyć pliku!\nPlik nie istnieje.");
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

        public bool IsClassesDatesLoading {
            get { return _isClassesDatesLoading; }
            set {
                _isClassesDatesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPaymentsLoading {
            get { return _isPaymentsLoading; }
            set {
                _isPaymentsLoading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private async void FillStudents(string filter) {
            IsStudentsLoading = true;

            await Task.Run(() => {
                if (string.IsNullOrEmpty(filter)) {
                    using (var repository = new BaseRepository()) {
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
                    if (filter.Contains(" ")) {
                        var filters = filter.Split(' ');

                        if (!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                            using (var repository = new BaseRepository()) {
                                Students =
                                    new ObservableCollection<Student>(
                                        repository.ToList<Student>()
                                            .Where(
                                                x =>
                                                    x.FirstName.Contains(filters[0]) &&
                                                    x.LastName.Contains(filters[1]) ||
                                                    x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                            .OrderBy(student => student.LastName)
                                            .ThenBy(student => student.FirstName)
                                            .ThenBy(student => student.SecondName)
                                            .ThenBy(student => student.BirthDate)
                                            .ToList());
                            }
                        } else {
                            using (var repository = new BaseRepository()) {
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
                        using (var repository = new BaseRepository()) {
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
            });

            IsStudentsLoading = false;
        }

        private async void FillParticipant(Student student) {
            IsParticipantsLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    Participants =
                        new ObservableCollection<Participant>(
                            repository.ToList<Participant>()
                                .Where(x => x.Student == student && x.Student != null)
                                .ToList());
                }
            });
            
            IsParticipantsLoading = false;
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

        private async void FillClassesDates(Participant participant) {
            IsClassesDatesLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    ClassesDates =
                        new ObservableCollection<ClassesDates>(
                            repository.ToList<ClassesDates>()
                                .Where(x => x.Participant == participant && x.Participant != null)
                                .ToList());
                }
            });

            IsClassesDatesLoading = false;
        }

        private async void FillPayment(Participant participant) {
            IsPaymentsLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    Payments =
                        new ObservableCollection<Payment>(
                            repository.ToList<Payment>()
                                .Where(x => x.Participant == participant && x.Participant != null)
                                .ToList());
                }
            });

            IsPaymentsLoading = false;
        }

        private async void ResolveCheckboxes() {
            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    // DuringCourse checkbox
                    DuringCourse =
                        repository.ToList<Participant>()
                            .Where(x => x.Student == _student && x.EndDate == null)
                            .ToList()
                            .Count != 0;

                    // OverduePayment checkbox
                    decimal coursesPrice = 0;
                    decimal paidCourses = 0;
                    repository.ToList<Participant>().Where(x => x.Student == _student).ToList().ForEach(e => {
                        coursesPrice += e.CoursePrice;
                        repository.ToList<Payment>()
                            .Where(y => y.Participant == e)
                            .ToList()
                            .ForEach(f => paidCourses += f.Amount);
                    });

                    OverduePayment = coursesPrice > paidCourses;
                }
            });
        }
        #endregion

        #endregion
    }
}