using System.Text;
using authentication_jwt.Models;
using authentication_jwt.Services;
using authentication_jwt.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// 🔐 Carregar variáveis de ambiente
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); 

// Database App
builder.Services.AddDbContextPool<AppDbContext>((serviceProvider, options) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    if(httpContextAccessor.HttpContext?.Request.Headers["TenantId"].FirstOrDefault() != null)
    {
        string sqlServerConnection = builder.Configuration.GetConnectionString(httpContextAccessor.HttpContext.Request.Headers["TenantId"].FirstOrDefault());
        options.UseSqlServer(sqlServerConnection);
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

string MyAllowSpecificOrigins = "_myAllowspecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(MyAllowSpecificOrigins, builder => 
    {
        builder.WithOrigins(
            "https://medscan-web.fly.dev", 
            "http://localhost:3000", 
            "https://medscan-web.vercel.app",
            "https://www.mdscan.xyz",
            "https://mdscan.xyz"
        );
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});


builder.Services.AddControllers();

var key = Encoding.ASCII.GetBytes(authentication_jwt.Settings.Secret);

builder.Services.AddAuthorization(x => 
{
    x.AddPolicy("AdminPolicy", p => p.RequireAuthenticatedUser().RequireClaim("Perfil", "Admin"));
});

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TipoMedicamentoService>();
builder.Services.AddScoped<UnidadesService>();
builder.Services.AddScoped<MedicamentosService>();
builder.Services.AddScoped<ReceituarioService>();
builder.Services.AddScoped<CartaoControleService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<CepService>();
builder.Services.AddScoped<PacientesService>();
builder.Services.AddScoped<AcessoService>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SetupService>();
builder.Services.AddScoped<NotificacoesService>();
builder.Services.AddScoped<RelatoriosService>();
builder.Services.AddScoped<LogsService>();
builder.Services.AddScoped<SolicitacoesService>();
builder.Services.AddScoped<Funcoes>();
builder.Services.AddHostedService<ProcessamentoNotificacoesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
