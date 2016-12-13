using DSManager.Model.Entities;

namespace DSManager.Messengers {
    public class AddEditEntityMessage<T> where T : BaseEntity<T> {
        public BaseEntity<T> Entity { get; set; }
    }
}