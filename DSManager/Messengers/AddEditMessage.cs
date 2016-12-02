using DSManager.Model.Entities;
using GalaSoft.MvvmLight;

namespace DSManager.Messengers {
    public class AddEditMessage {
        public ViewModelBase Page { get; set; }
        public BaseEntity Entity { get; set; }
    }
}
