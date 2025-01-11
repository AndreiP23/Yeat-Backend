using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Models;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginsController : BaseController
    {
        private readonly ILoginService _loginService;

        public LoginsController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        //[HttpPost]
        //[Route("TryToLog")]
        //public async Task<IActionResult> Log([FromBody] User user)
        //{
        //    var result = _loginService.GenerateToken(user);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password must be provided.");
            }

            try
            {
                var userId = await _loginService.RegisterUserAsync(request.Email, request.Password);
                return Ok(new { UserId = userId });
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
