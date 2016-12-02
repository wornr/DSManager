using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        #endregion

        #region View Elements
        private string _filter;
        private bool _actualInspection;
        private bool _actualInsurance;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion

        public CarsViewModel() {
            FillCars(_filter);
        }

        #region Methods

        #region Selections
        public Car Car {
            get { return _car; }
            set {
                if(_car == value)
                    return;
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
        #endregion

        #region Commands
        public RelayCommand AddCar {
            get {
                return _addCar ?? (_addCar = new RelayCommand(() => {
                    var addWindow = new AddEditWindow { Title = "Dodaj pojazd" };
                    Messenger.Default.Send(new AddEditMessage {
                        Page = ViewModelLocator.Instance.AddEditCar,
                        Entity = null
                    });
                    addWindow.Show();
                }));
            }
        }

        public RelayCommand EditCar {
            get {
                return _editCar ?? (_editCar = new RelayCommand(() => {
                    if(Car == null)
                        // TODO wyrzucić komunikat "Nie wybrano żadnego pojazdu"
                        return;
                    var editWindow = new AddEditWindow { Title = "Edytuj pojazd" };
                    Messenger.Default.Send(new AddEditMessage {
                        Page = ViewModelLocator.Instance.AddEditCar,
                        Entity = _car
                    });
                    editWindow.Show();
                }));
            }
        }

        public RelayCommand DeleteCar {
            get {
                return _deleteCar ?? (_deleteCar = new RelayCommand(() => {
                    if(_car == null) {
                        // TODO wyrzucić komunikat "Nie wybrano żadnego pojazdu"
                    } else {
                        // TODO wyrzucić dialog z zapytaniem "Czy jesteś pewien, że chcesz usunąć dany pojazd?"
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
                    FillCars(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterCars {
            get {
                return _filterCars ?? (_filterCars = new RelayCommand(() => {
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
        #endregion

        #region Helpers
        private void FillCars(string filter) {
            if(string.IsNullOrEmpty(filter)) {
                using(var repository = new BaseRepository()) {
                    Cars =
                        new ObservableCollection<Car>(
                            repository.ToList<Car>().OrderBy(car => car.Brand).ToList());
                }
            } else {
                if(filter.Contains(" ")) {
                    var filters = filter.Split(' ');

                    if(!string.IsNullOrEmpty(filters[0]) && !string.IsNullOrEmpty(filters[1])) {
                        using(var repository = new BaseRepository()) {
                            Cars =
                                new ObservableCollection<Car>(
                                    repository.ToList<Car>()
                                        .Where(x => x.Brand.Contains(filters[0]) && x.Model.Contains(filters[1]) || x.Brand.Contains(filters[1]) && x.Model.Contains(filters[0]))
                                        .OrderBy(car => car.Brand)
                                        .ToList());
                        }
                    } else {
                        using(var repository = new BaseRepository()) {
                            Cars =
                                new ObservableCollection<Car>(
                                    repository.ToList<Car>()
                                        .Where(x => x.Brand.Contains(filter) || x.Model.Contains(filter))
                                        .OrderBy(car => car.Brand)
                                        .ToList());
                        }
                    }
                } else {
                    using(var repository = new BaseRepository()) {
                        Cars =
                            new ObservableCollection<Car>(
                                repository.ToList<Car>()
                                    .Where(x => x.Brand.Contains(filter) || x.Model.Contains(filter))
                                    .OrderBy(car => car.Brand)
                                    .ToList());
                    }
                }
            }
        }

        private void FillClassesDates(Car car) {
            using(BaseRepository repository = new BaseRepository()) {
                ClassesDates = new ObservableCollection<ClassesDates>(repository.ToList<ClassesDates>().Where(x => x.Car == car && x.Car != null).ToList());
            }
        }

        private void FillExamsDates(Car car) {
            using(BaseRepository repository = new BaseRepository()) {
                ExamsDates = new ObservableCollection<ExamsDates>(repository.ToList<ExamsDates>().Where(x => x.Car == car && x.Car != null).ToList());
            }
        }

        private void ResolveCheckboxes() {
            if(_car == null) {
                ActualInspection = false;
                ActualInsurance = false;

                return;
            }

            // ActualInspection checkbox
            ActualInspection = _car.InspectionDate >= DateTime.Now;

            // ActualInsurance checkbox
            ActualInsurance = _car.InsuranceDate >= DateTime.Now;
        }
        #endregion

        #endregion
    }
}