using System;

namespace Features.Cards
{
    [Serializable]
    public class UnitCard
    {
        public string UnitKey;
        public CardTier Tier;

        public static bool operator ==(UnitCard a, UnitCard b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            if (ReferenceEquals(a, b))
            {
                return true;
            }

            return a.UnitKey == b.UnitKey && a.Tier == b.Tier;
        }

        public static bool operator !=(UnitCard a, UnitCard b)
        {
            return !(a == b);
        }

        protected bool Equals(UnitCard other)
        {
            return UnitKey == other.UnitKey && Tier == other.Tier;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((UnitCard) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UnitKey, (int) Tier);
        }
    }
}