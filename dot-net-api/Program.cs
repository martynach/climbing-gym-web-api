using System.Text;
using dot_net_api;
using dot_net_api.Authorization;
using dot_net_api.ClimbingGymService;
using dot_net_api.Dtos;
using dot_net_api.Entities;
using dot_net_api.Middlewares;
using dot_net_api.Services;
using dot_net_api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClimbingGymDbContext>();

builder.Services.AddScoped<IClimbingGymService, ClimbingGymService>();
builder.Services.AddScoped<IClimbingRouteService, ClimbingRouteService>();
builder.Services.AddScoped<ClimbingGymDbSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddLogging(config => config.AddConsole());

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

builder.Services.AddControllers().AddFluentValidation();

AuthenticationSettings authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CreatorRequirementHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";

}).AddJwtBearer(cfg =>
{
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.IsAdultPolicy, policyBuilder => policyBuilder.AddRequirements(new MinimumAgeRequirement(18)));
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var dbSeeder = scope.ServiceProvider.GetService<ClimbingGymDbSeeder>();
if (dbSeeder is not null)
{
    Console.WriteLine("Db seeder service found");
    dbSeeder.Seed();
}
else
{
    Console.WriteLine("----No db seeder service");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();


