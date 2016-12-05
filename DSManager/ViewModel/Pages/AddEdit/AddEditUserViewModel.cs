using System.ComponentModel;

using DSManager.Model.Entities;

namespace DSManager.ViewModel.Pages.AddEdit {
    public class AddEditUserViewModel : AddEditBaseViewModel, IDataErrorInfo {
        public override bool Save() {
            throw new System.NotImplementedException();
        }

        public override bool Validate() {
            throw new System.NotImplementedException();
        }

        public string this[string columnName] {
            get { throw new System.NotImplementedException(); }
        }

        public string Error { get; }
    }
}