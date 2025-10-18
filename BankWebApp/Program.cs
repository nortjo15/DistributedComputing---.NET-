var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient to talk to the WebService
var bankApiBase = builder.Configuration.GetValue<string>("BankApi:BaseUrl") ?? "https://localhost:7001/";
builder.Services.AddHttpClient("BankApi", client => client.BaseAddress = new Uri(bankApiBase));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
