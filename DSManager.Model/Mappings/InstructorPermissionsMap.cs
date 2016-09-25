using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class InstructorPermissionsMap : ClassMap<InstructorPermissions> {
        public InstructorPermissionsMap() {
            Id(x => x.Id);
            Map(x => x.Category);

            References(x => x.Instructor);
        }
    }
}
