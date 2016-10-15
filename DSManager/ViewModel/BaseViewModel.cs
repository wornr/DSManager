using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using DSManager.Model.Entities;

namespace DSManager.ViewModel {
    public abstract class BaseViewModel : ViewModelBase {
        private bool _isLoading;
        private ViewModelBase _currentViewModel;

        protected BaseViewModel() {
        }

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
            this.CurrentViewModel = viewModel;
            var DSManagerViewModel = viewModel as BaseViewModel;
            if(DSManagerViewModel != null) {
                DSManagerViewModel.OnLoad();
            }
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

        public User GetUser(int id) {
            User user = new User();
            return user;
        }
    }
}