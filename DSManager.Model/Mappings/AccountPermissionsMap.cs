using FluentNHibernate.Mapping;

using DSManager.Model.Entities.Dictionaries;

namespace DSManager.Model.Mappings {
    class AccountPermissionsMap : ClassMap<AccountPermissions> {
        public AccountPermissionsMap() {
            CompositeId().KeyProperty(x => x.AccountType)
                .KeyProperty(x => x.Permission);
            Map(x => x.Description).Nullable();
        }
    }
}