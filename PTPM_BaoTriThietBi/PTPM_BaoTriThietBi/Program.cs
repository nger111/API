using DAL;
using DAL.Interfaces;
using BLL;
using BLL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Đăng ký DAL + BLL ---
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ITaiSanRepository>(sp => new TaiSanRepository(connectionString));
builder.Services.AddScoped<ITaiSanService, TaiSanService>();
// -----------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
