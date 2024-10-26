using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;
using ProofOfConcept.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProofOfConcept.Services
{
    public interface ILoginService
    {
        Task<string> RegisterUserAsync(string email, string password);
    }
    public class LoginService : ILoginService
    {
        private FirebaseAuth _firebaseAuth;

        public LoginService(FirebaseAuth firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
        }

        // Register a new user with email and password
        public async Task<string> RegisterUserAsync(string email, string password)
        {
            var userRecordArgs = new UserRecordArgs
            {
                Email = email,
                Password = password,
                EmailVerified = false,
            };

            var userRecord = await _firebaseAuth.CreateUserAsync(userRecordArgs);
            return userRecord.Uid;
        }


        //private readonly IConfiguration _config;

        //public LoginService(IConfiguration config)
        //{
        //    _config = config;
        //}

        //public string GenerateToken(User user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //            {
        //       new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //       new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //       new Claim(ClaimTypes.Role,user.Type)
        //   };

        //    var token = new JwtSecurityToken(
        //        issuer: _config["JwtSettings:Issuer"],
        //        audience: _config["JwtSettings:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: credentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
