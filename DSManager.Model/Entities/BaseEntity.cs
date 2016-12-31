namespace DSManager.Model.Entities {
    public class BaseEntity<T> where T : BaseEntity<T> {
        public virtual int Id { get; set; }

        public override bool Equals(object obj) {
            var other = obj as T;
            if(other == null)
                return false;

            var otherIsTransient = Equals(other.Id, 0);
            var thisIsTransient = Equals(Id, 0);
            if(otherIsTransient && thisIsTransient)
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }

        private int? _oldHashCode;

        public override int GetHashCode() {
            if(_oldHashCode.HasValue)
                return _oldHashCode.Value;

            var thisIsTransient = Equals(Id, 0);

            if(thisIsTransient) {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity<T> x, BaseEntity<T> y) {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity<T> x, BaseEntity<T> y) {
            return !(x == y);
        }
    }
}
