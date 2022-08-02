using Ardalis.GuardClauses;
using DomainDrivenDesign.DomainObjects;
using System;

namespace WillisTW.Claims.Domain.ValueObjects
{
    internal class Claim : ComparableValue<ClaimCompareValue>
    {
        #region Constructors
        private Claim(decimal payment,
                      DateTime originYear,
                      DateTime developmentYear) : base(new ClaimCompareValue(originYear, developmentYear))
        {
            OriginYear = originYear;
            DevelopmentYear = developmentYear;
            Payment = payment;
        }

        public static Claim Create(decimal payment,
                                   DateTime originYear,
                                   DateTime developmentYear)
        {
            return new Claim(payment, originYear, developmentYear);
        }

        /// <summary>
        /// There should be more checks for throwing more expresive exceptions in case of wrong format.
        /// </summary>
        /// <param name="typelessPayment"></param>
        /// <param name="typelessOriginYear"></param>
        /// <param name="typelessDevelopmentYear"></param>
        /// <returns></returns>
        public static Claim Create(string typelessPayment,
                                   string typelessOriginYear,
                                   string typelessDevelopmentYear)
        {
            Guard.Against.NullOrWhiteSpace(typelessPayment, nameof(typelessPayment));
            Guard.Against.NullOrWhiteSpace(typelessOriginYear, nameof(typelessOriginYear));
            Guard.Against.NullOrWhiteSpace(typelessDevelopmentYear, nameof(typelessDevelopmentYear));

            decimal payment = decimal.Parse(typelessPayment);
            DateTime originYear = new DateTime(int.Parse(typelessOriginYear), 1, 1);
            DateTime developmentYear = new DateTime(int.Parse(typelessDevelopmentYear), 1, 1);

            return new Claim(payment, originYear, developmentYear);
        }
        #endregion

        #region Properties
        public decimal Payment { get; }
        public DateTime OriginYear { get; }
        public DateTime DevelopmentYear { get; }
        #endregion

    }
}
