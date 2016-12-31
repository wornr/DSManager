using System;
using System.Windows;

using DSManager.ViewModel;

namespace DSManager.View.Windows {
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow {
        public SignInWindow() {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);

            if (BaseViewModel.Locked)
                Application.Current.Shutdown();
        }
    }
}
