using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class CourseMap : ClassMap<Course> {
        public CourseMap() {
            Id(x => x.Id);
            Map(x => x.Category).Not.Nullable();
            Map(x => x.CourseType).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();

            HasMany(x => x.Participants).Cascade.All();
        }
    }
}
