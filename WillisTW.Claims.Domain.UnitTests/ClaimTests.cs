using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using WillisTW.Claims.Domain.ValueObjects;
using Xunit;

namespace WillisTW.Claims.Domain.UnitTests
{
    public class ClaimTests
    {
        [Fact]
        public void ComparableValue_IsGreater_True_Success()
        {
            //Arrange
            Claim oldlClaimValue = Claim.Create(50, new DateTime(1991, 1, 1), new DateTime(1992, 1, 1));
            Claim newClaimValue = Claim.Create(50, new DateTime(1992, 1, 1), new DateTime(1993, 1, 1));

            //Act
            bool isGreater = newClaimValue > oldlClaimValue;

            //Assert
            isGreater.Should().BeTrue();
        }

        [Fact]
        public void ComparableValue_IsGreater_False_Success()
        {
            //Arrange
            Claim oldClaimValue = Claim.Create(50, new DateTime(1991, 1, 1), new DateTime(1992, 1, 1));
            Claim newClaimValue = Claim.Create(50, new DateTime(1992, 1, 1), new DateTime(1993, 1, 1));

            //Act
            bool isGreater = newClaimValue < oldClaimValue;

            //Assert
            isGreater.Should().BeFalse();
        }

        [Fact]
        public void ComparableValue_Sort_Success()
        {
            //Arrange
            List<Claim> claims = new List<Claim>();

            Claim expectedTriangleValue1 = Claim.Create(50m, new DateTime(1993, 1, 1), new DateTime(1993, 1, 1));
            Claim expectedTriangleValue2 = Claim.Create(25.3m, new DateTime(1992, 1, 1), new DateTime(1993, 1, 1));
            Claim expectedTriangleValue3 = Claim.Create(75.7m, new DateTime(1992, 1, 1), new DateTime(1992, 1, 1));
            Claim expectedTriangleValue4 = Claim.Create(75.7m, new DateTime(1991, 1, 1), new DateTime(1993, 1, 1));
            Claim expectedTriangleValue5 = Claim.Create(75.7m, new DateTime(1991, 1, 1), new DateTime(1992, 1, 1));
            Claim expectedTriangleValue6 = Claim.Create(75.7m, new DateTime(1991, 1, 1), new DateTime(1991, 1, 1));
            Claim expectedTriangleValue7 = Claim.Create(75.7m, new DateTime(1990, 1, 1), new DateTime(1993, 1, 1));
            Claim expectedTriangleValue8 = Claim.Create(75.7m, new DateTime(1990, 1, 1), new DateTime(1991, 1, 1));
            Claim expectedTriangleValue9 = Claim.Create(75.7m, new DateTime(1990, 1, 1), new DateTime(1990, 1, 1));

            claims.Add(expectedTriangleValue1);
            claims.Add(expectedTriangleValue9);
            claims.Add(expectedTriangleValue2);
            claims.Add(expectedTriangleValue8);
            claims.Add(expectedTriangleValue3);
            claims.Add(expectedTriangleValue7);
            claims.Add(expectedTriangleValue4);
            claims.Add(expectedTriangleValue6);
            claims.Add(expectedTriangleValue5);

            //Act
            claims.Sort();

            //Assert
            claims.First().Should().BeSameAs(expectedTriangleValue9);
            claims[1].Should().BeSameAs(expectedTriangleValue8);
            claims[2].Should().BeSameAs(expectedTriangleValue7);
            claims[3].Should().BeSameAs(expectedTriangleValue6);
            claims[4].Should().BeSameAs(expectedTriangleValue5);
            claims[5].Should().BeSameAs(expectedTriangleValue4);
            claims[6].Should().BeSameAs(expectedTriangleValue3);
            claims[7].Should().BeSameAs(expectedTriangleValue2);
            claims[8].Should().BeSameAs(expectedTriangleValue1);

        }
    }
}
