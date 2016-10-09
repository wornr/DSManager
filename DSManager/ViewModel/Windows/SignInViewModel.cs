using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using DSManager.Model;
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
                return this._signInCommand ?? (_signInCommand = new RelayCommand<object>((param) => {
                    if(param != null && param is SignInWindow) {
                        var windowInstance = param as SignInWindow;
                        string _password = MD5Encrypter.Encrypt(windowInstance.password.Password);

                        if(UserRepository.getUser(_login, _password) != null) {
                            var MainWindow = new MainWindow();
                            MainWindow.Show();
                            windowInstance.Close();
                        } else {
                            // TODO wyrzucić ładniejszy komunikat o błędnych danych logowania + i18n
                            MessageBox.Show("Podano błędne dane logowania");
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
