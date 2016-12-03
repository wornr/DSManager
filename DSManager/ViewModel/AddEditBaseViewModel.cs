using DSManager.Interfaces;
using GalaSoft.MvvmLight;

namespace DSManager.ViewModel {
    public abstract class AddEditBaseViewModel : ViewModelBase, IDataSave {
        public abstract bool Save();
        public abstract bool Validate();
    }
}