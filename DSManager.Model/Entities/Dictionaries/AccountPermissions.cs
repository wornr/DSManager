using DSManager.Model.Enums;
// ReSharper disable RedundantOverriddenMember
// ReSharper disable BaseObjectEqualsIsObjectEquals
// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode

namespace DSManager.Model.Entities.Dictionaries {
    public class AccountPermissions {
        #region Composite Key
        public virtual AccountType AccountType { get; set; }
        public virtual string Permission { get; set; }
        #endregion

        public virtual string Description { get; set; }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
