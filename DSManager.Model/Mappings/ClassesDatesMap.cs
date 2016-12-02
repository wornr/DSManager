using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class ClassesDatesMap : ClassMap<ClassesDates> {
        public ClassesDatesMap() {
            Id(x => x.Id);
            Map(x => x.CourseKind).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.Distance).Nullable();

            References(x => x.Participant).Not.Nullable();
            References(x => x.Instructor).Not.Nullable();
            References(x => x.Car);
        }
    }
}
