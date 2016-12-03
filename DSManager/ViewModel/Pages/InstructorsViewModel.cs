using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
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
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        public InstructorsViewModel() {
            FillInstructors(_filter);
        }

        #region Methods

        #region Selections
        public Instructor Instructor {
            get { return _instructor; }
            set {
                if(_instructor == value)
                    return;
                _instructor = value;
                FillParticipant(value);
                RaisePropertyChanged();
            }
        }

        public Participant Participant {
            get { return _participant; }
            set {
                if(_participant == value)
                    return;
                _participant = value;
                FillClassesDates(value);
                FillExamsDates(value);
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
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = null
                    });
                    addWindow.Show();
                }));
            }
        }

        public RelayCommand EditInstructor {
            get {
                return _editInstructor ?? (_editInstructor = new RelayCommand(() => {
                    if(Instructor == null)
                        // TODO wyrzucić komunikat "Nie wybrano żadnego kursanta"
                        return;
                    var editWindow = new AddEditWindow { Title = "Edytuj instruktora" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditInstructor,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = _instructor
                    });
                    editWindow.Show();
                }));
            }
        }

        public RelayCommand DeleteInstructor {
            get {
                return _deleteInstructor ?? (_deleteInstructor = new RelayCommand(() => {
                    if(_instructor == null) {
                        // TODO wyrzucić komunikat "Nie wybrano żadnego instruktora"
                    } else {
                        // TODO wyrzucić dialog z zapytaniem "Czy jesteś pewien, że chcesz usunąć danego instruktora?"
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
                    FillInstructors(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterInstructors {
            get {
                return _filterInstructors ?? (_filterInstructors = new RelayCommand(() => {
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
        #endregion

        #region Helpers
        private void FillInstructors(string filter) {
            if(string.IsNullOrEmpty(filter)) {
                using(var repository = new BaseRepository()) {
                    Instructors =
                        new ObservableCollection<Instructor>(
                            repository.ToList<Instructor>().OrderBy(instructor => instructor.LastName).ToList());
                }
            } else {
                if(filter.Contains(" ")) {
                    var filters = filter.Split(' ');

                    if(!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                        using(var repository = new BaseRepository()) {
                            Instructors =
                                new ObservableCollection<Instructor>(
                                    repository.ToList<Instructor>()
                                        .Where(x => x.FirstName.Contains(filters[0]) && x.LastName.Contains(filters[1]) || x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                        .OrderBy(instructor => instructor.LastName)
                                        .ToList());
                        }
                    } else {
                        using(var repository = new BaseRepository()) {
                            Instructors =
                                new ObservableCollection<Instructor>(
                                    repository.ToList<Instructor>()
                                        .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                        .OrderBy(instructor => instructor.LastName)
                                        .ToList());
                        }
                    }
                } else {
                    using(var repository = new BaseRepository()) {
                        Instructors =
                            new ObservableCollection<Instructor>(
                                repository.ToList<Instructor>()
                                    .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                    .OrderBy(instructor => instructor.LastName)
                                    .ToList());
                    }
                }
            }
        }

        private void FillParticipant(Instructor instructor) {
            using(var repository = new BaseRepository()) {
                Participants = new ObservableCollection<Participant>(repository.ToList<Participant>().Where(x => x.Instructor == instructor && x.Instructor != null).ToList());
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
            using(var repository = new BaseRepository()) {
                ClassesDates = participant != null
                    ?
                    new ObservableCollection<ClassesDates>(repository.ToList<ClassesDates>().Where(x => x.Participant == participant && x.Participant != null && x.Instructor == participant.Instructor && x.Instructor != null).ToList())
                    :
                    new ObservableCollection<ClassesDates>();
            }
        }

        private void FillExamsDates(Participant participant) {
            using(var repository = new BaseRepository()) {
                ExamsDates = participant != null
                    ?
                    new ObservableCollection<ExamsDates>(repository.ToList<ExamsDates>().Where(x => x.Participant == participant && x.Participant != null && x.Instructor == participant.Instructor && x.Instructor != null).ToList())
                    :
                    new ObservableCollection<ExamsDates>();
            }
        }
        #endregion

        #endregion
    }
}