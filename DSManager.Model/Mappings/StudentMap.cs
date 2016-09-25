using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class StudentMap : ClassMap<Student> {
        public StudentMap() {
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

            References(x => x.DrivingLicense);
            HasMany(x => x.Participants);
        }
    }
}
