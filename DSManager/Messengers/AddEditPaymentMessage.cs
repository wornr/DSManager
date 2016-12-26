using DSManager.Model.Entities;

namespace DSManager.Messengers {
    public class AddEditPaymentMessage<T> where T : BaseEntity<T> {
        public BaseEntity<T> Entity { get; set; }
        public Participant Participant { get; set; }
    }
}