﻿using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class StudentMap : ClassMap<Student> {
        public StudentMap() {
            Id(x => x.Id);
            Map(x => x.FirstName).Not.Nullable().Length(25);
            Map(x => x.SecondName).Nullable().Length(25);
            Map(x => x.LastName).Not.Nullable().Length(50);
            Map(x => x.PESEL).Nullable().Length(11);
            Map(x => x.BirthDate).Not.Nullable();
            Map(x => x.City).Not.Nullable().Length(50);
            Map(x => x.PostalCode).Nullable().Length(6);
            Map(x => x.Street).Not.Nullable().Length(50);
            Map(x => x.HouseNr).Not.Nullable().Length(10);
            Map(x => x.ApartmentNr).Nullable().Length(10);
            Map(x => x.PhoneNr).Nullable().Length(20);
            Map(x => x.Email).Nullable().Length(100);

            HasOne(x => x.DrivingLicense).Cascade.All();
            HasOne(x => x.User).PropertyRef(x => x.Student);
            HasMany(x => x.Participants).Cascade.All();
        }
    }
}
