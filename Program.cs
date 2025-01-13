using System.Text;
using authentication_jwt.Models;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database App
string sqlServerConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(sqlServerConnection));

string MyAllowSpecificOrigins = "_myAllowspecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(MyAllowSpecificOrigins,
    builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.SetIsOriginAllowed(origin => true);
        builder.WithExposedHeaders("X-Pagination");
    });
});

builder.Services.AddHttpContextAccessor();

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
