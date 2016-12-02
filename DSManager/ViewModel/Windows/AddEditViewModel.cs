using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Interfaces;
using DSManager.Messengers;
using DSManager.Model.Entities;

namespace DSManager.ViewModel.Windows {
    public sealed class AddEditViewModel : BaseViewModel {
        public ViewModelBase Page { get; set; }
        public BaseEntity Entity { get; set; }

        private RelayCommand<IClosable> _closeWindow;

        public AddEditViewModel() {
            Messenger.Default.Register<AddEditMessage>(this, HandleMessage);
        }

        private void HandleMessage(AddEditMessage message) {
            Page = message.Page;
            Entity = message.Entity;

            if(Page != null)
                NavigateTo(Page);
        }

        public RelayCommand<IClosable> CloseWindowCommand => _closeWindow ?? (_closeWindow = new RelayCommand<IClosable>(CloseWindow));

        private void CloseWindow(IClosable window) {
            window?.Close();
        }
    }
}