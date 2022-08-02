using Ardalis.GuardClauses;
using DomainDrivenDesign.DomainObjects;

namespace WillisTW.Claims.Domain.Entities
{
    internal class TriangleId : Id<Triangle, string>
    {
        public static TriangleId Create(string product)
        {
            return new TriangleId(product);
        }

        private TriangleId(string product) : base(product)
        {
            Guard.Against.NullOrWhiteSpace(product, nameof(product));
        }
    }
}
