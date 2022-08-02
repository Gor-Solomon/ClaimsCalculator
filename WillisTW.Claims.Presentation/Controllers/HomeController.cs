using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WillisTW.Claims.Presentation.Models;
using WillisTW.Claims.Services.Abstractions;

namespace WillisTW.Claims.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClaimsAccumulatorService _claimsAccumulatorService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IClaimsAccumulatorService claimsAccumulatorService, ILogger<HomeController> logger)
        {
            _claimsAccumulatorService = claimsAccumulatorService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> CalculateCumulativeClaimsData(IFormFile claimsDataFile, CancellationToken cancellationToken)
        {
            IActionResult actionResult;

            try
            {
                byte[] fileContent = await _claimsAccumulatorService.GenerateAccumulatedClaimsFileStream(claimsDataFile, cancellationToken);
                actionResult = File(fileContent, "text/csv", "accumilated_triangles.csv");
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ex);
                _logger.LogError(ex, $"Error occurred during {nameof(CalculateCumulativeClaimsData)}");
            }

            return actionResult;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
