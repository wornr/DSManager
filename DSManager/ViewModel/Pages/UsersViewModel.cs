﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Enums;
using DSManager.Model.Services;
using DSManager.View.Windows;


namespace DSManager.ViewModel.Pages {
    public class UsersViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private User _user;
        #endregion

        #region Lists
        private ObservableCollection<User> _users;
        #endregion

        #region Commands
        private RelayCommand _addUser;
        private RelayCommand _editUser;
        private RelayCommand _deleteUser;
        private RelayCommand _refreshUsers;
        private RelayCommand _filterUsers;
        #endregion

        #region View Elements
        private string _filter;
        private bool _isUsersLoading;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion

        public UsersViewModel() {
            _filter = _prevFilter = string.Empty;
            FillUsers(_filter);
        }

        #region Methods

        #region Selections
        public User User {
            get { return _user; }
            set {
                if(_user == value)
                    return;
                _user = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<User> Users {
            get {
                return _users;
            }
            set {
                _users = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddUser {
            get {
                return _addUser ?? (_addUser = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj użytkownika" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditUser,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<User> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                    FillUsers(_filter);
                }));
            }
        }

        public RelayCommand EditUser {
            get {
                return _editUser ?? (_editUser = new RelayCommand(() => {
                    if(User == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego użytkownika!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj użytkownika" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditUser,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<User> {
                        Entity = User
                    });
                    editWindow.ShowDialog();
                    FillUsers(_filter);
                    User = _user;
                }));
            }
        }
        public RelayCommand DeleteUser {
            get {
                return _deleteUser ?? (_deleteUser = new RelayCommand(async () => {
                    if(_user == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego użytkownika!");
                    } else {
                        if (_user.AccountType == AccountType.Boss) {
                            ShowDialog("Błąd", "Nie można usunąć użytkownika z najwyższymi uprawnieniami!");
                        } else {
                            if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć danego użytkownika?"))
                                using(var repository = new BaseRepository()) {
                                    repository.Delete(_user);
                                }
                        }
                    }
                    FillUsers(_filter);
                }));
            }
        }

        public RelayCommand RefreshUsers {
            get {
                return _refreshUsers ?? (_refreshUsers = new RelayCommand(() => {
                    User = null;
                    FillUsers(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterUsers {
            get {
                return _filterUsers ?? (_filterUsers = new RelayCommand(() => {
                    if (_filter.Equals(_prevFilter))
                        return;

                    FillUsers(_filter);
                    _prevFilter = _filter;
                }));
            }
        }
        #endregion

        #region View Elements
        public string Filter {
            get { return _filter; }
            set {
                if(_filter == value)
                    return;
                _filter = value;
                RaisePropertyChanged();
            }
        }

        public bool IsUsersLoading {
            get { return _isUsersLoading; }
            set {
                _isUsersLoading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private async void FillUsers(string filter) {
            IsUsersLoading = true;

            await Task.Run(() => {
                if (string.IsNullOrEmpty(filter)) {
                    using (var repository = new BaseRepository()) {
                        Users =
                            new ObservableCollection<User>(
                                repository.ToList<User>().OrderBy(user => user.LastName).ToList());
                    }
                } else {
                    if (filter.Contains(" ")) {
                        var filters = filter.Split(' ');

                        if (!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                            using (var repository = new BaseRepository()) {
                                Users =
                                    new ObservableCollection<User>(
                                        repository.ToList<User>()
                                            .Where(
                                                x =>
                                                    x.FirstName.Contains(filters[0]) &&
                                                    x.LastName.Contains(filters[1]) ||
                                                    x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                            .OrderBy(user => user.LastName)
                                            .ToList());
                            }
                        } else {
                            using (var repository = new BaseRepository()) {
                                Users =
                                    new ObservableCollection<User>(
                                        repository.ToList<User>()
                                            .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                            .OrderBy(user => user.LastName)
                                            .ToList());
                            }
                        }
                    } else {
                        using (var repository = new BaseRepository()) {
                            Users =
                                new ObservableCollection<User>(
                                    repository.ToList<User>()
                                        .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                        .OrderBy(user => user.LastName)
                                        .ToList());
                        }
                    }
                }
            });

            IsUsersLoading = false;
        }
        #endregion

        #endregion
    }
}
