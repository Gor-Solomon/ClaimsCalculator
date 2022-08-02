using DomainDrivenDesign.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WillisTW.Claims.Domain.Entities;
using WillisTW.Claims.Domain.ValueObjects;

namespace WillisTW.Claims.Domain.AggregateRoots
{
    public class ClaimsAccumulator : Aggregate<ClaimsAccumulator>
    {
        #region Fields
        private readonly SortedSet<Triangle> _triangles;
        #endregion

        #region Constructors
        public ClaimsAccumulator(Id<ClaimsAccumulator> id, IEnumerable<string> typelessClaims) : base(id)
        {
            _triangles = ConstructTriangles(typelessClaims);
        }
        #endregion

        #region Behaviours
        public string GenerateAccumulatedClaims()
        {
            StringBuilder result = new StringBuilder();
            Dictionary<string, List<decimal>> accumulatedTriangles = new Dictionary<string, List<decimal>>();

            foreach (Triangle triangle in _triangles)
            {
                List<decimal> triangleIncrementalPayments = triangle.CalculateIncrementalPayments();
                accumulatedTriangles.Add(triangle.Id.ToString(), triangleIncrementalPayments);
            }

            result.AppendLine(_triangles.First().EarliestOriginYear + ", " + _triangles.First().DevelopmentYears);
            result.Append(GenerateTriangleLines(accumulatedTriangles));

            return result.ToString();
        }

        private StringBuilder GenerateTriangleLines(Dictionary<string, List<decimal>> accumulatedTriangles)
        {
            StringBuilder tiangleLines = new StringBuilder();
            int maxPayments = accumulatedTriangles.Values.Max(x => x.Count);

            foreach (KeyValuePair<string, List<decimal>> accumulatedTriangle in accumulatedTriangles)
            {
                List<decimal> accumulatedPayments = accumulatedTriangle.Value;
                List<decimal> redundantZeros = Enumerable.Repeat(0m, maxPayments - accumulatedPayments.Count).ToList();
                accumulatedPayments.InsertRange(0, redundantZeros);
                string accumulatedPaymentsSection = string.Join(", ", accumulatedPayments.Select(x => x.ToString("0.####")));
                tiangleLines.AppendLine(string.Join(", ", accumulatedTriangle.Key, accumulatedPaymentsSection));
            }

            return tiangleLines;
        }
        #endregion

        #region Private Logic
        private SortedSet<Triangle> ConstructTriangles(IEnumerable<string> typelessClaims)
        {
            SortedSet<Triangle> triangles = new SortedSet<Triangle>();
            Dictionary<string, SortedSet<Claim>> unconstructedTriangles = new Dictionary<string, SortedSet<Claim>>();

            int line = 1;

            foreach (string typelessClaim in typelessClaims)
            {
                string[] typlessClaimFields = typelessClaim.Split(',');

                if (typlessClaimFields.Length < 3 || typlessClaimFields.Length > 4)
                {
                    throw new FormatException($"Error at line {line}: there should be 3 or 4 fileds, (Product, Origin Year, Development Year, payment) the payment field is optional");
                }

                string typlessProductName = typlessClaimFields.First();
                string typlessOriginYear = typlessClaimFields[1];
                string typlessDevelopmentYear = typlessClaimFields[2];
                string typlessPayment = "0";

                if (typlessClaimFields.Length > 3)
                {
                    typlessPayment = typlessClaimFields[3];
                }

                Claim claim = Claim.Create(typlessPayment, typlessOriginYear, typlessDevelopmentYear);

                if (unconstructedTriangles.ContainsKey(typlessProductName))
                {
                    unconstructedTriangles[typlessProductName].Add(claim);
                }
                else
                {
                    unconstructedTriangles.Add(typlessProductName, new SortedSet<Claim>() { claim });
                }

                line++;
            }

            foreach (KeyValuePair<string, SortedSet<Claim>> unconstructedTriangle in unconstructedTriangles)
            {
                Triangle triangle = new Triangle(unconstructedTriangle.Key, unconstructedTriangle.Value);
                triangles.Add(triangle);
            }

            return triangles;
        }
        #endregion
    }
}
