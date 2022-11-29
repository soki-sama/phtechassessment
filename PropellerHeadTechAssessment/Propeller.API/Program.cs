
using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.Entities.Interface;
using Propeller.DALC.Sqlite;
using Propeller.DALC.Repositories;
using NLog.Web;
using NLog;
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

// Inject Automapper
// builder.Services.AddAutoMapper(typeof(Propeller.Mappers.CustomerProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.CustomerProfile));
builder.Services.AddAutoMapper(typeof(Propeller.Mappers.NoteProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
