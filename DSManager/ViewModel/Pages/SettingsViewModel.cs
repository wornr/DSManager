using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using DSManager.Messengers;
using DSManager.Model.Entities;
using DSManager.Model.Services;
using DSManager.View.Windows;

namespace DSManager.ViewModel.Pages {
    public class SettingsViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Prices _selectedPrice;
        private DurationTime _selectedDurationTime;
        private MinimalAge _selectedMinimalAge;
        #endregion

        #region Lists
        private ObservableCollection<Prices> _prices;
        private ObservableCollection<DurationTime> _durationTimes;
        private ObservableCollection<MinimalAge> _minimalAges;
        #endregion

        #region Commands
        private RelayCommand _addPrice;
        private RelayCommand _editPrice;
        private RelayCommand _deletePrice;
        private RelayCommand _refreshPrices;
        private RelayCommand _addDurationTime;
        private RelayCommand _editDurationTime;
        private RelayCommand _deleteDurationTime;
        private RelayCommand _refreshDurationTimes;
        private RelayCommand _addMinimalAge;
        private RelayCommand _editMinimalAge;
        private RelayCommand _deleteMinimalAge;
        private RelayCommand _refreshMinimalAges;
        #endregion

        #region View Elements
        private bool _isPricesLoading;
        private bool _isDurationTimesLoading;
        private bool _isMinimalAgesLoading;
        #endregion

        #endregion

        public SettingsViewModel() {
            FillPrices();
            FillDurationTimes();
            FillMinimalAges();
        }

        #region Methods

        #region Selections
        public Prices SelectedPrice {
            get { return _selectedPrice; }
            set {
                _selectedPrice = value;
                RaisePropertyChanged();
            }
        }

        public DurationTime SelectedDurationTime {
            get { return _selectedDurationTime; }
            set {
                _selectedDurationTime = value;
                RaisePropertyChanged();
            }
        }

        public MinimalAge SelectedMinimalAge {
            get { return _selectedMinimalAge; }
            set {
                _selectedMinimalAge = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<Prices> Prices {
            get {
                return _prices;
            }
            set {
                _prices = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DurationTime> DurationTimes {
            get {
                return _durationTimes;
            }
            set {
                _durationTimes = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MinimalAge> MinimalAges {
            get {
                return _minimalAges;
            }
            set {
                _minimalAges = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddPrice {
            get {
                return _addPrice ?? (_addPrice = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj cenę" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditPrices,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Prices> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                    FillPrices();
                }));
            }
        }

        public RelayCommand EditPrice {
            get {
                return _editPrice ?? (_editPrice = new RelayCommand(() => {
                    if(SelectedPrice == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj cenę" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditPrices,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Prices> {
                        Entity = SelectedPrice
                    });
                    editWindow.ShowDialog();
                    FillPrices();
                    SelectedPrice = SelectedPrice;
                }));
            }
        }
        public RelayCommand DeletePrice {
            get {
                return _deletePrice ?? (_deletePrice = new RelayCommand(async () => {
                    if(_selectedPrice == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dany wpis?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_selectedPrice);
                            }
                    }
                    FillPrices();
                }));
            }
        }

        public RelayCommand RefreshPrices {
            get {
                return _refreshPrices ?? (_refreshPrices = new RelayCommand(() => {
                    SelectedPrice = null;
                    FillPrices();
                }));
            }
        }

        public RelayCommand AddDurationTime {
            get {
                return _addDurationTime ?? (_addDurationTime = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj czas trwania szkolenia" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditDurationTime,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<DurationTime> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                    FillDurationTimes();
                }));
            }
        }

        public RelayCommand EditDurationTime {
            get {
                return _editDurationTime ?? (_editDurationTime = new RelayCommand(() => {
                    if(SelectedDurationTime == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj czas trwania szkolenia" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditDurationTime,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<DurationTime> {
                        Entity = SelectedDurationTime
                    });
                    editWindow.ShowDialog();
                    FillDurationTimes();
                    SelectedDurationTime = SelectedDurationTime;
                }));
            }
        }
        public RelayCommand DeleteDurationTime {
            get {
                return _deleteDurationTime ?? (_deleteDurationTime = new RelayCommand(async () => {
                    if(_selectedDurationTime == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dany wpis?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_selectedDurationTime);
                            }
                    }
                    FillDurationTimes();
                }));
            }
        }

        public RelayCommand RefreshDurationTimes {
            get {
                return _refreshDurationTimes ?? (_refreshDurationTimes = new RelayCommand(() => {
                    SelectedDurationTime = null;
                    FillDurationTimes();
                }));
            }
        }

        public RelayCommand AddMinimalAge {
            get {
                return _addMinimalAge ?? (_addMinimalAge = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj minimalny wiek" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditMinimalAge,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<MinimalAge> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                    FillMinimalAges();
                }));
            }
        }

        public RelayCommand EditMinimalAge {
            get {
                return _editMinimalAge ?? (_editMinimalAge = new RelayCommand(() => {
                    if(SelectedMinimalAge == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj minimalny wiek" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditMinimalAge,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<MinimalAge> {
                        Entity = SelectedMinimalAge
                    });
                    editWindow.ShowDialog();
                    FillMinimalAges();
                    SelectedMinimalAge = SelectedMinimalAge;
                }));
            }
        }
        public RelayCommand DeleteMinimalAge {
            get {
                return _deleteMinimalAge ?? (_deleteMinimalAge = new RelayCommand(async () => {
                    if(_selectedMinimalAge == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego wpisu!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dany wpis?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_selectedMinimalAge);
                            }
                    }
                    FillMinimalAges();
                }));
            }
        }

        public RelayCommand RefreshMinimalAges {
            get {
                return _refreshMinimalAges ?? (_refreshMinimalAges = new RelayCommand(() => {
                    SelectedMinimalAge = null;
                    FillMinimalAges();
                }));
            }
        }
        #endregion

        #region View Elements
        public bool IsPricesLoading {
            get { return _isPricesLoading; }
            set {
                _isPricesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDurationTimesLoading {
            get { return _isDurationTimesLoading; }
            set {
                _isDurationTimesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsMinimalAgesLoading {
            get { return _isMinimalAgesLoading; }
            set {
                _isMinimalAgesLoading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private async void FillPrices() {
            IsPricesLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    Prices = new ObservableCollection<Prices>(repository.ToList<Prices>().OrderBy(x => x.Category).ThenBy(x => x.CourseType).ThenBy(x => x.StartDate));
                }
            });

            IsPricesLoading = false;
        }

        private async void FillDurationTimes() {
            IsDurationTimesLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    DurationTimes = new ObservableCollection<DurationTime>(repository.ToList<DurationTime>().OrderBy(x => x.Category).ThenBy(x => x.CourseKind).ThenBy(x => x.StartDate));
                }
            });

            IsDurationTimesLoading = false;
        }

        private async void FillMinimalAges() {
            IsDurationTimesLoading = true;

            await Task.Run(() => {
                using(var repository = new BaseRepository()) {
                    MinimalAges = new ObservableCollection<MinimalAge>(repository.ToList<MinimalAge>().OrderBy(x => x.Category).ThenBy(x => x.StartDate));
                }
            });

            IsDurationTimesLoading = false;
        }
        #endregion

        #endregion
    }
}
