using System;
using System.Windows.Media;
using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Utilities;
using DSManager.View.Windows;
using DSManager.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using WpfScheduler;

namespace DSManager.View.Pages {
    public partial class AgendaPage {
        public AgendaPage() {
            InitializeComponent();
        }

        private void InstructorScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            /*if(Instructor == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego instruktora!");
            } else {*/
            var addWindow = new AddEditWindow { Title = "Dodaj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda,
            });
            Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates> {
                Entity = null,
                Instructor = null, // TODO tutaj ma być podawany obiekt Instructor wybrany z DataGrid
                Participant = null,
                Car = null
            });
            addWindow.ShowDialog();
            //}
        }

        private void StudentScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            /*if(Participant == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego kursanta!");
            } else {*/
                var addWindow = new AddEditWindow { Title = "Dodaj termin" };
                Messenger.Default.Send(new AddEditPageMessage {
                    Page = ViewModelLocator.Instance.AddEditAgenda,
                });
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates>  {
                    Entity = null,
                    Instructor = null,
                    Participant = null, // TODO tutaj ma być podawany obiekt Participant wybrany z DataGrid
                    Car = null
                });
                addWindow.ShowDialog();
            //}
        }

        private void CarScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            /*if(Car == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego pojazdu!");
            } else {*/
            var addWindow = new AddEditWindow { Title = "Dodaj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda,
            });
            Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates> {
                Entity = null,
                Instructor = null,
                Participant = null,
                Car = null // TODO tutaj ma być podawany obiekt Car wybrany z DataGrid
            });
            addWindow.ShowDialog();
            //}
        }

        private void InstructorScheduler_OnOnEventDoubleClick(object sender, Event e) {
            /*if(Instructor == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego instruktora!");
                return;
            }*/
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda,
            });
            if (e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates> {
                    Entity = e.ClassesDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if (e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates> {
                    Entity = e.ExamsDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if (e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates> {
                    Entity = e.LockedDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            }
            editWindow.ShowDialog();
        }

        private void StudentScheduler_OnOnEventDoubleClick(object sender, Event e) {
            /*if(Participant == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego kursanta!");
                return;
            }*/
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda,
            });
            if(e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates> {
                    Entity = e.ClassesDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if(e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates> {
                    Entity = e.ExamsDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if(e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates> {
                    Entity = e.LockedDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            }
            editWindow.ShowDialog();
        }

        private void CarScheduler_OnOnEventDoubleClick(object sender, Event e) {
            /*if(Car == null) {
                ShowDialog("Błąd", "Nie wybrano żadnego pojazdu!");
                return;
            }*/
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda,
            });
            if(e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates> {
                    Entity = e.ClassesDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if(e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates> {
                    Entity = e.ExamsDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            } else if(e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates> {
                    Entity = e.LockedDates,
                    Instructor = e.Instructor,
                    Participant = e.Participant,
                    Car = e.Car
                });
            }
            editWindow.ShowDialog();
        }

        private void Scheduler_OnOnEventMouseEnter(object sender, Event e) {
            ((EventUserControl)sender).EventElement.Background = BrushesConverter.GetLighter(((EventUserControl)sender).EventElement.Background);
        }

        private void Scheduler_OnOnEventMouseLeave(object sender, Event e) {
            ((EventUserControl)sender).EventElement.Background = BrushesConverter.GetDarker(((EventUserControl)sender).EventElement.Background);
        }
    }
}
