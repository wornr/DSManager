using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class InstructorPermissionsMap : ClassMap<InstructorPermissions> {
        public InstructorPermissionsMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();

            References(x => x.Instructor).Nullable();
        }
    }
}
