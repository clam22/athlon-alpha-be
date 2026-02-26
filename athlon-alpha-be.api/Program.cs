using athlon_alpha_be.api.Configuration;
using athlon_alpha_be.api.Middleware;
using athlon_alpha_be.api.Services;
using athlon_alpha_be.database.Persistence;


using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Scalar.AspNetCore;

using Serilog;


using Serilog;

using StackExchange.Redis;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    DotNetEnv.Env.Load();

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();

    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions["traceID"] = ctx.HttpContext.TraceIdentifier;
            ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
        };
    });

    builder.Services.AddHealthChecks()
        .AddNpgSql(
            connectionString: builder.Configuration.GetConnectionString("DatabaseConnection")!,
            name: "PostgreSQL")
        .AddRedis(
            redisConnectionString: builder.Configuration.GetConnectionString("RedisConnection")!,
            name: "Redis");
    builder.Services.AddHealthChecks()
        .AddNpgSql(
            connectionString: builder.Configuration.GetConnectionString("DatabaseConnection")!,
            name: "PostgreSQL")
        .AddRedis(
            redisConnectionString: builder.Configuration.GetConnectionString("RedisConnection")!,
            name: "Redis");

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy
                .WithOrigins(
                    builder.Configuration["Frontend:Url"]!
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    builder.Services.Configure<CognitoSettings>(builder.Configuration.GetSection("Cognito"));

    builder.Services.AddScoped<ICognitoService, CognitoService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            var cognito = builder.Configuration.GetSection("Cognito");
            options.Authority = $"https://cognito-idp.{cognito["Region"]}.amazonaws.com/{cognito["UserPoolId"]}";

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = cognito["ClientId"],
                ValidateLifetime = true,
                RoleClaimType = "cognito:groups"
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddControllers();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")!));
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")!));

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        string connection = builder.Configuration.GetConnectionString("RedisConnection")!;
        return ConnectionMultiplexer.Connect(connection);
    });

    builder.Services.AddOpenApi();
    builder.Services.AddOpenApi();

    WebApplication app = builder.Build();
    WebApplication app = builder.Build();

    app.UseExceptionHandler();
    //app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}




