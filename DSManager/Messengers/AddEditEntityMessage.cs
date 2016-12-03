using DSManager.Model.Entities;

namespace DSManager.Messengers {
    public class AddEditEntityMessage {
        public BaseEntity Entity { get; set; }
    }
}