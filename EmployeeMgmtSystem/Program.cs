using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Repository.Implementation;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Host.ConfigureLogging(logging =>
{
	logging.ClearProviders();
	logging.AddConsole();
});
// ----------------[ Add service to the container ]--------------//
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EmployeeDbContext>(options =>
       options.UseNpgsql(connectionString)
);
//---------------------------------[Adding cookie based authentication]--------------------//
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/";
    });

//-----------------------------[Adding Services for Repository]----------------------------//

builder.Services.AddTransient(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddTransient<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddTransient<IAdminRepository,AdminRepository>();

//-----------------------------------------------------------------------------------------//
var app = builder.Build();
// ----------------[ Configure the HTTP request pipeline ] --------//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//--------------------[Use this middleware correctly ]-----------------------//

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name:"employee",
    pattern:"{controller=Employee}/{Action=Employees}/{id?}"
    );

app.Run();
