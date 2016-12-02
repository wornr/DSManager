using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class ExamsDatesMap : ClassMap<ExamsDates> {
        public ExamsDatesMap() {
            Id(x => x.Id);
            Map(x => x.CourseKind).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.IsPassed).Nullable();

            References(x => x.Participant).Not.Nullable();
            References(x => x.Instructor).Not.Nullable();
            References(x => x.Car);
        }
    }
}
