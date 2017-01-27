using System.Windows;

using GalaSoft.MvvmLight.Command;

using MahApps.Metro.Controls.Dialogs;

using DSManager.Model;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Windows {
    public class CreateAccountViewModel : BaseViewModel {
        private RelayCommand<object> _createAccountCommand;
        RelayCommand _cancelCommand;

        public CreateAccountViewModel() {
            NHibernateConfiguration.Initialize();
        }

        #region Commands
        public RelayCommand<object> CreateAccountCommand {
            get {
                return _createAccountCommand ?? (_createAccountCommand = new RelayCommand<object>(param => {
                    var windowInstance = param as CreateAccountWindow;
                    if(windowInstance != null) {
                        if (windowInstance.Password.Password.Equals(windowInstance.ConfirmPassword.Password)) {
                            var password = MD5Encrypter.Encrypt(windowInstance.Password.Password);
                            SignedUser.Password = password;
                            SignedUser.ConfirmationKey = "";
                            SignedUser.Active = true;
                            using (var repository = new BaseRepository()) {
                                repository.Save(SignedUser);
                            }
                            MainWindow = new MainWindow();
                            MainWindow.Show();
                            windowInstance.Close();
                        } else {
                            windowInstance.ShowMessageAsync("Błąd", "Wprowadzone hasła nie są identyczne.");
                        }
                    }
                }));
            }
        }

        public RelayCommand CancelCommand {
            get {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() => {
                    Application.Current.Shutdown();
                }));
            }
        }
        #endregion
    }
}
