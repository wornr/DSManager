using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            References(x => x.Student);
            References(x => x.Instructor);
            HasMany(x => x.Payments);
            References(x => x.Course);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
