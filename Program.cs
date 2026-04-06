using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.IISIntegration;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.DataProtection;
using BusinessERP.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    const string defaultCulture = "en";
    var supportedCultures = new[]
    {
        new CultureInfo(defaultCulture),
        new CultureInfo("ar")
    };

    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    //options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddScoped<ApplicationDbContext>();
var _ApplicationInfo = builder.Configuration.GetSection("ApplicationInfo").Get<ApplicationInfo>();
string _GetConnStringName = ControllerExtensions.GetConnectionString(builder.Configuration);
if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connMySQL)
{
    builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(_GetConnStringName, ServerVersion.AutoDetect(_GetConnStringName)));
}
else if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connPostgreSQL)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_GetConnStringName));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));
}

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

//Set Identity Options
DefaultIdentityOptions _DefaultIdentityOptions = null;
var _context = ProgramTaskExtension.GetDBContextInstance(builder.Services);
try
{
    bool IsDBCanConnect = _context.Database.CanConnect();
    if (IsDBCanConnect && _context.DefaultIdentityOptions.Any())
    {
        _DefaultIdentityOptions = _context.DefaultIdentityOptions.Where(x => x.Id == 1).FirstOrDefault();
    }
}
catch
{
    // Fallback to configuration when DB is not initialized yet.
}

if (_DefaultIdentityOptions == null)
{
    IConfigurationSection _IConfigurationSection = builder.Configuration.GetSection("IdentityDefaultOptions");
    builder.Services.Configure<DefaultIdentityOptions>(_IConfigurationSection);
    _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
}
AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);

//Get Super Admin Default options
builder.Services.Configure<SuperAdminDefaultOptions>(builder.Configuration.GetSection("SuperAdminDefaultOptions"));
builder.Services.Configure<ApplicationInfo>(builder.Configuration.GetSection("ApplicationInfo"));

builder.Services.AddTransient<ICommon, Common>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddTransient<ISalesService, SalesService>();
builder.Services.AddTransient<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<IIncomeServie, IncomeServie>();
builder.Services.AddTransient<IAccount, Account>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IRoles, Roles>();
builder.Services.AddTransient<IFunctional, Functional>();
builder.Services.AddTransient<ITransferItemService, TransferItemService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});


builder.Services.AddDataProtection()
.SetApplicationName("Business ERP Solution")
.AddKeyManagementOptions(options =>
{
    options.NewKeyLifetime = new TimeSpan(180, 0, 0, 0);
    options.AutoGenerateKeys = true;
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Business ERP Solution/Product/POS/Company Management", Version = "v1" });
    c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.OperationFilter<AuthResponsesOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddSignalR();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Business ERP v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCors("Open");

app.MapHub<SignalServer>("/notify");

app.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}");

ProgramTaskExtension.SeedingData(app);
app.Run();
