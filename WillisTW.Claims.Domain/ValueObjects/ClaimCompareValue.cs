using System;

namespace WillisTW.Claims.Domain.ValueObjects
{
    /// <summary>
    /// This class was created because we can't make IncrementalTriangle class a direct comparable object and pass it to the base.
    /// if I had more time, I would develop my custom DDD base classes to do it the right way
    /// </summary>
    internal class ClaimCompareValue : IComparable<ClaimCompareValue>, IComparable
    {
        public ClaimCompareValue(DateTime originYear, DateTime developmentYear)
        {
            OriginYear = originYear;
            DevelopmentYear = developmentYear;
        }

        public DateTime OriginYear { get; }
        public DateTime DevelopmentYear { get; }

        public int CompareTo(ClaimCompareValue other)
        {
            int result = OriginYear.CompareTo(other.OriginYear);

            if (result == 0)
            {
                result = DevelopmentYear.CompareTo(other.DevelopmentYear);
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            ClaimCompareValue other = obj as ClaimCompareValue;

            if (other is null)
            {
                throw new NotSupportedException($"Cannot compare {obj} to {this.GetType()}");
            }

            return CompareTo(other);
        }
    }
}
