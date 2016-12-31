using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfScheduler
{
    public partial class WeekScheduler
    {
        private Scheduler _scheduler;

        internal event EventHandler<Event> OnEventMouseEnter;
        internal event EventHandler<Event> OnEventMouseLeave;
        internal event EventHandler<Event> OnEventDoubleClick;
        internal event EventHandler<Event> OnEventDelete; 
        internal event EventHandler<DateTime> OnScheduleDoubleClick;

        #region FirstDay
        private DateTime _firstDay;
        internal DateTime FirstDay
        {
            get { return _firstDay; }
            set {
                while (value.DayOfWeek != DayOfWeek.Monday)
                    value = value.AddDays(-1);
                _firstDay = value;
                AdjustFirstDay(value);
            }
        }

        private void AdjustFirstDay(DateTime firstDay)
        {
            DayLabel1.Content = firstDay.ToString("dddd dd/MM");
            DayLabel2.Content = firstDay.AddDays(1).ToString("dddd dd/MM");
            DayLabel3.Content = firstDay.AddDays(2).ToString("dddd dd/MM");
            DayLabel4.Content = firstDay.AddDays(3).ToString("dddd dd/MM");
            DayLabel5.Content = firstDay.AddDays(4).ToString("dddd dd/MM");
            DayLabel6.Content = firstDay.AddDays(5).ToString("dddd dd/MM");
            DayLabel7.Content = firstDay.AddDays(6).ToString("dddd dd/MM");

            PaintAllEvents(null);
            PaintAllDayEvents();
        }
        #endregion

        public WeekScheduler()
        {
            InitializeComponent();

            EventsGrid.MouseDown += EventsGrid_MouseDown;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs ea)
        {
            DependencyObject ucParent = (sender as WeekScheduler).Parent;
            while (!(ucParent is Scheduler))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }
            _scheduler = ucParent as Scheduler;

            _scheduler.OnEventAdded += (s, e) =>
            {
                if (e.Start.Date == e.End.Date)
                    PaintAllEvents(e.Start);
                else
                    PaintAllDayEvents();
            };

            _scheduler.OnEventDeleted += (s, e) =>
            {
                if (e.Start.Date == e.End.Date)
                    PaintAllEvents(e.Start);
                else
                    PaintAllDayEvents();
            };

            _scheduler.OnEventsModified += (s, e) =>
            {
                PaintAllEvents(null);
                PaintAllDayEvents();
            };

            _scheduler.OnStartJourneyChanged += (s, t) =>
            {
                if (_scheduler.StartJourney.Hours == 0)
                    StartJourney.Visibility = Visibility.Hidden;
                else
                    Grid.SetRowSpan(StartJourney, _scheduler.StartJourney.Hours);
            };

            _scheduler.OnEndJourneyChanged += (s, t) =>
            {
                if (_scheduler.EndJourney.Hours == 0)
                    EndJourney.Visibility = Visibility.Hidden;
                else
                {
                    Grid.SetRow(EndJourney, _scheduler.EndJourney.Hours);
                    Grid.SetRowSpan(EndJourney, 24 - _scheduler.EndJourney.Hours);
                }
            };

            SizeChanged += WeekScheduler_SizeChanged;

            ResizeGrids(new Size(ActualWidth, ActualHeight));
            PaintAllEvents(null);
            PaintAllDayEvents();
            if (_scheduler.StartJourney.Hours != 0)
            {
                double hourHeight = EventsGrid.ActualHeight / 22;
                ScrollEventsViewer.ScrollToVerticalOffset(hourHeight * (_scheduler.StartJourney.Hours - 1));
            }

            if (_scheduler.StartJourney.Hours == 0)
                StartJourney.Visibility = Visibility.Hidden;
            else
                Grid.SetRowSpan(StartJourney, _scheduler.StartJourney.Hours);

            if (_scheduler.EndJourney.Hours == 0)
                EndJourney.Visibility = Visibility.Hidden;
            else
            {
                Grid.SetRow(EndJourney, _scheduler.EndJourney.Hours);
                Grid.SetRowSpan(EndJourney, 24 - _scheduler.EndJourney.Hours);
            }
        }

        private void WeekScheduler_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeGrids(e.NewSize);
            PaintAllEvents(null);
            PaintAllDayEvents();
        }

        private void EventsGrid_MouseDown(object sender, MouseButtonEventArgs e) {
            if(e.ClickCount >= 2) {
                var point = Mouse.GetPosition(EventsGrid);

                int row = 0;
                int col = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;

                // calc row mouse was over
                foreach(var rowDefinition in EventsGrid.RowDefinitions) {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if(accumulatedHeight >= point.Y)
                        break;
                    row++;
                }

                // calc col mouse was over
                foreach(var columnDefinition in EventsGrid.ColumnDefinitions) {
                    accumulatedWidth += columnDefinition.ActualWidth;
                    if(accumulatedWidth >= point.X)
                        break;
                    col++;
                }

                if (col != 0) {
                    OnScheduleDoubleClick(sender,
                        new DateTime(FirstDay.Year, FirstDay.Month, FirstDay.Day, row, 0, 0).AddDays(col - 1));
                }
            }
        }

        private void PaintAllEvents(DateTime? date)
        {
            if (_scheduler?.Events == null) return;

            IEnumerable<Event> eventList = _scheduler.Events.Where(ev => ev.Start.Date == ev.End.Date && !ev.AllDay).OrderBy(ev => ev.Start);

            if (date == null)
            {
                Column1.Children.Clear();
                Column2.Children.Clear();
                Column3.Children.Clear();
                Column4.Children.Clear();
                Column5.Children.Clear();
                Column6.Children.Clear();
                Column7.Children.Clear();
            }
            else
            {
                int numColumn = (int)date.Value.Date.Subtract(FirstDay.Date).TotalDays + 1;
                ((Canvas)FindName("Column" + numColumn)).Children.Clear();

                eventList = eventList.Where(ev => ev.Start.Date == date.Value.Date).OrderBy(ev => ev.Start);
            }

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in eventList)
            {
                int numColumn = (int)e.Start.Date.Subtract(FirstDay.Date).TotalDays + 1;
                if (numColumn >= 0 && numColumn < 7)
                {
                    Canvas sp = (Canvas)FindName("Column" + numColumn);
                    sp.Width = columnWidth;

                    double oneHourHeight = sp.ActualHeight / 22;

                    var concurrentEvents = _scheduler.Events.Where(e1 => ((e1.Start <= e.Start && e1.End > e.Start) ||
                                                                          (e1.Start > e.Start && e1.Start < e.End)) &&
                                                                         e1.End.Date == e1.Start.Date).OrderBy(ev => ev.Start);

                    double marginTop = oneHourHeight * (e.Start.Hour + e.Start.Minute / 60.0);
                    double width = columnWidth / concurrentEvents.Count();
                    double marginLeft = width * getIndex(e, concurrentEvents.ToList());

                    EventUserControl wEvent = new EventUserControl(e, true) {
                        Width = width,
                        Height = e.End.Subtract(e.Start).TotalHours*oneHourHeight,
                        Margin = new Thickness(marginLeft, marginTop, 0, 0)
                    };
                    wEvent.MouseEnter += (sender, ea) => {
                        ea.Handled = true;
                        OnEventMouseEnter(sender, wEvent.Event);
                    };
                    wEvent.MouseLeave += (sender, ea) => {
                        ea.Handled = true;
                        OnEventMouseLeave(sender, wEvent.Event);
                    };
                    wEvent.MouseDoubleClick += (sender, ea) =>
                    {
                        ea.Handled = true;
                        OnEventDoubleClick(sender, wEvent.Event);
                    };
                    wEvent.Delete += (sender, ea) => {
                        ea.Handled = true;
                        OnEventDelete(sender, wEvent.Event);
                    };

                    sp.Children.Add(wEvent);
                }
            }
        }

        private int getIndex(Event e, List<Event> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (e.Id == list[i].Id) return i;
            }
            return -1;
        }

        private void PaintAllDayEvents()
        {
            if (_scheduler?.Events == null) return;

            AllDayEvents.Children.Clear();

            double columnWidth = EventsGrid.ColumnDefinitions[1].Width.Value;

            foreach (Event e in _scheduler.Events.Where(ev => ev.End.Date > ev.Start.Date || ev.AllDay))
            {
                int numColumn = (int)e.Start.Date.Subtract(FirstDay.Date).TotalDays;
                int numEndColumn = (int)e.End.Date.Subtract(FirstDay.Date).TotalDays + 1;

                if (numColumn >= 7 || numEndColumn <= 0) continue;

                if (numColumn < 0) numColumn = 0;
                if (numEndColumn > 7) numEndColumn = 7;

                if ((numColumn >= 0 && numColumn < 7) || (numEndColumn >= 0 && numEndColumn < 7))
                {
                    double marginLeft = numColumn * columnWidth;

                    EventUserControl wEvent = new EventUserControl(e, false) {
                        Width = columnWidth*(numEndColumn - numColumn),
                        Margin = new Thickness(marginLeft, 0, 0, 0)
                    };
                    wEvent.MouseEnter += (sender, ea) => {
                        ea.Handled = true;
                        OnEventMouseEnter(sender, wEvent.Event);
                    };
                    wEvent.MouseLeave += (sender, ea) => {
                        ea.Handled = true;
                        OnEventMouseLeave(sender, wEvent.Event);
                    };
                    wEvent.MouseDoubleClick += (sender, ea) => {
                        ea.Handled = true;
                        OnEventDoubleClick(sender, wEvent.Event);
                    };
                    wEvent.Delete += (sender, ea) => {
                        ea.Handled = true;
                        OnEventDelete(sender, wEvent.Event);
                    };

                    AllDayEvents.Children.Add(wEvent);
                }
            }
        }

        private void ResizeGrids(Size newSize)
        {
            EventsGrid.Width = newSize.Width;
            EventsHeaderGrid.Width = newSize.Width;

            double columnWidth = (ActualWidth - EventsGrid.ColumnDefinitions[0].ActualWidth) / 7;
            for (int i = 1; i < EventsGrid.ColumnDefinitions.Count; i++)
            {
                EventsGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
                EventsHeaderGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth);
            }
        }
    }
}
