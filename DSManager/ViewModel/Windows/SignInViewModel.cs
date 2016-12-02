using System;
using System.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using DSManager.Model;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Windows {
    public class SignInViewModel : ViewModelBase {
        private RelayCommand<object> _signInCommand;
        RelayCommand _cancelCommand;
        private string _login;

        public SignInViewModel() {
            NHibernateConfiguration.Initialize();
        }

        #region Commands
        public RelayCommand<object> SignInCommand {
            get {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>((param) => {
                    if((bool)Properties.Settings.Default["DeveloperMode"]) {
                        UserSignedIn.User = new User {
                            FirstName = "Developer",
                            LastName = "Mode"
                        };

                        var windowInstance = param as SignInWindow;
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        windowInstance?.Close();
                    } else {
                        if(param is SignInWindow) {
                            var windowInstance = param as SignInWindow;
                            string password = MD5Encrypter.Encrypt(windowInstance.Password.Password);
                            User user = UserRepository.GetUser(_login, password);

                            if(user != null) {
                                UserSignedIn.User = user;
                                var mainWindow = new MainWindow();
                                mainWindow.Show();
                                windowInstance.Close();
                            } else {
                                // TODO wyrzucić ładniejszy komunikat o błędnych danych logowania + i18n
                                MessageBox.Show("Podano błędne dane logowania");
                            }
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

        #region Getters/Setters
        public string Login {
            get { return _login; }
            set {
                if(_login == value)
                    return;
                _login = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
