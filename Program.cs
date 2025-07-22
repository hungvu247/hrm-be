using human_resource_management.Dto;
using human_resource_management.IService;
using human_resource_management.Mapper;
using human_resource_management.Model;
using human_resource_management.Repository;
using human_resource_management.Service;
using human_resource_management.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────
// CONFIG DATABASE & CONTROLLERS
// ───────────────────────────────────────
builder.Services.AddControllers(options =>
{
    // Tắt kiểm tra strict với Accept header (tránh lỗi 406)
    options.ReturnHttpNotAcceptable = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<HumanResourceManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")).LogTo(Console.WriteLine, LogLevel.Information));


// ───────────────────────────────────────
// CONFIG CORS (React FE chạy ở port 3000)
// ───────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3003")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


// ───────────────────────────────────────
// CONFIG JWT
// ───────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
        };
    });

builder.Services.AddAuthorization();


// ───────────────────────────────────────
// CONFIG SWAGGER (thêm support cho JWT)
// ───────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter JWT token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
builder.Services.AddScoped<IPerformanceReviewRepository, PerformanceReviewRepository>();
// ───────────────────────────────────────
// DEPENDENCY INJECTION
// ───────────────────────────────────────
builder.Services.AddScoped<DbSeeder>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<DepartmentService>();

builder.Services.Configure<PasswordSettings>(
    builder.Configuration.GetSection("PasswordSettings")
);
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

// Đăng ký service
builder.Services.AddSingleton<PasswordGenerator>();
builder.Services.AddScoped<IEmailService, EmailService>();

// ───────────────────────────────────────
// BUILD APP
// ───────────────────────────────────────
var app = builder.Build();

// ───────────────────────────────────────
// MIDDLEWARE PIPELINE
// ───────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Set Content-Type response
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
    await next();
});

// MUST ORDER: HTTPS -> CORS -> Auth -> Controllers
app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
