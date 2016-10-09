using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class DrivingLicenseMap : ClassMap<DrivingLicense> {
        public DrivingLicenseMap() {
            Id(x => x.Id);
            Map(x => x.IssueDate).Not.Nullable();
            Map(x => x.DrivingLicenseNr).Not.Nullable();

            HasMany(x => x.DrivingLicensePermissions);
            References(x => x.Student);
        }
    }
}
