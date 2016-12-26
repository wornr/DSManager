using DSManager.Model.Entities;

namespace DSManager.Messengers {
    public class AddEditAgendaMessage<T> where T : BaseEntity<T> {
            public BaseEntity<T> Entity { get; set; }
            public Participant Participant { get; set; }
            public Instructor Instructor { get; set; }
            public Car Car { get; set; }
    }
}