using eSya.ProductSetup.WebAPI.Utility;
using eSya.ProductSetup.WebAPI.Filters;
using Microsoft.Extensions.Configuration;
using DL_ProductSetup = eSya.ProductSetup.DL.Entities;
using eSya.ProductSetup.IF;
using eSya.ProductSetup.DL.Repository;
using Microsoft.Extensions.Localization;
using eSya.ProductSetup.DL.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DL_ProductSetup.eSyaEnterprise._connString = builder.Configuration.GetConnectionString("dbConn_eSyaEnterprise");

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApikeyAuthAttribute>();
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<HttpAuthAttribute>();
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CultureAuthAttribute>();
});
//Localization

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                   //new CultureInfo(name:"en-IN"),
                    new CultureInfo(name:"en-US"),
                    new CultureInfo(name:"ar-EG"),
                };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

});

builder.Services.AddLocalization();
//localization


//for cross origin support
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

builder.Services.AddScoped<IFormsRepository, FormsRepository>();
builder.Services.AddScoped<IApplicationCodesRepository, ApplicationCodesRepository>();
builder.Services.AddScoped<IConfigureMenuRepository, ConfigureMenuRepository>();
builder.Services.AddScoped<ICurrencyMasterRepository, CurrencyMasterRepository>();
builder.Services.AddScoped<IParametersRepository, ParametersRepository>();
builder.Services.AddScoped<IProcessMasterRepository, ProcessMasterRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBusinessStructureRepository, BusinessStructureRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<ITaxIdentificationRepository, TaxIdentificationRepository>();
builder.Services.AddScoped<IDocumentControlRepository, DocumentControlRepository>();
builder.Services.AddScoped<IeSyaCultureRepository, eSyaCultureRepository>();
builder.Services.AddScoped<IConnectRepository, ConnectRepository>();
builder.Services.AddScoped<IAgeRangeRepository, AgeRangeRepository>();
builder.Services.AddScoped<IBusinessCalendarRepository, BusinessCalendarRepository>();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

//Localization

var supportedCultures = new[] { /*"en-IN", */ "en-US", "ar-EG" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
//Localization



app.MapControllers();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=values}/{action=Get}/{id?}");

app.Run();
