using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class PaymentMap : ClassMap<Payment> {
        public PaymentMap() {
            Id(x => x.Id);
            Map(x => x.Amount).Not.Nullable();
            Map(x => x.PaymentNr).Not.Nullable();
            Map(x => x.Date).Not.Nullable();

            References(x => x.Participant).Not.Nullable();
        }
    }
}
