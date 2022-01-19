using RentalService.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace RentalService.Models
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context, ApplicationDbContext dbContext)
        {
            ApplicationDbContext dbc = new ApplicationDbContext();
            string adminEmail = "admin@gmail.com";
            string password = "Admin1@";
           
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, FirstName = "admin", LastName = "admin", DateOfBirth = DateTime.Now, 
                    Adress = "RentalService.com", IsEmailConfirmed = true};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            FuelType diesel = new FuelType() { Id = 1, Name = "Diesel" };
            FuelType gasoline = new FuelType() { Id = 2, Name = "Gasoline" };
            FuelType electric = new FuelType() { Id = 3, Name = "Electric" };
            FuelType hybridElectricGasoline = new FuelType() { Id = 4, Name = "Hybrid electric" };
            dbContext.FuelType.AddRange(diesel, gasoline, electric, hybridElectricGasoline);
            dbContext.SaveChanges();
            if (!dbContext.Location.Any())
            {
                Location borispol = new Location() { Name = "Kyiv Boryspil International Airport", Adress = "Airport Boryspil, 08307 Boryspil 7, Kyiv Region, Ukraine" };
                Location sikorsky = new Location() { Name = "Kyiv Igor Sikorsky International Airport", Adress = "Vulytsya Medova, 2, Kyiv, Ukraine, 03048" };
                Location stoyanka = new Location() { Name = "Stoyanka Aeroport Kyiv Zhulyany", Adress = "Povitroflots'kyi Ave, 77, Kyiv, Ukraine, 02000" };
                dbContext.Location.AddRange(borispol, sikorsky, stoyanka);
            }
            if (!dbContext.AdditionalService.Any())
            {
                AdditionalService babySeat = new AdditionalService() { Name = "Baby seat", Price = 300 };
                AdditionalService gps = new AdditionalService() { Name = "GPS", Price = 400 };
                AdditionalService pickUp = new AdditionalService() { Name = "Pick up at your adress", Price = 710 };
                AdditionalService dvr = new AdditionalService() { Name = "Video recorder", Price = 377 };
                AdditionalService personalDriver = new AdditionalService() { Name = "Personal driver", Price = 1814 };
                dbContext.AdditionalService.AddRange(babySeat, gps, pickUp, dvr, personalDriver);
            }
        }
    }
}
