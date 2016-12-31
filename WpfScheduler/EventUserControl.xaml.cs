using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfScheduler
{
    public partial class EventUserControl
    {
        public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent("Delete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EventUserControl));

        public EventUserControl(Event e, bool showTime)
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteExecuted));

            Event = e;

            VerticalAlignment = VerticalAlignment.Top;
            HorizontalAlignment = HorizontalAlignment.Left;
            Subject = e.Subject;
            BorderElement.Background = e.Color;
            if (!showTime || e.AllDay)
            {
                DisplayDateText.Visibility = Visibility.Hidden;
                DisplayDateText.Height = 0;
                DisplayDateText.Text = $"{e.Start.ToShortDateString()} - {e.End.ToShortDateString()}";
            }
            else
            {
                DisplayDateText.Text = $"{e.Start:HH:mm} - {e.End:HH:mm}";
            }
            BorderElement.ToolTip = DisplayDateText.Text + Environment.NewLine + DisplayText.Text;
        }

        public Event Event { get; }

        public Border EventElement => BorderElement;

        #region Subject
        public static readonly DependencyProperty SubjectProperty = 
            DependencyProperty.Register("Subject", typeof(string), typeof(EventUserControl),
            new FrameworkPropertyMetadata(AdjustSubject));

        public string Subject
        {
            get { return (string)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        private static void AdjustSubject(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EventUserControl).DisplayText.Text = (string)e.NewValue;
        }
        #endregion

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e) {
            RaiseDeleteEvent();
        }
        
        public event RoutedEventHandler Delete {
            add { AddHandler(DeleteEvent, value); }
            remove { RemoveHandler(DeleteEvent, value); }
        }

        protected virtual void RaiseDeleteEvent() {
            RoutedEventArgs args = new RoutedEventArgs(DeleteEvent);
            RaiseEvent(args);
        }
    }
}
