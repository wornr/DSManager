using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages {
    public class CoursesViewModel : BaseViewModel {
        private Course _course;
        private Participant _participant;
        private ObservableCollection<Participant> _participants;

        public CoursesViewModel() {
            using (var repository = new BaseRepository()) {
                Courses = new ObservableCollection<Course>(repository.ToList<Course>().ToList().OrderBy(x => x.StartDate));
            }
        }

        public Participant Participant {
            get { return _participant; }
            set {
                _participant = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Participant> Participants {
            get { return _participants; }
            set {
                _participants = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Course> Courses { get; set; }

        public Course Course {
            get { return _course; }
            set {
                _course = value;
                RaisePropertyChanged();
                FillParticipants(_course);
            }
        }

        private void FillParticipants(Course course) {
            using (var repository = new BaseRepository()) {
                Participants = new ObservableCollection<Participant>(repository.ToList<Participant>().Where(x => x.Course == course && x.Course != null).ToList());
            }
        }
    }
}
