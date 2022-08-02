using Ardalis.GuardClauses;
using DomainDrivenDesign.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using WillisTW.Claims.Domain.ValueObjects;

namespace WillisTW.Claims.Domain.Entities
{
    internal class Triangle : Entity<Triangle, TriangleId>, IComparable<Triangle>, IComparable
    {
        #region Fileds
        private readonly SortedSet<Claim> _claims;
        #endregion

        #region Constructors
        public Triangle(string product, SortedSet<Claim> claims) : base(TriangleId.Create(product))
        {
            Guard.Against.NullOrEmpty(claims, nameof(claims));
            _claims = claims;
        }
        #endregion

        #region Properties
        public int EarliestOriginYear => _claims.First().DevelopmentYear.Year;
        public int DevelopmentYears => _claims.Last().DevelopmentYear.Year - _claims.First().OriginYear.Year + 1;
        #endregion

        #region Behaviours
        /// <summary>
        /// There should be a check for overflow case, but due to time short time, I skiped.
        /// </summary>
        /// <returns></returns>
        public List<decimal> CalculateIncrementalPayments()
        {
            decimal previousIncrementalPayment = 0; //at the very first origin year, there is no previous incremental payment
            List<decimal> incrementalPayments = new List<decimal>(); //to store the results of the triangles 

            //I used this aproach since I wasn't able to access _claims via indexer and at the same time it was some how elegant solution (reduces the number of i,j,k indexers).
            _claims.Aggregate(
            (Claim currentClaim, Claim nextClaim) =>
            {
                previousIncrementalPayment += currentClaim.Payment;

                if (currentClaim.OriginYear.Equals(nextClaim.OriginYear))
                {
                    //For possible claims that had no payments in the progressive years and left out 
                    for (int i = currentClaim.DevelopmentYear.Year; i < nextClaim.DevelopmentYear.Year; i++)
                    {
                        incrementalPayments.Add(previousIncrementalPayment);
                    }
                }
                else
                {
                    //No iteration because the next claim is from diffrent origin year
                    incrementalPayments.Add(previousIncrementalPayment);

                    //For possible origin years with no claims that where left out
                    for (int i = currentClaim.OriginYear.Year + 1; i < nextClaim.OriginYear.Year; i++)
                    {
                        incrementalPayments.Add(0);
                    }

                    //reseting previous incremental payment because it is a new origin year, the next cycle will not need incremental value
                    previousIncrementalPayment = 0;
                }

                //If it is the last itaration, add it to the the list, previousIncrementalPayment will be zero in case it is a diffrent origin year
                if (nextClaim.Equals(_claims.Last()))
                {
                    incrementalPayments.Add(previousIncrementalPayment + nextClaim.Payment);
                }

                return nextClaim;
            });

            return incrementalPayments;
        }
        #endregion

        #region ComparableLogic
        public int CompareTo(Triangle other)
        {
            return EarliestOriginYear.CompareTo(other.EarliestOriginYear);
        }

        public int CompareTo(object obj)
        {
            Triangle other = obj as Triangle;

            if (other is null)
            {
                throw new NotSupportedException($"Cannot compare {obj} to {this.GetType()}");
            }

            return CompareTo(other);
        }
        #endregion
    }
}
