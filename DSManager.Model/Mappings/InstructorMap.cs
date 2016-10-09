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
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.SecondName).Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.PESEL).Nullable();
            Map(x => x.BirthDate).Not.Nullable();
            Map(x => x.City).Not.Nullable();
            Map(x => x.Street).Not.Nullable();
            Map(x => x.HouseNr).Not.Nullable();
            Map(x => x.ApartmentNr).Nullable();
            Map(x => x.PhoneNr).Nullable();
            Map(x => x.Email).Nullable().Length(100);
            Map(x => x.PermissionsNr).Not.Nullable();

            HasMany(x => x.Participants);
            HasMany(x => x.Permissions);
            HasMany(x => x.ClassesDates);
            HasMany(x => x.ExamsDates);
        }
    }
}
