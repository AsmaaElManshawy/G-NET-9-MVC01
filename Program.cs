using GymManagment.BLL.ViewModels;
using GymManagment.BLL.Profiles;
using GymManagment.BLL.Services.Classes;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.DAL.Context;
using GymManagment.DAL.Repositories.Class;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==================MVC Services=========================
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // =====================Register the DbContext========================== 
            builder.Services.AddDbContext<GymDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            //================Register our repositories====================
            // EF Core will create DbContext and Repository instances per request, ensuring they are disposed of correctly

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<IAttachmentService , AttachmentService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<ISessionRepositorie, SessionRepositorie>();
            builder.Services.AddScoped<ISessionsService, SessionsService>();
            builder.Services.AddScoped<IBookingRepositorie, BookingRepositorie>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));



            var app = builder.Build();

            // seeding
            await app.MigrateAndSeedDataAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
