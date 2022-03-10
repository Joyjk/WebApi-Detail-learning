using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_test.Controllers
{
    [Route("/")]
    //[ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : ControllerBase
    {
        [Route("/")]
        [Route("/swagger")]
        public RedirectResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
