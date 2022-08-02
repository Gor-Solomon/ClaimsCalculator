using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WillisTW.Claims.Services.Abstractions
{
    public interface IClaimsAccumulatorService
    {
        Task<byte[]> GenerateAccumulatedClaimsFileStream(IFormFile formFile, CancellationToken cancellationToken);
    }
}
