using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Repository.Implementation;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
        options.LoginPath = "/Employee/Login/";
        options.LogoutPath = "/Employee/Index/";
    });

//-----------------------------[Adding Services for Repository]----------------------------//

builder.Services.AddTransient(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddTransient<IEmployeeRepository,EmployeeRepository>();

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


app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}"
    );

app.Run();
