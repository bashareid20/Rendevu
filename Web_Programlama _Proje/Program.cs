using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Areas.Identity.Data;

using Web_Programlama__Proje.Models;
namespace wEB_PROJESÝ_SÝYAH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection")
                ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

            // DbContext Ekleme
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity Servisi Ekleme
            builder.Services.AddDefaultIdentity<DetilsUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                // Þifre politikalarý
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
            })
            .AddRoles<IdentityRole>() // Rolleri ekler
            .AddEntityFrameworkStores<ApplicationDbContext>(); // DbContext baðlar

            // Razor Pages ve MVC
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            var app = builder.Build();

            // Middleware sýrasý
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}