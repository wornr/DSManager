using System.Windows;

using GalaSoft.MvvmLight.Command;

using MahApps.Metro.Controls.Dialogs;

using DSManager.Model;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.Utilities;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Windows {
    public class SignInViewModel : BaseViewModel {
        private RelayCommand<object> _signInCommand;
        RelayCommand _cancelCommand;
        private string _login;

        public SignInViewModel() {
            NHibernateConfiguration.Initialize();
        }

        #region Commands
        public RelayCommand<object> SignInCommand {
            get {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(param => {
                    if((bool)Properties.Settings.Default["DeveloperMode"]) {
                        var windowInstance = param as SignInWindow;

                        if (SignedUser == null) {
                            SignedUser = new User {
                                FirstName = "Developer",
                                LastName = "Mode"
                            };
                            MainWindow = new MainWindow();
                            MainWindow.Show();
                        }
                        Locked = false;
                        windowInstance?.Close();
                    } else {
                        var windowInstance = param as SignInWindow;
                        if(windowInstance != null) {
                            string password = MD5Encrypter.Encrypt(windowInstance.Password.Password);
                            User user = UserRepository.GetUser(_login, password);

                            if (SignedUser == null) {
                                if (user != null && user.Active) {
                                    SignedUser = user;
                                    Locked = false;
                                    MainWindow = new MainWindow();
                                    MainWindow.Show();
                                    windowInstance.Close();
                                    return;
                                }
                                if (user != null && !user.Active) {
                                    windowInstance.ShowMessageAsync("Błąd", "Konto jest nieaktywne!\nSkontaktuj się z administratorem.");
                                    return;
                                }
                            } else {
                                if (user != null && SignedUser == user && user.Active) {
                                    Locked = false;
                                    windowInstance.Close();
                                    return;
                                }
                                if(user != null && SignedUser == user && !user.Active) {
                                    windowInstance.ShowMessageAsync("Błąd", "Konto jest nieaktywne!\nSkontaktuj się z administratorem.");
                                    return;
                                }
                            }

                            windowInstance.ShowMessageAsync("Błąd", "Podano błędne dane logowania!");
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

        #region View Elements
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
