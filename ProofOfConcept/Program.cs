using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProofOfConcept.Controllers;
using ProofOfConcept.Refit;
using ProofOfConcept.Services;
using Refit;
using System.Text;

var yeatAppName = "Yeat";

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
    Credential = GoogleCredential.FromFile(@"C:\Users\tavi_\Documents\firebase_backend\yeat-7841b-firebase-adminsdk-lfsmj-2a42250f92.json")
});

builder.Services.AddSingleton(firebaseApp);

// Register FirebaseAuth with a factory that uses the initialized FirebaseApp
builder.Services.AddSingleton(provider => FirebaseAuth.GetAuth(firebaseApp));
builder.Services.AddRefitClient<ISerpApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://serpapi.com"));
builder.Services.AddRefitClient<IOCR>().ConfigureHttpClient(c => c.BaseAddress = new Uri("http://127.0.0.1:5000"));

builder.Services.AddSingleton<ILoginService, LoginService>();
builder.Services.AddSingleton<ISerpService, SerpService>();
builder.Services.AddSingleton<IOCRService, OCRService>();
builder.Services.AddHttpClient<ILocationsService, LocationsService>();
builder.Services.AddSingleton<ISearchService, SearchService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: yeatAppName,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Firebase";
    options.DefaultChallengeScheme = "Firebase";
}).AddScheme<FirebaseAuthenticationSchemeOptions, AuthService>("Firebase", options => { });

//test

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(yeatAppName);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
