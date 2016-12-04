using System;
using DSManager.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace DSManager.View.Windows {
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : IClosable {
        public AddEditWindow() {
            InitializeComponent();
        }

        async void IClosable.ShowDialog(string title, string description) {
            await this.ShowMessageAsync(title, description);
        }
    }
}
