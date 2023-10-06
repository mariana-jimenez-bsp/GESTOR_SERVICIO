using System.Text;
using BSP.POS.NEGOCIOS.CorreosService;
using BSP.POS.NEGOCIOS.WhatsappService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICorreosInterface, CorreosService>();
builder.Services.AddScoped<IWhatsappInterface, WhatsappService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Nueva Política", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// Configuración de autenticación JWT
string _secretKey;
string _tipoInicio;
//var configuration = new ConfigurationBuilder()
//    .AddUserSecrets<Program>()
//    .Build();
var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

_tipoInicio = configuration["AppSettings:TipoInicio"];
if (_tipoInicio == "debug")
{
    _secretKey = Environment.GetEnvironmentVariable("SecretKeyGS");
}
else
{
    _secretKey = configuration["AppSettings:SecretKey"];
}

    
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // En producción, establece a true para requerir HTTPS
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "BSP",
        ValidAudience = "Usuarios",
        IssuerSigningKey = key
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Nueva Política");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
