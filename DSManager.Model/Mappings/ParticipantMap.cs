using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class ParticipantMap : ClassMap<Participant> {
        public ParticipantMap() {
            Id(x => x.Id);
            Map(x => x.KEOSNr).Not.Nullable();
            Map(x => x.PKKNr).Not.Nullable();
            Map(x => x.CoursePrice).Not.Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.CertificateNr).Nullable();
            Map(x => x.IsTheory).Not.Nullable();

            References(x => x.Student).Not.Nullable();
            References(x => x.Instructor).Not.Nullable();
            References(x => x.Course).Nullable();
            HasMany(x => x.Payments).Cascade.All();
            HasMany(x => x.ClassesDates).Cascade.All();
            HasMany(x => x.ExamsDates).Cascade.All();
        }
    }
}
