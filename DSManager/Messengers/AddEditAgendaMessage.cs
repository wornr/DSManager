using System;
using DSManager.Model.Entities;

namespace DSManager.Messengers {
    public class AddEditAgendaMessage<T, U>
        where T : BaseEntity<T>
        where U : BaseEntity<U> {
            public BaseEntity<T> Entity { get; set; }
            public BaseEntity<U> Owner { get; set; }
            public DateTime? StartDate { get; set; }
    }
}