using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using LiveCharts;
using LiveCharts.Wpf;

using NHibernate.Util;

using DSManager.Model.Entities;
using DSManager.Model.Services;

namespace DSManager.ViewModel.Pages {
    public class StatisticsViewModel : BaseViewModel {
        #region Variables

        #region Selections
        private Car _car;
        private Car _prevCar;
        #endregion

        #region Lists
        private ObservableCollection<Car> _cars;
        #endregion

        #region Collections

        #region Income
        private SeriesCollection _incomeCollection;
        private string[] _incomeLabels;
        private bool _isIncomeEmpty;
        #endregion

        #region CarExploitation
        private SeriesCollection _carExploitationCollection;
        private string[] _carExploitationLabels;
        private bool _isCarExploitationEmpty;
        #endregion

        #endregion

        #region Commands
        private RelayCommand _refreshIncome;
        private RelayCommand _filterIncome;
        private RelayCommand _refreshCarExploitation;
        private RelayCommand _filterCarExploitation;
        #endregion

        #region View Elements
        private string _incomeFilter;
        private string _carExploitationFilter;
        #endregion

        #region Helpers
        private string _prevIncomeFilter;
        private string _prevCarExploitationFilter;
        #endregion

        #endregion

        public StatisticsViewModel() {
            InitializeEmptyFilters();
            FillIncome(_incomeFilter);
            FillCars();
            FillCarExploitation(_carExploitationFilter, _car);
        }

        #region Methods

        #region Commands
        public RelayCommand RefreshIncome {
            get {
                return _refreshIncome ?? (_refreshIncome = new RelayCommand(() => {
                    FillIncome(_prevIncomeFilter);
                }));
            }
        }

        public RelayCommand FilterIncome {
            get {
                return _filterIncome ?? (_filterIncome = new RelayCommand(() => {
                    if(_incomeFilter.Equals(_prevIncomeFilter))
                        return;

                    FillIncome(_incomeFilter);
                    _prevIncomeFilter = _incomeFilter;
                }));
            }
        }

        public RelayCommand RefreshCarExploitation {
            get {
                return _refreshCarExploitation ?? (_refreshCarExploitation = new RelayCommand(() => {
                    FillCars();
                    FillCarExploitation(_prevCarExploitationFilter, _prevCar);
                }));
            }
        }

        public RelayCommand FilterCarExploitation {
            get {
                return _filterCarExploitation ?? (_filterCarExploitation = new RelayCommand(() => {
                    if(_carExploitationFilter.Equals(_prevCarExploitationFilter) && _car == _prevCar)
                        return;

                    FillCarExploitation(_carExploitationFilter, _car);
                    _prevCarExploitationFilter = _carExploitationFilter;
                    _prevCar = _car;
                }));
            }
        }
        #endregion

        #region View Elements
        public string IncomeFilter {
            get { return _incomeFilter; }
            set {
                if (_incomeFilter == value)
                    return;
                _incomeFilter = value;
                RaisePropertyChanged();
            }
        }

