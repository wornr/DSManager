using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class DurationTimeMap : ClassMap<DurationTime> {
        public DurationTimeMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();
            Map(x => x.CourseKind).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.Time).Not.Nullable();
        }
    }
}
