using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages {
    public class CarsViewModel : BaseViewModel {
        public CarsViewModel() {
            FillCars(_filter);
        }

        private Car _car;
        private ObservableCollection<Car> _cars;
        private ObservableCollection<ClassesDates> _classesDates;
        private ObservableCollection<ExamsDates> _examsDates;
        private RelayCommand _filterCars;
        private RelayCommand _deleteCar;
        private RelayCommand _refreshCars;
        private string _filter;
        private string _prevFilter;

        public string Filter {
            get { return _filter; }
            set {
                if(_filter == value)
                    return;
                _filter = value;
                RaisePropertyChanged();
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

        public Car Car {
            get { return _car; }
            set {
                if(_car == value)
                    return;
                _car = value;
                FillClassesDates(value);
                FillExamsDates(value);
                RaisePropertyChanged();
            }
        }

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
    }
}