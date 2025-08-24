using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using TiendaOnline.Data;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;
using TiendaOnline.Services;

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

// INYECCION DE SERVICIOS PARA CONTROLADORES 
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<IProductoService, ProductoService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<ICarritoService, CarritoService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPagoService, PagoService>();
builder.Services.AddTransient<IEntregaService, EntregaService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// FIN INYECCION DE SERVICIOS PARA CONTROLADORES 

// SERVICIO JWT 

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!))
    };

    opciones.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine($"Token received: {context.Token}");
            return Task.CompletedTask;
        }
    };
});

// SERVICIO SWAGGER
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tienda Online API",
        Description = "API para comprar productos online",
        Contact = new OpenApiContact
        {
            Email = "juanandreskochp@gmail.com",
            Name = "Juan Koch",
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit/")
        }
    });

    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// FIN SERVICIO SWAGGER

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

// dotnet ef dbcontext scaffold "Server=DESKTOP-#######; Database=tubd; Trusted_Connection=True; TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context ApplicationDbContext --force

/*
JWT PACKAGES
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
*/