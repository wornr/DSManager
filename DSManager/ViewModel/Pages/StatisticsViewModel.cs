using System;
using System.Collections.Generic;
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

        #region Collections

        #region Income
        private SeriesCollection _incomeCollection;
        private string[] _incomeLabels;
        private bool _isIncomeEmpty;
        #endregion

        #endregion

        #region Commands
        private RelayCommand _refreshIncome;
        private RelayCommand _filterIncome;
        #endregion

        #region View Elements
        private string _filter;
        #endregion

        #region Helpers
        private string _prevFilter;
        #endregion

        #endregion

        public StatisticsViewModel() {
            _filter = _prevFilter = string.Empty;
            FillIncome(_filter);
        }

        #region Methods

        #region Commands
        public RelayCommand RefreshIncome {
            get {
                return _refreshIncome ?? (_refreshIncome = new RelayCommand(() => {
                    FillIncome(_prevFilter);
                }));
            }
        }

        public RelayCommand FilterIncome {
            get {
                return _filterIncome ?? (_filterIncome = new RelayCommand(() => {
                    if (_filter.Equals(_prevFilter))
                        return;

                    FillIncome(_filter);
                    _prevFilter = _filter;
                }));
            }
        }
        #endregion

        #region View Elements
        public string Filter {
            get { return _filter; }
            set {
                if (_filter == value)
                    return;
                _filter = value;
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

        #endregion

        #endregion

        #region Helpers
        private void FillIncome(string filter) {
            var firstDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0) : new DateTime(int.Parse(filter), 1, 1, 0, 0, 0);
            var lastDayOfYear = string.IsNullOrEmpty(filter) ? new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59) : new DateTime(int.Parse(filter), 12, 31, 23, 59, 59);

            var columnSeries = new ColumnSeries {
                Title = firstDayOfYear.Year.ToString(),
                Values = new ChartValues<decimal>()
            };
            var label = new List<string>();

            using (var repository = new BaseRepository()) {
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

            IncomeCollection = new SeriesCollection {columnSeries};
            IncomeLabels = new string[label.Count];
            var i = 0;
            label.ForEach(x => IncomeLabels[i++] = x);
            IncomeFormatter = value => value + " zł";
            
            IsIncomeEmpty = IncomeCollection[0].Values.Count == 0;
        }
        #endregion

        #endregion
    }
}
