using Data.Repository.Authorization.Interface;
using Data.Repository.Authorization;
using Data.Repository.Interface;
using Data.Repository;
using Services.ServiceAsync.Authorization.Interface;
using Services.ServiceAsync.Authorization;
using Services.ServiceAsync.Interface;
using Services.ServiceAsync;
using Data.Repository.Form.Interface;
using Data.Repository.Form;
using Services.ServiceAsync.Form.Interface;
using Services.ServiceAsync.Form;
using Data.LoggerRepository;
using Data.LoggerRepository.Interface;
using CompetencyMatrixAPI.Tools;

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
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAuditDataRepository, AuditDataRepository>();
builder.Services.AddScoped<ILevelDescriptionRepository, LevelDescriptionRepository>();
builder.Services.AddScoped<IPersonalPurposeRepository, PersonalPurposeRepository>();

//Services
builder.Services.AddScoped<IEmployeeAsyncService, EmpoyeeAsyncService>();
builder.Services.AddScoped<IAuditAsyncService, AuditAsyncService>();
builder.Services.AddScoped<IDepartmentAsyncService, DepartmentAsyncService>();
builder.Services.AddScoped<IAreasAsyncService, AreasAsyncService>();
builder.Services.AddScoped<IPositionAsyncService, PositionAsyncService>();
builder.Services.AddScoped<IAuthorizationAsyncService, AuthorizationAsyncService>();
builder.Services.AddScoped<IPermissionAsyncService, PermissionAsyncService>();
builder.Services.AddScoped<IFormAsyncService, FormAsyncService>();
builder.Services.AddScoped<IQuestionAsyncService, QuestionAsyncService>();
builder.Services.AddScoped<IAuditDataAsyncService, AuditDataAsyncService>();
builder.Services.AddScoped<ILevelDescriptionAsyncService, LevelDescriptionAsyncService>();
builder.Services.AddScoped<IPersonalPurposeAsyncService, PersonalPurposeAsyncService>();

//My logger
builder.Services.AddScoped<Data.LoggerRepository.Interface.ILogger, Logger>();

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

app.UseMiddleware<UserIdMiddleware>();

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
