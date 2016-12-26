using FluentNHibernate.Mapping;

using DSManager.Model.Entities;

namespace DSManager.Model.Mappings {
    class PaymentMap : ClassMap<Payment> {
        public PaymentMap() {
            Id(x => x.Id);
            Map(x => x.Amount).Not.Nullable().Precision(2);
            Map(x => x.PaymentNr).Not.Nullable();
            Map(x => x.PaymentDate).Not.Nullable();

            References(x => x.Participant).Not.Nullable();
        }
    }
}
