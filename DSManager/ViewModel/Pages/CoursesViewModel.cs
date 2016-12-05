﻿using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Pages {
    public class CoursesViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Course _course;
        private Participant _participant;
        #endregion

        #region Lists
        private ObservableCollection<Participant> _participants;
        #endregion

        #region Commands
        private RelayCommand _addCourse;
        private RelayCommand _editCourse;
        private RelayCommand _deleteCourse;
        #endregion

        #region View Elements
        private string _filter;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion
        
        public CoursesViewModel() {
            FillCourses();
        }

        #region Methods

        #region Selections
        public Course Course {
            get { return _course; }
            set {
                _course = value;
                RaisePropertyChanged();
                FillParticipants(_course);
            }
        }

        public Participant Participant {
            get { return _participant; }
            set {
                _participant = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<Course> Courses { get; set; }

        public ObservableCollection<Participant> Participants {
            get { return _participants; }
            set {
                _participants = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddCourse {
            get {
                return _addCourse ?? (_addCourse = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj szkolenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditCourse,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand EditCourse {
            get {
                return _editCourse ?? (_editCourse = new RelayCommand(() => {
                    if (Course == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego szkolenia!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj szkolenie" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditCourse,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage {
                        Entity = _course
                    });
                    editWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand DeleteCourse {
            get {
                return _deleteCourse ?? (_deleteCourse = new RelayCommand(async () => {
                    if(_course == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego szkolenia!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dane szkolenie?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_course);
                            }
                    }
                    FillCourses();
                }));
            }
        }
        #endregion

        #region Helpers
        // TODO dodać filtrowanie
        private void FillCourses() {
            using(var repository = new BaseRepository()) {
                Courses = new ObservableCollection<Course>(repository.ToList<Course>().ToList().OrderBy(x => x.StartDate));
            }
        }

        private void FillParticipants(Course course) {
            using(var repository = new BaseRepository()) {
                Participants = new ObservableCollection<Participant>(repository.ToList<Participant>().Where(x => x.Course == course && x.Course != null).ToList());
            }
        }
        #endregion

        #endregion
    }
}
