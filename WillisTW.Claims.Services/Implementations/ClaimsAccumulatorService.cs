using Ardalis.GuardClauses;
using DomainDrivenDesign.DomainObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WillisTW.Claims.Domain.AggregateRoots;
using WillisTW.Claims.Services.Abstractions;

namespace WillisTW.Claims.Services.Implementations
{
    public class ClaimsAccumulatorService : IClaimsAccumulatorService
    {
        public async Task<byte[]> GenerateAccumulatedClaimsFileStream(IFormFile formFile, CancellationToken cancellationToken)
        {
            IEnumerable<string> typlessClaims = await GenerateTyplessClaims(formFile);
            Id<ClaimsAccumulator> _claimsAccumulatorId = Id<ClaimsAccumulator>.Create(Guid.NewGuid());

            ClaimsAccumulator claimsAccumulator = new ClaimsAccumulator(_claimsAccumulatorId, typlessClaims);
            string text = claimsAccumulator.GenerateAccumulatedClaims();

            byte[] fileBytes = Encoding.UTF8.GetBytes(text);

            return fileBytes;
        }

        private async Task<IEnumerable<string>> GenerateTyplessClaims(IFormFile formFile)
        {
            Guard.Against.Null(formFile, nameof(formFile));

            if (formFile.Length < 1)
            {
                throw new InvalidOperationException("File content is empty");
            }

            List<string> claims = new List<string>();
            using Stream fileStream = formFile.OpenReadStream();
            using StreamReader fileStreamReader = new StreamReader(fileStream);

            await fileStreamReader.ReadLineAsync();

            while (!fileStreamReader.EndOfStream)
            {
                string fileLine = await fileStreamReader.ReadLineAsync();
                claims.Add(fileLine);
            }

            return claims;
        }
    }
}
