﻿using System;

namespace DSManager.Model.Entities {
    public class Person<T> : BaseEntity<T> where T : BaseEntity<T> {
        public virtual string FirstName { get; set; }
        public virtual string SecondName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PESEL { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Street { get; set; }
        public virtual string HouseNr { get; set; }
        public virtual string ApartmentNr { get; set; }
        public virtual string PhoneNr { get; set; }
        public virtual string Email { get; set; }
    }
}
