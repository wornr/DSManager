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
        private ObservableCollection<string> _incomeYears;
        private ObservableCollection<string> _carExploitationYears;
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

        #region InstructorPassRate
        private SeriesCollection _instructorPassRateCollection;
        private string[] _instructorPassRateLabels;
        private bool _isInstructorPassRateEmpty;
        #endregion

        #endregion

        #region Commands
        private RelayCommand _refreshIncome;
        private RelayCommand _filterIncomeYear;
        private RelayCommand _refreshCarExploitation;
        private RelayCommand _filterCarExploitationYear;
        private RelayCommand _refreshInstructorPassRate;
        #endregion

        #region View Elements
        private string _incomeYearFilter;
        private string _carExploitationYearFilter;
        #endregion

        #region Helpers
        private string _prevIncomeYearFilter;
        private string _prevCarExploitationYearFilter;
        #endregion

        #endregion

        public StatisticsViewModel() {
            InitializeEmptyFilters();
            FillIncomeYears();
            FillIncome(_incomeYearFilter);
            FillCars();
            FillCarExploitationYears();
            FillCarExploitation(_carExploitationYearFilter, _car);
            FillInstructorPassRate();
        }

        #region Methods

        #region Commands
        public RelayCommand RefreshIncome {
            get {
                return _refreshIncome ?? (_refreshIncome = new RelayCommand(() => {
                    FillIncome(_prevIncomeYearFilter);
                }));
            }
        }

        public RelayCommand FilterYearIncome {
            get {
                return _filterIncomeYear ?? (_filterIncomeYear = new RelayCommand(() => {
                    if(_incomeYearFilter.Equals(_prevIncomeYearFilter))
                        return;

                    FillIncome(_incomeYearFilter);
                    _prevIncomeYearFilter = _incomeYearFilter;
                }));
            }
        }

        public RelayCommand RefreshCarExploitation {
            get {
                return _refreshCarExploitation ?? (_refreshCarExploitation = new RelayCommand(() => {
                    FillCars();
                    FillCarExploitation(_prevCarExploitationYearFilter, _prevCar);
                }));
            }
        }

        public RelayCommand FilterYearCarExploitation {
            get {
                return _filterCarExploitationYear ?? (_filterCarExploitationYear = new RelayCommand(() => {
                    if(_carExploitationYearFilter.Equals(_prevCarExploitationYearFilter) && _car == _prevCar)
                        return;

                    FillCarExploitation(_carExploitationYearFilter, _car);
                    _prevCarExploitationYearFilter = _carExploitationYearFilter;
                    _prevCar = _car;
                }));
            }
        }

        public RelayCommand RefreshInstructorPassRate => _refreshInstructorPassRate ?? (_refreshInstructorPassRate = new RelayCommand(FillInstructorPassRate));

        #endregion

        #region View Elements
        public string IncomeYearFilter {
            get { return _incomeYearFilter; }
            set {
                if (_incomeYearFilter == value)
                    return;
                _incomeYearFilter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> IncomeYearsFilter {
            get { return _incomeYears; }
            set {
                _incomeYears = value;
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

        public string CarExploitationYearFilter {
            get { return _carExploitationYearFilter; }
            set {
                if(_carExploitationYearFilter == value)
                    return;
                _carExploitationYearFilter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> CarExploitationYearsFilter {
            get { return _carExploitationYears; }
            set {
                _carExploitationYears = value;
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

        #region InstructorPassRate
        public SeriesCollection InstructorPassRateCollection {
            get { return _instructorPassRateCollection; }
            set {
                _instructorPassRateCollection = value;
                RaisePropertyChanged();
            }
        }
        public string[] InstructorPassRateLabels {
            get { return _instructorPassRateLabels; }
            set {
                _instructorPassRateLabels = value;
                RaisePropertyChanged();
            }
        }
        public Func<double, string> InstructorPassRateFormatter { get; set; }

        public bool IsInstructorPassRateEmpty {
            get { return _isInstructorPassRateEmpty; }
            set {
                _isInstructorPassRateEmpty = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #endregion

        #region Helpers

        private void InitializeEmptyFilters() {
            _incomeYearFilter =
                _prevIncomeYearFilter =
                _carExploitationYearFilter =
                _prevCarExploitationYearFilter =
                    DateTime.Now.Year.ToString();
            _car = _prevCar = null;
        }

        private void FillIncomeYears() {
            using (var repository = new BaseRepository()) {
                IncomeYearsFilter = new ObservableCollection<string>(repository.ToList<Payment>().GroupBy(g => g.Date.Year).Select(s => s.First().Date.Year.ToString()));
            }
        }

        private void FillIncome(string filter) {
            var firstDayOfYear = new DateTime(int.Parse(filter), 1, 1, 0, 0, 0);
            var lastDayOfYear = new DateTime(int.Parse(filter), 12, 31, 23, 59, 59);

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

        private void FillCarExploitationYears() {
            using(var repository = new BaseRepository()) {
                CarExploitationYearsFilter = new ObservableCollection<string>(repository.ToList<ClassesDates>().Where(x => x.Car != null && x.Distance != null).GroupBy(g => g.StartDate.Year).Select(s => s.First().StartDate.Year.ToString()));
            }
        }

        private void FillCarExploitation(string filter, Car car) {
            var firstDayOfYear = new DateTime(int.Parse(filter), 1, 1, 0, 0, 0);
            var lastDayOfYear = new DateTime(int.Parse(filter), 12, 31, 23, 59, 59);

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

        private void FillInstructorPassRate() {
            var columnSeries = new ColumnSeries {
                Title = "Zdawalność",
                Values = new ChartValues<decimal>()
            };
            var label = new List<string>();

            using(var repository = new BaseRepository()) {
                repository.ToList<ExamsDates>()
                    .Where(x => x.IsPassed == true)
                    .GroupBy(g => g.Instructor).Select(s => new {
                        countPassed = s.Count(),
                        instructor = s.First().Instructor
                        //date = s.First().StartDate
                    }).ForEach(x => {
                        repository.ToList<ExamsDates>()
                        .Where(y => y.IsPassed != null)
                            .GroupBy(g => g.Instructor).Select(s => new {
                                count = s.Count(),
                            }).ForEach(y => {
                                label.Add(x.instructor.FirstName + " " + x.instructor.LastName);
                                columnSeries.Values.Add(decimal.Multiply(decimal.Divide(x.countPassed, y.count), 100));
                            });
                    });
            }

            InstructorPassRateCollection = new SeriesCollection { columnSeries };
            InstructorPassRateLabels = new string[label.Count];
            var i = 0;
            label.ForEach(x => InstructorPassRateLabels[i++] = x);
            InstructorPassRateFormatter = value => value + " %";

            IsInstructorPassRateEmpty = InstructorPassRateCollection[0].Values.Count == 0;
        }
        #endregion

        #endregion
    }
}
