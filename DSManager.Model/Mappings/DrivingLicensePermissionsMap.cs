using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class DrivingLicensePermissionsMap : ClassMap<DrivingLicensePermissions> {
        public DrivingLicensePermissionsMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();

            References(x => x.DrivingLicense).Nullable();
        }
    }
}
