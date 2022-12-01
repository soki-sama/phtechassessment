
using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.Entities.Interface;
using Propeller.DALC.Sqlite;
using Propeller.DALC.Repositories;
using NLog.Web;
using NLog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// using Propeller.Entities.DbContexts;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// Switch log provider
// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
// TODO: Add proper options to limit pointsof error
builder.Services
    .AddControllers();
// .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hook up the DbContext
//builder.Services.AddDbContext<CustomerDbContext>(dbContextOptions =>
//    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings.CustomersSQLiteConnString"]));

builder.Services.AddDbContext<CustomerDbContext>(dbContextOptions =>
    dbContextOptions.UseSqlite("Data Source=propeller.db"));

// Inject Repos
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
builder.Services.AddScoped<ICustomerStatusRepository, CustomerStatusRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// Inject Automapper
// builder.Services.AddAutoMapper(typeof(Propeller.Mappers.CustomerProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.CustomerProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.NoteProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.ContactProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.UserProfile));

// Attach Auth
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Secret"]))
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
