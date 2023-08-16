using Data.Repository.Authorization.Interface;
using Data.Repository.Authorization;
using Data.Repository.Interface;
using Data.Repository;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Services.ServiceAsync.Authorization.Interface;
using Services.ServiceAsync.Authorization;
using Services.ServiceAsync.Interface;
using Services.ServiceAsync;
using Tools.FileServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Database
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPositionRepositry, PositionRepositry>();
builder.Services.AddScoped<IAutorizationRepository, AutorizationRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

//Services
builder.Services.AddScoped<IEmployeeAsyncService, EmpoyeeAsyncService>();
builder.Services.AddScoped<IAuditAsyncService, AuditAsyncService>();
builder.Services.AddScoped<IDepartmentAsyncService, DepartmentAsyncService>();
builder.Services.AddScoped<IAreasAsyncService, AreasAsyncService>();
builder.Services.AddScoped<IPositionAsyncService, PositionAsyncService>();
builder.Services.AddScoped<IAuthorizationAsyncService, AuthorizationAsyncService>();
builder.Services.AddScoped<IPermissionAsyncService, PermissionAsyncService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
