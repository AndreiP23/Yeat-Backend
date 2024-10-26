using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProofOfConcept.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = config["JwtSettings:Issuer"],
//        ValidAudience = config["JwtSettings:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)
//            ),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true
//    };
//});
var firebaseApp = FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("C:\\Users\\Andrei\\Downloads\\yeat-7841b-firebase-adminsdk-lfsmj-b7618b2d2d.json")
});

builder.Services.AddSingleton(firebaseApp);

// Register FirebaseAuth with a factory that uses the initialized FirebaseApp
builder.Services.AddSingleton(provider => FirebaseAuth.GetAuth(firebaseApp));

builder.Services.AddSingleton<ILoginService, LoginService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Firebase";
    options.DefaultChallengeScheme = "Firebase";
}).AddScheme<FirebaseAuthenticationSchemeOptions, AuthService>("Firebase", options => { });

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
