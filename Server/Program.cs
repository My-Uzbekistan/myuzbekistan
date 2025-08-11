using BackuptaGram;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Minio;
using MudBlazorWebApp1.Components.Account;
using myuzbekistan.Services;
using Server;
using Server.Infrastructure;
using Server.Infrastructure.ServiceCollection;
using System.Text.Json;
using tusdotnet.Stores;
using UTC.Minio;


#region Builder And Logging
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;

builder.Services.AddOpenTelemetryFeature(builder);

services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestBody | HttpLoggingFields.RequestPath;
    logging.RequestBodyLogLimit = 1512;
    logging.ResponseBodyLogLimit = 1512;

});

#endregion
#region DATABASE
var dbType = cfg.GetValue<string>("DatabaseProviderConfiguration:ProviderType");
services.AddDataBase<AppDbContext>(env, cfg, (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType!, true));
services.AddAlertaGram();
services.AddBackuptaGram(cfg);
services.AddMinio(cfg);
services.AddSingleton<IMinioUpload>(s =>
{
    var minioClient = new MinioClient()
        .WithEndpoint(cfg["Minio:Endpoint"])
        .WithCredentials(cfg["Minio:AccessKey"], cfg["Minio:SecretKey"])
        .WithSSL(true)
        .Build();
    var createEvent = s.GetRequiredService<IOnCreateCompleteEvent>();

    return new MinioUploadServer((MinioClient)minioClient , cfg, createEvent);
});

//services.AddSingleton<IMinioUpload, MinioUploadServer>();
services.AddControllersWithViews().AddApplicationPart(typeof(MinioController).Assembly).AddControllersAsServices();
services.AddScoped<IUFileService, MinioUploadHelper>();

builder.Services.PostConfigure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var errorResponse = new
        {
            status = 400,
            code = 400,
            Message = errors.FirstOrDefault().Value.First()
        };

        return new BadRequestObjectResult(errorResponse);
    };

    options.SuppressModelStateInvalidFilter = false;
});

#endregion
#region AUTH
services.AddAuth(env, cfg);
services.AddAuthorizationCore(config =>
{
    foreach (var p in builder.Configuration.GetSection("Auth:Policies").Get<List<Policy>>() ?? [])
    {
        config.AddPolicy(p.Name, policy => policy.RequireRole(p.Roles));
    }
});

services.AddHttpClient("multicard", (client) =>
{
    client.BaseAddress = new Uri(cfg.GetValue<string>("Multicard:Url")!);
    client.DefaultRequestHeaders.Add("accept", "application/json");
});

services.AddHttpClient("globalpay", (client) =>
{
    client.BaseAddress = new Uri(cfg.GetValue<string>("GlobalPay:Url")!);
    client.DefaultRequestHeaders.Add("accept", "application/json");
});

services.AddScoped<MultiCardService>();
services.AddScoped<GlobalPayService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, CustomPasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

#endregion
#region ActualLab.Fusion
ComputedState.DefaultOptions.FlowExecutionContext = true;
services.AddFusionServices();
services.AddSingleton(c => new Server.Infrastructure.CommandTracer(c));
#endregion
#region MudBlazor and Pages
builder.Services
    .AddControllers()
 .AddJsonOptions(options =>
  {
      options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());
      //options.JsonSerializerOptions.Converters.Add(new TrimNullableConverter<MainPageApi>());
      //options.JsonSerializerOptions.Converters.Add(new TrimNullableConverter<ContentApiView>());
      //options.JsonSerializerOptions.Converters.Add(new TrimNullableConverter<FavoriteApiView>());
  });
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

services.AddScheduler();
services.AddTransient<CurrencyInvalidateScheduler>();


//var supportedCultures = new[] { "en-US", "ru-RU", "uz-Latn" };

//var localizationOptions = new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("en-US"),
//    SupportedCultures = [.. supportedCultures.Select(c => new System.Globalization.CultureInfo(c))],
//    SupportedUICultures = [.. supportedCultures.Select(c => new System.Globalization.CultureInfo(c))]
//};

