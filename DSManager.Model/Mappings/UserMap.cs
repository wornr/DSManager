using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class UserMap : ClassMap<User> {
        public UserMap() {
            Id(x => x.Id);
            Map(x => x.Login).Not.Nullable().Length(25);
            Map(x => x.Password).Not.Nullable().Length(32);
            Map(x => x.Email).Nullable().Length(100);
            Map(x => x.FirstName).Nullable().Length(25);
            Map(x => x.LastName).Nullable().Length(50);
            Map(x => x.AccountType).Not.Nullable();
            Map(x => x.Active).Not.Nullable();

            References(x => x.Instructor).Unique().Nullable();
            References(x => x.Student).Unique().Nullable();
        }
    }
}
