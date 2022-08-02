using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace WillisTW.Claims.Presentation.Models
{
    public class BufferedSingleFileUploadPhysicalModel : PageModel
    {
        private readonly long _fileSizeLimit;

        public BufferedSingleFileUploadPhysicalModel(IConfiguration config)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }
    }
}
