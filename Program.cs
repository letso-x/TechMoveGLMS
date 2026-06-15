using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("GLMSAPI", client =>
{
    var baseUrl = builder.Configuration["GLMSAPI:BaseUrl"]
                  ?? "http://localhost:5001/";
    client.BaseAddress = new Uri(baseUrl);
});

// register service dependencies
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
