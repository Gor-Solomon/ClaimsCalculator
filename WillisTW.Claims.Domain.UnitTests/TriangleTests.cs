using FluentAssertions;
using System;
using System.Collections.Generic;
using WillisTW.Claims.Domain.Entities;
using WillisTW.Claims.Domain.ValueObjects;
using Xunit;

namespace WillisTW.Claims.Domain.UnitTests
{
    public class TriangleTests
    {

        //Facts should have been Theorys and the input data should have been feed from a file and parsed, by due to time shortage I choose Fact
        [Fact]
        public void CalculateIncrementalPayments_NonComp_Success()
        {
            //Arrange
            string productName = "Non-Comp";
            int excpectedDevelopmentYears = 4;
            int excpectedEarliestOriginYear = 1990;
            SortedSet<Claim> claims = new SortedSet<Claim>();
            IEnumerable<decimal> excpectedIncrementalPayments = new List<decimal>() { 45.2M, 110, 110, 147, 50, 125, 150, 55, 140, 100 };

            claims.Add(Claim.Create(100.0m, new DateTime(1993, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(85.0m, new DateTime(1992, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(55.0m, new DateTime(1992, 1, 1), new DateTime(1992, 1, 1)));
            claims.Add(Claim.Create(25.0m, new DateTime(1991, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(75.0m, new DateTime(1991, 1, 1), new DateTime(1992, 1, 1)));
            claims.Add(Claim.Create(50.0m, new DateTime(1991, 1, 1), new DateTime(1991, 1, 1)));
            claims.Add(Claim.Create(37.0m, new DateTime(1990, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(64.8m, new DateTime(1990, 1, 1), new DateTime(1991, 1, 1)));
            claims.Add(Claim.Create(45.2m, new DateTime(1990, 1, 1), new DateTime(1990, 1, 1)));
            Triangle UutTriangle = new Triangle(productName, claims);

            //Act
            IEnumerable<decimal> actualIncrementalPayments = UutTriangle.CalculateIncrementalPayments();

            //Assert
            UutTriangle.Id.ToString().Should().Be(productName);
            UutTriangle.DevelopmentYears.Should().Be(excpectedDevelopmentYears);
            UutTriangle.EarliestOriginYear.Should().Be(excpectedEarliestOriginYear);
            actualIncrementalPayments.Should().BeEquivalentTo(excpectedIncrementalPayments);
        }

        [Fact]
        public void CalculateIncrementalPayments_Comp_Success()
        {
            //Arrange
            string productName = "Comp";
            int excpectedDevelopmentYears = 2;
            int excpectedEarliestOriginYear = 1992;
            SortedSet<Claim> claims = new SortedSet<Claim>();
            IEnumerable<decimal> excpectedIncrementalPayments = new List<decimal>() { 110, 280, 200 };

            claims.Add(Claim.Create(200.0m, new DateTime(1993, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(170.0m, new DateTime(1992, 1, 1), new DateTime(1993, 1, 1)));
            claims.Add(Claim.Create(110.0m, new DateTime(1992, 1, 1), new DateTime(1992, 1, 1)));

            Triangle UutTriangle = new Triangle(productName, claims);

            //Act
            IEnumerable<decimal> actualIncrementalPayments = UutTriangle.CalculateIncrementalPayments();

            //Assert
            UutTriangle.Id.ToString().Should().Be(productName);
            UutTriangle.DevelopmentYears.Should().Be(excpectedDevelopmentYears);
            UutTriangle.EarliestOriginYear.Should().Be(excpectedEarliestOriginYear);
            actualIncrementalPayments.Should().BeEquivalentTo(excpectedIncrementalPayments);
        }

        //there should be other unit tests which tests the exceptions that will be thrown, but due to time I will skip them.
    }
}
