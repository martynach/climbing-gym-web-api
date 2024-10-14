using dot_net_api.ClimbingGymService;
using dot_net_api.Entities;
using dot_net_api.Middlewares;
using dot_net_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClimbingGymDbContext>();

builder.Services.AddControllers();

builder.Services.AddScoped<IClimbingGymService, ClimbingGymService>();
builder.Services.AddScoped<ClimbingGymDbSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddLogging(config => config.AddConsole());

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

app.MapControllers();


app.Run();


