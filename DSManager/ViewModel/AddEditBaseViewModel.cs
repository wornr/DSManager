using GalaSoft.MvvmLight;

namespace DSManager.ViewModel {
    public abstract class AddEditBaseViewModel : ViewModelBase {
        public abstract bool Save();
        public abstract bool Validate();
    }
}