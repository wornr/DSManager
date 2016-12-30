using System;
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
    public class CarsViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Car _car;
        #endregion

        #region Lists
        private ObservableCollection<Car> _cars;
        private ObservableCollection<ClassesDates> _classesDates;
        private ObservableCollection<ExamsDates> _examsDates;
        #endregion

        #region Commands
        private RelayCommand _addCar;
        private RelayCommand _editCar;
        private RelayCommand _deleteCar;
        private RelayCommand _filterCars;
        private RelayCommand _refreshCars;
        private RelayCommand _changeFiltering;
        #endregion

        #region View Elements
        private string _filter;
        private bool _actualInspection;
        private bool _actualInsurance;
        private bool _isCarsLoading;
        private bool _isClassesDatesLoading;
        private bool _isExamsDatesLoading;
        private bool _isAll;
        private bool _isLocked;
        private bool _isUnlocked;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion

        public CarsViewModel() {
            _filter = _prevFilter = string.Empty;
            IsAll = true;
            FillCars(_filter);
        }

        #region Methods

        #region Selections
        public Car Car {
            get { return _car; }
            set {
                _car = value;
                FillClassesDates(value);
                FillExamsDates(value);
                ResolveCheckboxes();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Lists
        public ObservableCollection<Car> Cars {
            get {
                return _cars;
            }
            set {
                _cars = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ClassesDates> ClassesDates {
            get {
                return _classesDates;
            }
            set {
                _classesDates = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExamsDates> ExamsDates {
            get {
                return _examsDates;
            }
            set {
                _examsDates = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> LockedCarsFilters => new ObservableCollection<string> {"Wszystkie", "Aktualne", "Nieaktualne"};
        #endregion

        #region Commands
        public RelayCommand AddCar {
            get {
                return _addCar ?? (_addCar = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj pojazd" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditCar,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Car> {
                        Entity = null
                    });
                    addWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand EditCar {
            get {
                return _editCar ?? (_editCar = new RelayCommand(() => {
                    if (Car == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego pojazdu!");
                        return;
                    }
                    var editWindow = new AddEditWindow { Title = "Edytuj pojazd" };
                    Messenger.Default.Send(new AddEditPageMessage {
                        Page = ViewModelLocator.Instance.AddEditCar,
                    });
                    Messenger.Default.Send(new AddEditEntityMessage<Car> {
                        Entity = _car
                    });
                    editWindow.ShowDialog();
                }));
            }
        }

        public RelayCommand DeleteCar {
            get {
                return _deleteCar ?? (_deleteCar = new RelayCommand(async () => {
                    if(_car == null) {
                        ShowDialog("Błąd", "Nie wybrano żadnego pojazdu!");
                    } else {
                        if(await ConfirmationDialog("Potwierdź", "Czy jesteś pewien, że chcesz usunąć dany pojazd?"))
                            using(var repository = new BaseRepository()) {
                                repository.Delete(_car);
                            }
                    }
                    FillCars(_filter);
                }));
            }
        }

        public RelayCommand RefreshCars {
            get {
                return _refreshCars ?? (_refreshCars = new RelayCommand(() => {
                    Car = null;
                    FillCars(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterCars {
            get {
                return _filterCars ?? (_filterCars = new RelayCommand(() => {
                    if(_filter.Equals(_prevFilter))
                        return;

                    FillCars(_filter);
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

        public bool ActualInspection {
            get { return _actualInspection; }
            set {
                _actualInspection = value;
                RaisePropertyChanged();
            }
        }

        public bool ActualInsurance {
            get { return _actualInsurance; }
            set {
                _actualInsurance = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCarsLoading {
            get { return _isCarsLoading; }
            set {
                _isCarsLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsClassesDatesLoading {
            get { return _isClassesDatesLoading; }
            set {
                _isClassesDatesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExamsDatesLoading {
            get { return _isExamsDatesLoading; }
            set {
                _isExamsDatesLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAll {
            get { return _isAll; }
            set {
                _isAll = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLocked {
            get { return _isLocked; }
            set {
                _isLocked = value;
                RaisePropertyChanged();
            }
        }
        public bool IsUnlocked {
            get { return _isUnlocked; }
            set {
                _isUnlocked = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Helpers
        private async void FillCars(string filter) {
            IsCarsLoading = true;

            await Task.Run(() => {
                if (string.IsNullOrEmpty(filter)) {
                    using (var repository = new BaseRepository()) {
                        Cars =
                            new ObservableCollection<Car>(
                                repository.ToList<Car>().OrderBy(car => car.Brand));
                    }
                } else {
                    if (filter.Contains(" ")) {
                        var filters = filter.Split(' ');

                        if (!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                            using (var repository = new BaseRepository()) {
                                Cars =
                                    new ObservableCollection<Car>(
                                        repository.ToList<Car>()
                                            .Where(
                                                x =>
                                                    x.Brand.Contains(filters[0]) && x.Model.Contains(filters[1]) ||
                                                    x.Brand.Contains(filters[1]) && x.Model.Contains(filters[0]))
                                            .OrderBy(car => car.Brand));
                            }
                        } else {
                            using (var repository = new BaseRepository()) {
                                Cars =
                                    new ObservableCollection<Car>(
                                        repository.ToList<Car>()
                                            .Where(x => x.Brand.Contains(filter) || x.Model.Contains(filter))
                                            .OrderBy(car => car.Brand));
                            }
                        }
                    } else {
                        using (var repository = new BaseRepository()) {
                            Cars =
                                new ObservableCollection<Car>(
                                    repository.ToList<Car>()
                                        .Where(x => x.Brand.Contains(filter) || x.Model.Contains(filter))
                                        .OrderBy(car => car.Brand));
                        }
                    }
                }

                if (_isLocked)
                    Cars = new ObservableCollection<Car>(Cars.Where(x => x.InspectionDate < DateTime.Now || x.InsuranceDate < DateTime.Now));
                else if(_isUnlocked)
                    Cars = new ObservableCollection<Car>(Cars.Where(x => x.InspectionDate >= DateTime.Now && x.InsuranceDate >= DateTime.Now));
            });

            IsCarsLoading = false;
        }

        private async void FillClassesDates(Car car) {
            IsClassesDatesLoading = true;

            await Task.Run(() => {
                using (BaseRepository repository = new BaseRepository()) {
                    ClassesDates =
                        new ObservableCollection<ClassesDates>(
                            repository.ToList<ClassesDates>().Where(x => x.Car == car && x.Car != null).ToList());
                }
            });

            IsClassesDatesLoading = false;
        }

        private async void FillExamsDates(Car car) {
            IsExamsDatesLoading = true;

            await Task.Run(() => {
                using (BaseRepository repository = new BaseRepository()) {
                    ExamsDates =
                        new ObservableCollection<ExamsDates>(
                            repository.ToList<ExamsDates>().Where(x => x.Car == car && x.Car != null).ToList());
                }
            });

            IsExamsDatesLoading = false;
        }

        private async void ResolveCheckboxes() {
            await Task.Run(() => {
                if (_car == null) {
                    ActualInspection = false;
                    ActualInsurance = false;

                    return;
                }

                // ActualInspection checkbox
                ActualInspection = _car.InspectionDate >= DateTime.Now;

                // ActualInsurance checkbox
                ActualInsurance = _car.InsuranceDate >= DateTime.Now;
            });
        }
        #endregion

        #endregion
    }
}