// Добавляем провайдер для чтения из заголовка Accept-Language
//localizationOptions.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
//services.Configure<RequestLocalizationOptions>(options =>
//{
//    options = localizationOptions;
//});
#endregion
#region CORS AND PROXY
services.AddCors(cors => cors.AddDefaultPolicy(
   policy => policy
       .WithOrigins("http://localhost:7100", "https://localhost:7101", 
                    "https://auth.utc.uz:44310", "http://localhost:5173",
                    "https://esim-front.vercel.app", "https://esimapp.myuz.uz")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()

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
services.AddTransient<ESimPackageSyncner>();
services.AddSingleton<UserResolver>();
#endregion
#region File
//UFile.Server.UFileRegistration.AddFileServer(services,UploadType.Minio, cfg);
//services.AddFileServer(UploadType.Minio, cfg);
//services.AddFileServer(UploadType.Tus, cfg);
services.AddSingleton(delegate
{
    char sep = Path.DirectorySeparatorChar;
    string currentDirectory = Directory.GetCurrentDirectory();
    string path = $"wwwroot{sep}files";
    string text = Path.Combine(currentDirectory, path);
    if (!Directory.Exists(text))
    {
        Directory.CreateDirectory(text);
    }

    return new TusDiskStore(text);
});
services.AddSingleton<IOnCreateCompleteEvent, OnCreateCompleteEvent>();
services.AddSingleton<IOnAuthorizeEvent, OnAuthorizeEvent>();
services.AddSingleton<IOnFileCompleteEvent, OnFileCompleteEvent>();
#endregion
#region Audit and HttpAcess
services.AddAudit(cfg);
services.AddHttpContextAccessor();
#endregion

#region Telegram Bot
builder.Services.AddSingleton<MerchantNotifierService>();
services.AddSingleton<TelegramBotService>();
builder.Services.AddHostedService<TelegramBotService>();
#endregion


//services.AddSingleton(delegate
//{
//    string currentDirectory = Directory.GetCurrentDirectory();
//    string path = "..\\Server\\wwwroot\\files";
//    string text = Path.Combine(currentDirectory, path);
//    if (!Directory.Exists(text))
//    {
//        Directory.CreateDirectory(text);
//    }

//    return new TusDiskStore(text);
//});
//services.AddSingleton(TusConfiguration.CreateTusConfiguration);
//services.AddScoped<ITusUpload, TusUploadServer>();
//services.AddHostedService<ExpiredFilesCleanupService>();
//services.AddScoped<IUFileService, TusUploadHelper>();

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

app.UseMiddleware<ErrorHandlerMiddleware>();
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

#region BackgroundJobs

var provider = app.Services;
provider.UseScheduler(scheduler =>
{
    scheduler.ScheduleWithParams<ESimPackageSyncner>().Daily();
});

#endregion

app.UseFusionSession();
app.UseRequestLocalization();

app.UseResponseCaching();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseAntiforgery();
UFile.Server.UFileRegistration.UseFileServer(app, UploadType.Minio);
//UFile.Server.UFileRegistration.UseFileServer(app, UploadType.Tus);

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

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<CurrencyInvalidateScheduler>().Cron("0 9 * * *").Zoned(TimeZoneInfo.Local);
    scheduler.Schedule<CurrencyInvalidateScheduler>().Cron("0 16 * * *").Zoned(TimeZoneInfo.Local);
    scheduler.Schedule<CurrencyInvalidateScheduler>().Cron("9 15 * * *").Zoned(TimeZoneInfo.Local);

    scheduler.Schedule<CurrencyInvalidateScheduler>().Cron("0 21 * * *").Zoned(TimeZoneInfo.Local);
}).OnError(exception => throw exception);

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
var apldbContextFactory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
using var apldbContext = apldbContextFactory.CreateDbContext();
await dbContext.Database.MigrateAsync();
await apldbContext.Database.MigrateAsync();
// ✅ Инициализация 
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

    await Seeds.SeedRolesAsync(roleManager);
    await Seeds.SeedAdminAsync(userManager);
    Seeds.SeedAboutContent(dbContext);
}
var commander = app.Services.GetRequiredService<ICommander>();
await commander.Call(new SyncESimSlugCommand());
if (!dbContext.ESimPackages.Any())
{
    await commander.Call(new SyncESimPackagesCommand());
}

app.Run();