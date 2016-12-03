using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Interfaces;
using DSManager.Messengers;
using DSManager.Model.Entities;

namespace DSManager.ViewModel.Windows {
    public sealed class AddEditViewModel : BaseViewModel {
        public ViewModelBase Page { get; set; }

        private RelayCommand<IClosable> _closeWindow;
        private RelayCommand<IClosable> _saveCommand;

        public AddEditViewModel() {
            Messenger.Default.Register<AddEditPageMessage>(this, HandleMessage);
        }

        private void HandleMessage(AddEditPageMessage message) {
            Page = message.Page;

            if(Page != null)
                NavigateTo(Page);
        }

        public RelayCommand<IClosable> CloseWindowCommand => _closeWindow ?? (_closeWindow = new RelayCommand<IClosable>(CloseWindow));

        public RelayCommand<IClosable> SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand<IClosable>(Save));

        private void CloseWindow(IClosable window) {
            window?.Close();
        }

        private void Save(IClosable window) {
            if (!((AddEditBaseViewModel) Page).Save())
                return;

            window?.Close();
        }
    }
}