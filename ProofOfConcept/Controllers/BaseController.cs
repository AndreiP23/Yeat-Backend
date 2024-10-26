using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProofOfConcept.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        [HttpGet]
        [Route("GetTest")]
        public string GetTest()
        {
            return "test is succesful very";
        }
    }
}
