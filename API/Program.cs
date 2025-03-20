using System.Security.Claims;
using API.Middleware;
using Application;
using Application.Interface;
using Application.MyMapper;
using Application.Services;
using Application.Validation;
using Domain;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Hangfire;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppSetting>();
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidationAutoValidation();


//config api 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration!.ConnectionStrings.DefaultConnection);
    //options.UseNpgsql(configuration!.ConnectionStrings.LocalDockerConnection);
    options.ConfigureWarnings(warnings =>
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
});
//HangFire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(configuration!.ConnectionStrings.DefaultConnection));
builder.Services.AddHangfireServer();
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddSwaggerGen
    (
    opt =>
    {
        opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization (\"bearer {token}\" ) ",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        opt.OperationFilter<SecurityRequirementsOperationFilter>();

    }

    );
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration!.SecretToken.Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role
        };

        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/signalrHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
            
        };
        opt.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var claims = context.Principal?.Claims;
                foreach (var claim in claims!)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }
            }
        };
        
        
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);
builder.Services.AddSingleton(configuration!);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IHeathMetricService, HealthMetricService>();
builder.Services.AddScoped<IChildenService, ChildrenService>();
builder.Services.AddScoped<IWHOStandardService, WHOStandardService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IPdfExportService, PdfExportService>();
builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
builder.Services.AddScoped<IPostService, PostService>();


builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();

//Tac Vu Ngam dùng HangFire
builder.Services.AddScoped<IScheduleReminderService, ScheduleReminderService>();
builder.Services.AddScoped<IExpiredSubscriptionService, ExpiredSubscriptionService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddScoped<IScheduleReminderService, ScheduleReminderService>();


//builder.Services.AddScoped<IVnPayService, VnPayService>();


builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<RegisterValidator>();
builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<SubPlanValidator>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//HangFire
app.UseHangfireDashboard();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(p => p.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//HangFire
RecurringJob.AddOrUpdate<ScheduleReminderService>("daily-reminder-job",x => x.CheckAndSendReminders(),Cron.Daily(0,0));
RecurringJob.AddOrUpdate<ExpiredSubscriptionService>("Check-EndDate", x => x.CheckExpiredSub(), Cron.Daily(1, 0));
app.MapControllers();
app.Run();


