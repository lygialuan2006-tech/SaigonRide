using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
// 1. Cấu hình Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// ĐĂNG KÝ DỊCH VỤ ĐA NGÔN NGỮ (THIẾU CÁI NÀY NÊN NÓ KHÔNG CHẠY)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddControllersWithViews();
// Đăng ký dịch vụ Authentication bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn tới trang đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddScoped<SaigonRide.Services.RentalService>();

var app = builder.Build();

// 2. Cấu hình Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// ----- CHÈN ĐOẠN NÀY ĐỂ MỞ KHÓA TÍNH NĂNG ĐA NGÔN NGỮ -----
var supportedCultures = new[] { "vi", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
 
app.UseAuthentication(); // BẮT BUỘC phải nằm TRƯỚC UseAuthorization
app.UseAuthorization();

// 3. Cấu hình Route duy nhất
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Intro}/{action=Index}/{id?}"); // Sửa Home thành Intro

 


app.Run(); 