using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using YourApp.Configurations;
using YourApp.Data;
using YourApp.Services;
using YourApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();

        // policy.WithOrigins(
        //         "http://localhost:3000",     // React
        //         "http://localhost:5173",     // Vite
        //         "http://localhost:4200",     // Angular
        //         "http://localhost:8080"      // Тот же origin
        //     )
        //     .AllowAnyMethod()
        //     .AllowAnyHeader()
        //     .AllowCredentials(); // НЕ использовать с AllowAnyOrigin
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth API",
        Version = "v1",
        Description = "API для аутентификации и управления заметками",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com"
        }
    });

    // Настройка авторизации в Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {your_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configure JWT
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

if (jwtSettings == null)
{
    throw new InvalidOperationException("JWT settings not configured");
}

var secretKey = Encoding.ASCII.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IMarkService, MarkService>();
builder.Services.AddScoped<IReferenceDocService, ReferenceDocService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Auth API Documentation";
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();