using human_resource_management.Model;
using human_resource_management.Repository;
using human_resource_management.Service;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // giữ nguyên tên trường
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<HumanResourceManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Cho phép React truy cập
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
    await next();
});
app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();

app.MapControllers();

app.Run();
