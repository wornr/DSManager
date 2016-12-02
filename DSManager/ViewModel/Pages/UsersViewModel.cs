using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages {
    public class UsersViewModel : BaseViewModel {
        private ObservableCollection<User> _users;
        private RelayCommand _filterUsers;
        private RelayCommand _deleteUser;
        private RelayCommand _refreshUsers;
        private User _user;
        private string _filter;
        private string _prevFilter;
        public UsersViewModel() {
            FillUsers(_filter);
        }

        public string Filter {
            get { return _filter; }
            set {
                if(_filter == value)
                    return;
                _filter = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand FilterUsers {
            get {
                return _filterUsers ?? (_filterUsers = new RelayCommand(() => {
                    FillUsers(_filter);
                    _prevFilter = _filter;
                }));
            }
        }

        public RelayCommand DeleteUser {
            get {
                return _deleteUser ?? (_deleteUser = new RelayCommand(() => {
                    if(_user == null) {
                        // TODO wyrzucić komunikat "Nie wybrano żadnego kursanta"
                    } else {
                        // TODO wyrzucić dialog z zapytaniem "Czy jesteś pewien, że chcesz usunąć danego kursanta?"
                        using(var repository = new BaseRepository()) {
                            repository.Delete(_user);
                        }
                    }
                    FillUsers(_filter);
                }));
            }
        }

        public RelayCommand RefreshUsers {
            get {
                return _refreshUsers ?? (_refreshUsers = new RelayCommand(() => {
                    FillUsers(_prevFilter);
                }));
            }
        }

        public User User {
            get { return _user; }
            set {
                if(_user == value)
                    return;
                _user = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<User> Users {
            get {
                return _users;
            }
            set {
                _users = value;
                RaisePropertyChanged();
            }
        }
        private void FillUsers(string filter) {
            if(string.IsNullOrEmpty(filter)) {
                using(var repository = new BaseRepository()) {
                    Users =
                        new ObservableCollection<User>(
                            repository.ToList<User>().OrderBy(user => user.LastName).ToList());
                }
            } else {
                if(filter.Contains(" ")) {
                    var filters = filter.Split(' ');

                    if(!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                        using(var repository = new BaseRepository()) {
                            Users =
                                new ObservableCollection<User>(
                                    repository.ToList<User>()
                                        .Where(x => x.FirstName.Contains(filters[0]) && x.LastName.Contains(filters[1]) || x.FirstName.Contains(filters[1]) && x.LastName.Contains(filters[0]))
                                        .OrderBy(user => user.LastName)
                                        .ToList());
                        }
                    } else {
                        using(var repository = new BaseRepository()) {
                            Users =
                                new ObservableCollection<User>(
                                    repository.ToList<User>()
                                        .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                        .OrderBy(user => user.LastName)
                                        .ToList());
                        }
                    }
                } else {
                    using(var repository = new BaseRepository()) {
                        Users =
                            new ObservableCollection<User>(
                                repository.ToList<User>()
                                    .Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter))
                                    .OrderBy(user => user.LastName)
                                    .ToList());
                    }
                }
            }
        }
    }
}