        public Car Car {
            get { return _car; }
            set {
                _car = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Car> Cars {
            get { return _cars; }
            set {
                _cars = value;
                RaisePropertyChanged();
            }
        }

        public string CarExploitationFilter {
            get { return _carExploitationFilter; }
            set {
                if(_carExploitationFilter == value)
                    return;
                _carExploitationFilter = value;
                RaisePropertyChanged();
            }
        }

        #region Collections

        #region Income

        public SeriesCollection IncomeCollection {
            get { return _incomeCollection; }
            set {
                _incomeCollection = value;
                RaisePropertyChanged();
            }
        }
        public string[] IncomeLabels {
            get { return _incomeLabels; }
            set {
                _incomeLabels = value;
                RaisePropertyChanged();
            }
        }
        public Func<double, string> IncomeFormatter { get; set; }

        public bool IsIncomeEmpty {
            get { return _isIncomeEmpty; }
            set {
                _isIncomeEmpty = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CarExploitation

        public SeriesCollection CarExploitationCollection {
            get { return _carExploitationCollection; }
            set {
                _carExploitationCollection = value;
                RaisePropertyChanged();
            }
        }
        public string[] CarExploitationLabels {
            get { return _carExploitationLabels; }
            set {
                _carExploitationLabels = value;
                RaisePropertyChanged();
            }
        }
        public Func<double, string> CarExploitationFormatter { get; set; }

        public bool IsCarExploitationEmpty {
            get { return _isCarExploitationEmpty; }
            set {
                _isCarExploitationEmpty = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #endregion

        #region Helpers

        private void InitializeEmptyFilters() {
            _incomeFilter = _prevIncomeFilter = string.Empty;
            _carExploitationFilter = _prevCarExploitationFilter = string.Empty;
            _car = _prevCar = null;
        }

        private void FillIncome(string filter) {
            var firstDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0) : new DateTime(int.Parse(filter), 1, 1, 0, 0, 0);
            var lastDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59) : new DateTime(int.Parse(filter), 12, 31, 23, 59, 59);

            var columnSeries = new ColumnSeries {
                Title = firstDayOfYear.Year.ToString(),
                Values = new ChartValues<decimal>()
            };
            var label = new List<string>();

            using(var repository = new BaseRepository()) {
                repository.ToList<Payment>()
                    .Where(x => x.Date >= firstDayOfYear && x.Date <= lastDayOfYear)
                    .GroupBy(g => g.Date.Month).Select(s => new {
                        value = s.Sum(amount => amount.Amount),
                        date = s.First().Date
                    }).ForEach(x => {
                            label.Add(x.date.ToString("MMMM", CultureInfo.CurrentCulture));
                            columnSeries.Values.Add(x.value);
                        }
                    );
            }

            IncomeCollection = new SeriesCollection { columnSeries };
            IncomeLabels = new string[label.Count];
            var i = 0;
            label.ForEach(x => IncomeLabels[i++] = x);
            IncomeFormatter = value => value.ToString("C");

            IsIncomeEmpty = IncomeCollection[0].Values.Count == 0;
        }

        private void FillCars() {
            using (var repository = new BaseRepository()) {
                Cars = new ObservableCollection<Car>(repository.ToList<Car>());
            }
        }

        private void FillCarExploitation(string filter, Car car) {
            var firstDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0) : new DateTime(int.Parse(filter), 1, 1, 0, 0, 0);
            var lastDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59) : new DateTime(int.Parse(filter), 12, 31, 23, 59, 59);

            // TODO zmienić w przyszłości na wykres liniowy (obecnie framework nie działa poprawnie)
            var lineSeries = new ColumnSeries {
                Title = firstDayOfYear.Year.ToString(),
                Values = new ChartValues<decimal>()
            };
            var label = new List<string>();

            using(var repository = new BaseRepository()) {
                repository.ToList<ClassesDates>()
                    .Where(x => x.StartDate >= firstDayOfYear && x.StartDate <= lastDayOfYear && x.Car == car && x.Distance != null)
                    .GroupBy(g => g.StartDate.Month).Select(s => new {
                        value = s.Sum(distance => distance.Distance),
                        date = s.First().StartDate
                    }).ForEach(x => {
                            label.Add(x.date.ToString("MMMM", CultureInfo.CurrentCulture));
                            lineSeries.Values.Add(x.value);
                        }
                    );
            }

            CarExploitationCollection = new SeriesCollection { lineSeries };
            CarExploitationLabels = new string[label.Count];
            var i = 0;
            label.ForEach(x => CarExploitationLabels[i++] = x);
            CarExploitationFormatter = value => value + " km";

            IsCarExploitationEmpty = CarExploitationCollection[0].Values.Count == 0;
        }
        #endregion

        #endregion
    }
}
