using System;
using System.Windows;

using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using WpfScheduler;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Utilities;
using DSManager.View.Windows;
using DSManager.ViewModel;

namespace DSManager.View.Pages {
    public partial class AgendaPage {
        public AgendaPage() {
            InitializeComponent();
        }

        private void InstructorScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            if(Instructors.SelectedItem == null) {
                (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Błąd", "Nie wybrano żadnego instruktora!");
            } else {
                var addWindow = new AddEditWindow { Title = "Dodaj termin" };
                Messenger.Default.Send(new AddEditPageMessage {
                    Page = ViewModelLocator.Instance.AddEditAgenda
                });
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Instructor> {
                    Owner = (Instructor) Instructors.SelectedItem,
                    StartDate = e
                });
                addWindow.ShowDialog();
            }
        }

        private void StudentScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            if(Students.SelectedItem == null) {
                (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Błąd", "Nie wybrano żadnego kursanta!");
            } else {
                var addWindow = new AddEditWindow { Title = "Dodaj termin" };
                Messenger.Default.Send(new AddEditPageMessage {
                    Page = ViewModelLocator.Instance.AddEditAgenda
                });
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Student>  {
                    Owner = (Student) Students.SelectedItem,
                    StartDate = e
                });
                addWindow.ShowDialog();
            }
        }

        private void CarScheduler_OnOnScheduleDoubleClick(object sender, DateTime e) {
            if(Cars.SelectedItem == null) {
                (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Błąd", "Nie wybrano żadnego pojazdu!");
            } else {
                var addWindow = new AddEditWindow { Title = "Dodaj termin" };
                Messenger.Default.Send(new AddEditPageMessage {
                    Page = ViewModelLocator.Instance.AddEditAgenda
                });
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Car> {
                    Owner = (Car) Cars.SelectedItem,
                    StartDate = e
                });
                addWindow.ShowDialog();
            }
        }

        private void InstructorScheduler_OnOnEventDoubleClick(object sender, Event e) {
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda
            });
            if (e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Instructor> {
                    Entity = e.ClassesDates,
                    Owner = e.Instructor
                });
            } else if (e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates, Instructor> {
                    Entity = e.ExamsDates,
                    Owner = e.Instructor
                });
            } else if (e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates, Instructor> {
                    Entity = e.LockedDates,
                    Owner = e.Instructor
                });
            }
            editWindow.ShowDialog();
        }

        private void StudentScheduler_OnOnEventDoubleClick(object sender, Event e) {
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda
            });
            if(e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Participant> {
                    Entity = e.ClassesDates,
                    Owner = e.Participant
                });
            } else if(e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates, Participant> {
                    Entity = e.ExamsDates,
                    Owner = e.Participant
                });
            } else if(e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates, Participant> {
                    Entity = e.LockedDates,
                    Owner = e.Participant
                });
            }
            editWindow.ShowDialog();
        }

        private void CarScheduler_OnOnEventDoubleClick(object sender, Event e) {
            var editWindow = new AddEditWindow { Title = "Edytuj termin" };
            Messenger.Default.Send(new AddEditPageMessage {
                Page = ViewModelLocator.Instance.AddEditAgenda
            });
            if(e.ClassesDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ClassesDates, Car> {
                    Entity = e.ClassesDates,
                    Owner = e.Car
                });
            } else if(e.ExamsDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<ExamsDates, Car> {
                    Entity = e.ExamsDates,
                    Owner = e.Car
                });
            } else if(e.LockedDates != null) {
                Messenger.Default.Send(new AddEditAgendaMessage<LockedDates, Car> {
                    Entity = e.LockedDates,
                    Owner = e.Car
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
