using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class InstructorMap : ClassMap<Instructor> {
        public InstructorMap() {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.SecondName);
            Map(x => x.LastName);
            Map(x => x.PESEL);
            Map(x => x.BirthDate);
            Map(x => x.City);
            Map(x => x.Street);
            Map(x => x.HouseNr);
            Map(x => x.ApartmentNr);
            Map(x => x.PhoneNr);
            Map(x => x.PermissionsNr);

            HasMany(x => x.Participants);
            HasMany(x => x.Permissions);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
