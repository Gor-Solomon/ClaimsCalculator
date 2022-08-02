using DomainDrivenDesign.DomainObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WillisTW.Claims.Domain.AggregateRoots;
using Xunit;

namespace WillisTW.Claims.Domain.UnitTests
{
    public class ClaimsAccumulatorTests
    {
        private readonly Id<ClaimsAccumulator> _claimsAccumulatorId = Id<ClaimsAccumulator>.Create(Guid.NewGuid());

        [Fact]
        public void GenerateAccumulatedClaims_Success()
        {

            //Arrange
            StringBuilder expectedResult = new StringBuilder();
            expectedResult.AppendLine("1990, 4");
            expectedResult.AppendLine("Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100");
            expectedResult.AppendLine("Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200");


            List<string> typlessClaims = new List<string>()
            {
                "Comp, 1992, 1992, 110.0",
                "Comp, 1992, 1993, 170.0",
                "Comp, 1993, 1993, 200.0",
                "Non-Comp, 1990, 1990, 45.2",
                "Non-Comp, 1990, 1991, 64.8",
                "Non-Comp, 1990, 1993, 37.0",
                "Non-Comp, 1991, 1991, 50.0",
                "Non-Comp, 1991, 1992, 75.0",
                "Non-Comp, 1991, 1993, 25.0",
                "Non-Comp, 1992, 1992, 55.0",
                "Non-Comp, 1992, 1993, 85.0",
                "Non-Comp, 1993, 1993, 100.0"
            };

            ClaimsAccumulator claimsAccumulatorUut = new ClaimsAccumulator(_claimsAccumulatorId, typlessClaims);

            //Act
            string actualResult = claimsAccumulatorUut.GenerateAccumulatedClaims();

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResult.ToString());

        }
    }
}
