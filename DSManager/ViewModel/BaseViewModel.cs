using System.Windows;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using DSManager.Model.Entities;
using DSManager.View.Windows;

namespace DSManager.ViewModel {
    public abstract class BaseViewModel : ViewModelBase {
        private static MetroWindow _windowsInstance = Application.Current.MainWindow as MetroWindow;
        private bool _isLoading;
        private ViewModelBase _currentViewModel;
        private static User _signedUser;
        public static bool Locked { get; set; }

        public ViewModelBase CurrentViewModel {
            get { return _currentViewModel; }
            set {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoading {
            get { return _isLoading; }
            set {
                if (_isLoading == value)
                    return;
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        public MetroWindow MainWindow {
            get { return _windowsInstance; }
            set {
                if (value is MainWindow)
                    _windowsInstance = value;
            }
        }

        public User SignedUser {
            get { return _signedUser; }
            set {
                _signedUser = value;
                RaisePropertyChanged();
            }
        }

        public virtual void OnLoad() {
            CurrentViewModel = null;
        }

        public virtual void NavigateTo(ViewModelBase viewModel) {
            CurrentViewModel = viewModel;
            var dsManagerViewModel = viewModel as BaseViewModel;
            dsManagerViewModel?.OnLoad();
        }

        public async void ShowDialog(string title, string description) {
            await _windowsInstance.ShowMessageAsync(title, description);
        }

        public async Task<bool> ConfirmationDialog(string title, string description) {
            MetroDialogSettings settings = new MetroDialogSettings() {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult result = await _windowsInstance.ShowMessageAsync(title, description, MessageDialogStyle.AffirmativeAndNegative, settings);
            return result == MessageDialogResult.Affirmative;
        }

        //public void RunAsyncOperation(Action toExecute, Action<bool> executeUponFinish) {
        //    var taskScheduler1 = TaskScheduler.Default;

        //    var task = Task.Factory.StartNew(toExecute, CancellationToken.None, TaskCreationOptions.LongRunning,
        //        taskScheduler1);

        //    task.ContinueWith(finished =>
        //    {
        //        if(!finished.IsFaulted) {
        //            executeUponFinish(true);
        //            return;
        //        }
        //    });
        //}
    }
}