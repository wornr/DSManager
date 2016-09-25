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
            Map(x => x.KEOSNr);
            Map(x => x.PKKNr);
            Map(x => x.CoursePrice);
            Map(x => x.EndDate);
            Map(x => x.CertificateNr);
            Map(x => x.IsTheory);

            References(x => x.Student);
            References(x => x.Instructor);
            HasMany(x => x.Payments);
            References(x => x.Course);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
