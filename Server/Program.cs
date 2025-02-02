using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Services;
using EF.Audit.Core.Extensions;
using ActualLab.Fusion;
using MudBlazor.Services;
using Blazored.LocalStorage;
using MudBlazor;
using ActualLab.Fusion.Extensions;
using Client.Core.Services;
using myuzbekistan.Shared;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using ActualLab.Fusion.Server;
using ActualLab.Rpc.Server;
using Server.Infrastructure;
using Server.Infrastructure.ServiceCollection;
using Server;
using ActualLab.Fusion.Client.Caching;
using ActualLab.Rpc;
using ActualLab.CommandR.Configuration;
using Client;
using Server.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazorWebApp1.Components.Account;
using Microsoft.AspNetCore.Identity;
using System.Text;


#region Builder And Logging
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;

builder.Services.AddOpenTelemetryFeature(builder);

services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestBody | HttpLoggingFields.RequestPath;
    logging.RequestBodyLogLimit = 512;
    logging.ResponseBodyLogLimit = 512;
});

#endregion
#region DATABASE
var dbType = cfg.GetValue<string>("DatabaseProviderConfiguration:ProviderType");
services.AddDataBase<AppDbContext>(env, cfg, (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType!, true));
#endregion
#region AUTH
services.AddAuth(env, cfg);
services.AddAuthorizationCore(config =>
{
    foreach (var p in builder.Configuration.GetSection("Auth:Policies").Get<List<Policy>>() ?? new List<Policy>())
    {
        config.AddPolicy(p.Name, policy => policy.RequireRole(p.Roles));
    }
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, CustomPasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

#endregion
#region ActualLab.Fusion
ComputedState.DefaultOptions.FlowExecutionContext = true;
services.AddFusionServices();
services.AddSingleton(c => new CommandTracer(c));
#endregion
#region MudBlazor and Pages
builder.Services
    .AddControllers();
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents((t) =>
    {
        t.DetailedErrors = true;

    });
services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion
#region swagger and compression
services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

services.AddEndpointsApiExplorer();
services.AddSwaggerDocument();
#endregion
#region Localization
services.AddLocalization();
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "ru-RU", "uz-Latn" };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});
#endregion
#region CORS AND PROXY
services.AddCors(cors => cors.AddDefaultPolicy(
   policy => policy
       .WithOrigins("http://localhost:7100", "https://localhost:7101", "https://auth.utc.uz:44310")
   ));
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
#endregion
#region UI Services
services.AddBlazoredLocalStorage();
services.AddScoped<IUserPreferencesService, UserPreferencesService>();
services.AddScoped<LayoutService>();
services.AddSingleton<UserContext>();
services.AddScoped<PageHistoryState>();
services.AddScoped<UInjector>();
#endregion
#region Audit and HttpAcess
services.AddAudit(cfg);
services.AddHttpContextAccessor();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseResponseCompression();
}

#region Loggin Proxy Socket
app.UseHttpLogging();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseWebSockets(new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});
#endregion

app.UseFusionSession();
app.UseRequestLocalization();

app.UseResponseCaching();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<FusionAuthMiddleWare>();

#region Mapping
app.MapControllers();
app.MapRpcWebSocketServer();
app
 .MapRazorComponents<App>()
 .AddAdditionalAssemblies(typeof(_discovery).Assembly)
 .AddInteractiveServerRenderMode();
app.MapAdditionalIdentityEndpoints();
#endregion

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
var apldbContextFactory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
using var apldbContext = apldbContextFactory.CreateDbContext();
await dbContext.Database.MigrateAsync();
await apldbContext.Database.MigrateAsync();
// await dbContext.Database.EnsureDeletedAsync();
//dbContext.Database.EnsureCreated();
app.Run();


public class CustomPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    private readonly ILogger<CustomPasswordHasher<TUser>> _logger;

    public CustomPasswordHasher(ILogger<CustomPasswordHasher<TUser>> logger)
    {
        _logger = logger;
    }

    public string HashPassword(TUser user, string password)
    {
        return GetMD5Hash(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        if (hashedPassword == HashPassword(user, providedPassword))
            return PasswordVerificationResult.Success;
        else
            return PasswordVerificationResult.Failed;
    }
    private string GetMD5Hash(string input)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

    //public string HashPassword(TUser user, string password)
    //{
    //    return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10); // workFactor âëèÿåò íà ñêîðîñòü
    //}

    //public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    //{
    //    var stopwatch = Stopwatch.StartNew();

    //    try
    //    {
    //        BCrypt.Net.BCrypt.HashPassword(providedPassword, workFactor: 10); // workFactor âëèÿåò íà ñêîðîñòü
    //        _logger.LogInformation("HashPassword took: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
    //        stopwatch.Restart();
    //        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
    //        ? PasswordVerificationResult.Success
    //        : PasswordVerificationResult.Failed;
    //    }
    //    finally
    //    {
    //        stopwatch.Stop();
    //        _logger.LogInformation("VerifyHashedPassword took: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
    //    }
    //}
}

