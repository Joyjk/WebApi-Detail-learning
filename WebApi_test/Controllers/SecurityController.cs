using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IDataProtector dataProtectionProvider;
        private readonly HashServices hashServices;

        public SecurityController(IDataProtectionProvider dataProtectionProvider,
            HashServices hashServices)
        {
            this.dataProtectionProvider = dataProtectionProvider.CreateProtector("value_Secret_and_unique");
            this.hashServices = hashServices;
        }
        [HttpGet]
        public IActionResult Get()
        {
            string plainText = "Hello";
            string EncryptedText = dataProtectionProvider.Protect(plainText);
            string DecryptedText = dataProtectionProvider.Unprotect(EncryptedText);

            return Ok(new {plainText,EncryptedText ,DecryptedText});
        }
        [HttpGet("TimeBound")]
        public async Task<IActionResult> GetTimeBound()
        {
            var protectedTimeBound = dataProtectionProvider.ToTimeLimitedDataProtector();
            string plainText = "Hello";
            string EncryptedText = protectedTimeBound.Protect(plainText,lifetime:System.TimeSpan.FromSeconds(5));
            await Task.Delay(7000);
            string DecryptedText = protectedTimeBound.Unprotect(EncryptedText);

            return Ok(new { plainText, EncryptedText, DecryptedText });
        }

        [HttpGet("hash")]
        public IActionResult GetHash()
        {
            string plainText = "Hello";
            var hashResult1 = hashServices.Hash(plainText);
            var hashResult2 = hashServices.Hash(plainText);
            return Ok(new { plainText, hashResult1, hashResult2});
        }

    }
}
