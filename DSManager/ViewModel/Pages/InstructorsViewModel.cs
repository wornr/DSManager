using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Pages {
    public class InstructorsViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Instructor _instructor;
        private Participant _participant;
        #endregion

        #region Lists
        private ObservableCollection<Instructor> _instructors;
        private ObservableCollection<Participant> _participants;
        private ObservableCollection<ClassesDates> _classesDates;
        private ObservableCollection<ExamsDates> _examsDates;
        #endregion

        #region Commands
        private RelayCommand _addInstructor;
        private RelayCommand _editInstructor;
        private RelayCommand _deleteInstructor;
        private RelayCommand _refreshInstructors;
        private RelayCommand _filterInstructors;
        #endregion

        #region View Elements
        private string _filter;
        private bool _isInstructorsLoading;
        private bool _isParticipantsLoading;
        private bool _isClassesDatesLoading;
        private bool _isExamsDatesLoading;
        private bool _instructorsMgmtPermission;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        public InstructorsViewModel() {
            _instructorsMgmtPermission = CheckPermissions.CheckPermission(SignedUser.AccountType, "InstructorsManagement");
            _filter = _prevFilter = string.Empty;
            FillInstructors(_filter);
        }

        #region Methods

        #region Selections
        public Instructor Instructor {
            get { return _instructor; }
            set {
                _instructor = value;
                FillParticipant(value);
                RaisePropertyChanged();
            }
        }

        public Participant Participant {
            get { return _participant; }
            set {
                _participant = value;
                FillClassesDates(value);
                FillExamsDates(value);
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<Instructor> Instructors {
            get {
                return _instructors;
            }
            set {
                _instructors = value;
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

        public ObservableCollection<ExamsDates> ExamsDates {
            get {
                return _examsDates;
            }
            set {
                _examsDates = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddInstructor {
            get {
                return _addInstructor ?? (_addInstructor = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj instruktora" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditInstructor,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Instructor> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                    FillInstructors(_filter);
                }));
            }
        }

        public RelayCommand EditInstructor {
            get {
                return _editInstructor ?? (_editInstructor = new RelayCommand(() => {
                    if(Instructor == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego instruktora!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj instruktora" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditInstructor,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Instructor> {
                        Entity = _instructor
                    });
                    editWindow.ShowDialog();
                    FillInstructors(_filter);
                    Instructor = _instructor;
                }));
            }
        }

        public RelayCommand DeleteInstructor {
            get {
                return _deleteInstructor ?? (_deleteInstructor = new RelayCommand(async () => {
                    if(_instructor == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego instruktora!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć danego instruktora?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_instructor);
                            }
                    }
                    FillInstructors(_filter);
                }));
            }
        }

        public RelayCommand RefreshInstructors {
            get {
                return _refreshInstructors ?? (_refreshInstructors = new RelayCommand(() => {
                    Instructor = null;
                    FillInstructors(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterInstructors {
            get {
                return _filterInstructors ?? (_filterInstructors = new RelayCommand(() => {
                    if (_filter.Equals(_prevFilter))
                        return;

                    FillInstructors(_filter);
                    _prevFilter = _filter;
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

        public bool IsInstructorsLoading {
            get { return _isInstructorsLoading; }
            set {
                _isInstructorsLoading = value;
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

        public bool IsExamsDatesLoading {
            get { return _isExamsDatesLoading; }
            set {
                _isExamsDatesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool InstructorsMgmtPermission {
            get { return _instructorsMgmtPermission; }
            set {
                _instructorsMgmtPermission = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private async void FillInstructors(string filter) {
            IsInstructorsLoading = true;

            await Task.Run(() => {
                if (string.IsNullOrEmpty(filter)) {
                    using (var repository = new BaseRepository()) {
                        Instructors =
                            new ObservableCollection<Instructor>(
                                repository.ToList<Instructor>().OrderBy(instructor => instructor.LastName).ToList());
                    }
                } else {
                    if (filter.Contains(" ")) {
                        var filters = filter.Split(' ');

                        if (!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                            using (var repository = new BaseRepository()) {
                                Instructors =
                                    new ObservableCollection<Instructor>(
                                        repository.ToList<Instructor>()
                                            .Where(
                                                x =>
                                                    x.FirstName.Contains(filters[0]) &&
                                                    x.LastName.Contains(filters[1]) ||
                                                    x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                            .OrderBy(instructor => instructor.LastName)
                                            .ToList());
                            }
                        } else {
                            using (var repository = new BaseRepository()) {
                                Instructors =
                                    new ObservableCollection<Instructor>(
                                        repository.ToList<Instructor>()
                                            .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                            .OrderBy(instructor => instructor.LastName)
                                            .ToList());
                            }
                        }
                    } else {
                        using (var repository = new BaseRepository()) {
                            Instructors =
                                new ObservableCollection<Instructor>(
                                    repository.ToList<Instructor>()
                                        .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                        .OrderBy(instructor => instructor.LastName)
                                        .ToList());
                        }
                    }
                }
            });

            IsInstructorsLoading = false;
        }

        private async void FillParticipant(Instructor instructor) {
            IsParticipantsLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    Participants =
                        new ObservableCollection<Participant>(
                            repository.ToList<Participant>()
                                .Where(x => x.Instructor == instructor && x.Instructor != null)
                                .ToList());
                }
            });

            IsParticipantsLoading = false;
        }

        private async void FillClassesDates(Participant participant) {
            IsClassesDatesLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    ClassesDates = participant != null
                        ? new ObservableCollection<ClassesDates>(
                            repository.ToList<ClassesDates>()
                                .Where(
                                    x =>
                                        x.Participant == participant && x.Participant != null &&
                                        x.Instructor == participant.Instructor && x.Instructor != null)
                                .ToList())
                        : new ObservableCollection<ClassesDates>();
                }
            });

            IsClassesDatesLoading = false;
        }

        private async void FillExamsDates(Participant participant) {
            IsExamsDatesLoading = true;

            await Task.Run(() => {
                using (var repository = new BaseRepository()) {
                    ExamsDates = participant != null
                        ? new ObservableCollection<ExamsDates>(
                            repository.ToList<ExamsDates>()
                                .Where(
                                    x =>
                                        x.Participant == participant && x.Participant != null &&
                                        x.Instructor == participant.Instructor && x.Instructor != null)
                                .ToList())
                        : new ObservableCollection<ExamsDates>();
                }
            });

            IsExamsDatesLoading = false;
        }
        #endregion

        #endregion
    }
}