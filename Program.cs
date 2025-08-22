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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

// INYECCION DE SERVICIOS PARA CONTROLADORES 
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<IProductoService, ProductoService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();

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

// dotnet ef dbcontext scaffold "Server=DESKTOP-S5Q2S88; Database=tubd; Trusted_Connection=True; TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context ApplicationDbContext --force

/*

{
    "email": "juanandreskochp@gmail.com",
    "password": "1145"
}

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI0IiwiZW1haWwiOiJqdWFuYW5kcmVza29jaHBAZ21haWwuY29tIiwibm9tYnJlIjoiSnVhbiIsImFwZWxsaWRvIjoiS29jaCIsImp0aSI6IjkzZDBjZDM3LWU0ZGUtNDcxYy1iMTEzLTM0MThmNWI4ZmNlZSIsImV4cCI6MTc4NzQzMTc1Nn0.Jy35HtjH1ntjs7lR7iOr5wdBaEmc_9xWRgbTpPgDkXY

*/