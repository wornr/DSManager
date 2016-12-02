using GalaSoft.MvvmLight;

namespace DSManager.ViewModel {
    public abstract class BaseViewModel : ViewModelBase {
        private bool _isLoading;
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel {
            get {
                return _currentViewModel;
            }
            set {
                if(_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoading {
            get { return _isLoading; }
            set {
                if(_isLoading == value)
                    return;
                _isLoading = value;
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