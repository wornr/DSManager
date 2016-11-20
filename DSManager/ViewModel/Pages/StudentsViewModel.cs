using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages {
    public class StudentsViewModel : BaseViewModel {
        private Student _student;
        private Participant _participant;
        private ObservableCollection<Participant> _participants = null;
        private ObservableCollection<ClassesDates> _classesDates = null;
        private ObservableCollection<Payment> _payments = null;

        public StudentsViewModel() {

        }

        public Student Student {
            get { return _student; }
            set {
                if(_student == value)
                    return;
                _student = value;
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
                FillPayment(value);
            }
        }

        public ObservableCollection<Student> Students {
            get {
                using(BaseRepository repository = new BaseRepository()) {
                    try {
                        return new ObservableCollection<Student>(repository.ToList<Student>().OrderBy(student => student.LastName).ToList());
                    } catch {
                        return null;
                    }
                }
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

        private void FillParticipant(Student student) {
            using(BaseRepository repository = new BaseRepository()) {
                Participants = new ObservableCollection<Participant>(repository.ToList<Participant>().Where(x => x.Student == student).ToList());
            };

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
                ClassesDates = new ObservableCollection<ClassesDates>(repository.ToList<ClassesDates>().Where(x => x.Participant == participant).ToList());
            }
        }

        private void FillPayment(Participant participant) {
            using(BaseRepository repository = new BaseRepository()) {
                Payments = new ObservableCollection<Payment>(repository.ToList<Payment>().Where(x => x.Participant == participant).ToList());
            };
        }        
    }
}