using GalaSoft.MvvmLight;

namespace DSManager.ViewModel {
    public abstract class AddEditBaseViewModel : ViewModelBase {
        public abstract int Save();
        public abstract bool Validate();
    }
}