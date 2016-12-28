using System;
using System.Windows.Media;
using DSManager.Model.Entities;

namespace WpfScheduler
{
    public class Event {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start {get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public Brush Color { get; set; }
        public ClassesDates ClassesDates { get; set; }
        public ExamsDates ExamsDates { get; set; }
        public LockedDates LockedDates { get; set; }
        public Participant Participant { get; set; }
        public Instructor Instructor { get; set; }
        public Car Car { get; set; }


        public Event()
        {
            Id = Guid.NewGuid();
        }
    }
}
