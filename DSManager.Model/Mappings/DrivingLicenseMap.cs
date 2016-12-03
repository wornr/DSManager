using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class DrivingLicenseMap : ClassMap<DrivingLicense> {
        public DrivingLicenseMap() {
            Id(x => x.Id);
            Map(x => x.IssueDate).Not.Nullable();
            Map(x => x.DrivingLicenseNr).Not.Nullable().Length(20);

            HasMany(x => x.DrivingLicensePermissions).Cascade.All();
            References(x => x.Student).Not.Nullable();
        }
    }
}
