using BankWebService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DBManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Index}/{id?}");

//testing db creation - DELETE LATER
var okSchema = DBManager.RecreateSchema();
Console.WriteLine($"DB Schema Recreated: {okSchema}. Path: {Path.GetFullPath("mydatabase.db")}");
var ok2 = DBManager.CreateUserProfileTable();
Console.WriteLine($"DB created: {ok2}. Path: {Path.GetFullPath("mydatabase.db")}");
var ok = DBManager.CreateAccountsTable();
Console.WriteLine($"DB created: {ok}. Path: {Path.GetFullPath("mydatabase.db")}");
var ok1 = DBManager.CreateTransactionsTable();
Console.WriteLine($"DB created: {ok1}. Path: {Path.GetFullPath("mydatabase.db")}");

app.Run();
