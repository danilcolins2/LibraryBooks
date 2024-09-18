using LibraryBooks.Models.DbModels;
using LibraryBooks.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddAuthorization();



builder.Services.AddControllersWithViews(); // ��������� ������� MVC

// ��������� ���� ������
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//����������� ������������
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IBorrowRepository, BorrowRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();

var app = builder.Build();

// ���������� ����� �� ���������
app.UseDefaultFiles();
// ���������� ����������� �����
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();   // ���������� middleware ����������� 

// ������������� ������������� ��������� � �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "EditBook",
    pattern: "Admin/EditBook",
    defaults: new { controller = "Admin", action = "EditBook" });
app.Run();