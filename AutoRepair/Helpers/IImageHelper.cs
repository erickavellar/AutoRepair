using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutoRepair.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imagefile, string folder);
    }
}
