using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Interfaces;
using Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// DAL/BLL
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();

// Tickets
builder.Services.AddTransient<ITicketBusiness, TicketsBusiness>();
builder.Services.AddTransient<IticketsRepository, TicketsResponsitory>();

// Parts
builder.Services.AddTransient<IPartsRepository, PartsRepository>();
builder.Services.AddTransient<IPartsBusiness, PartsBusiness>();

// Users
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUsersBusiness, UsersBusiness>();

// Assets
builder.Services.AddTransient<IAssetsRepository, AssetsRepository>();
builder.Services.AddTransient<IAssetsBusiness, AssetsBusiness>();

// Warranties
builder.Services.AddTransient<IWarrantiesRepository, WarrantiesRepository>();
builder.Services.AddTransient<IWarrantiesBusiness, WarrantiesBusiness>();

// PartUsages
builder.Services.AddTransient<IPartUsagesRepository, PartUsagesRepository>();
builder.Services.AddTransient<IPartUsagesBusiness, PartUsagesBusiness>();

// Schedules
builder.Services.AddTransient<ISchedulesRepository, SchedulesRepository>();
builder.Services.AddTransient<ISchedulesBusiness, SchedulesBusiness>();

// WorkOrders
builder.Services.AddTransient<IWorkOrdersRepository, WorkOrdersRepository>();
builder.Services.AddTransient<IWorkOrdersBusiness, WorkOrdersBusiness>();

// JWT
IConfiguration configuration = builder.Configuration;
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
