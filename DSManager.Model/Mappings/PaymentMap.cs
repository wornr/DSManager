using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class PaymentMap : ClassMap<Payment> {
        public PaymentMap() {
            Id(x => x.Id);
            Map(x => x.Amount);
            Map(x => x.PaymentNr);
            Map(x => x.Date);

            References(x => x.Participant);
        }
    }
}
