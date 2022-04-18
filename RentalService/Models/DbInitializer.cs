using RentalService.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace RentalService.Models
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
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
            if (!dbContext.FuelType.Any())
            {
                FuelType diesel = new FuelType() { Name = "Diesel" };
                FuelType gasoline = new FuelType() { Name = "Gasoline" };
                FuelType electric = new FuelType() { Name = "Electric" };
                FuelType hybridElectricGasoline = new FuelType() { Name = "Hybrid electric" };
                dbContext.FuelType.AddRange(diesel, gasoline, electric, hybridElectricGasoline);
                dbContext.SaveChanges();
            }
            if (!dbContext.Location.Any())
            {
                Location borispol = new Location() { Name = "Kyiv Boryspil International Airport", Adress = "Airport Boryspil, 08307 Boryspil 7, Kyiv Region, Ukraine" };
                Location sikorsky = new Location() { Name = "Kyiv Igor Sikorsky International Airport", Adress = "Vulytsya Medova, 2, Kyiv, Ukraine, 03048" };
                Location stoyanka = new Location() { Name = "Stoyanka Aeroport Kyiv Zhulyany", Adress = "Povitroflots'kyi Ave, 77, Kyiv, Ukraine, 02000" };
                dbContext.Location.AddRange(borispol, sikorsky, stoyanka);
                dbContext.SaveChanges();
            }
            if (!dbContext.VehicleType.Any())
            {
                VehicleType car = new VehicleType() { Name = "Car" };
                VehicleType moto = new VehicleType() { Name = "Moto" };
                dbContext.VehicleType.AddRange(car, moto);
                dbContext.SaveChanges();
            }
            if (!dbContext.VehicleClassification.Any())
            {
                int carId = dbContext.VehicleType.Where(name => name.Name == "Car").First().Id;
                int motoId = dbContext.VehicleType.Where(name => name.Name == "Moto").First().Id;
                VehicleClassification sedan = new VehicleClassification() { Name = "Sedan", VehicleTypeId = carId };
                VehicleClassification coupe = new VehicleClassification() { Name = "Coupe", VehicleTypeId = carId };
                VehicleClassification sportsCar = new VehicleClassification() { Name = "Sports car", VehicleTypeId = carId };
                VehicleClassification stationWagon = new VehicleClassification() { Name = "Station wagon", VehicleTypeId = carId };
                VehicleClassification hatchback = new VehicleClassification() { Name = "Hatchback", VehicleTypeId = carId };
                VehicleClassification cabriolet = new VehicleClassification() { Name = "Cabriolet", VehicleTypeId = carId };
                VehicleClassification suv = new VehicleClassification() { Name = "Sports-utility vehicle", VehicleTypeId = carId };
                VehicleClassification minivan = new VehicleClassification() { Name = "Minivan", VehicleTypeId = carId };
                VehicleClassification limousine = new VehicleClassification() { Name = "Limousine", VehicleTypeId = carId };
                VehicleClassification roadster = new VehicleClassification() { Name = "Roadster", VehicleTypeId = carId };
                VehicleClassification pickupTrack = new VehicleClassification() { Name = "Pickup track", VehicleTypeId = carId };
                VehicleClassification microcar = new VehicleClassification() { Name = "Microcar", VehicleTypeId = carId };
                VehicleClassification standart = new VehicleClassification() { Name = "Standart", VehicleTypeId = motoId };
                VehicleClassification cruiser = new VehicleClassification() { Name = "Cruiser", VehicleTypeId = motoId };
                VehicleClassification sportBike = new VehicleClassification() { Name = "Sport bike", VehicleTypeId = motoId };
                VehicleClassification touring = new VehicleClassification() { Name = "Touring", VehicleTypeId = motoId };
                VehicleClassification sportTouring = new VehicleClassification() { Name = "Sport touring", VehicleTypeId = motoId };
                VehicleClassification scooter = new VehicleClassification() { Name = "Scooter", VehicleTypeId = motoId };
                VehicleClassification moped = new VehicleClassification() { Name = "Moped", VehicleTypeId = motoId };
                VehicleClassification offRoad = new VehicleClassification() { Name = "Off-road", VehicleTypeId = motoId };
                VehicleClassification dualSport = new VehicleClassification() { Name = "Dual sport", VehicleTypeId = motoId };
                dbContext.VehicleClassification.AddRange(sedan, coupe, sportsCar, stationWagon, hatchback, cabriolet, suv, minivan, limousine, roadster, pickupTrack, microcar,
                    standart, cruiser, sportBike, touring, sportTouring, scooter, moped, offRoad, dualSport);
                dbContext.SaveChanges();
            }
            if (!dbContext.AdditionalService.Any())
            {
                AdditionalService babySeat = new AdditionalService() { Name = "Baby seat", Price = 300 };
                AdditionalService gps = new AdditionalService() { Name = "GPS", Price = 400 };
                AdditionalService pickUp = new AdditionalService() { Name = "Pick up at your adress", Price = 710 };
                AdditionalService dvr = new AdditionalService() { Name = "Video recorder", Price = 377 };
                AdditionalService personalDriver = new AdditionalService() { Name = "Personal driver", Price = 1814 };
                dbContext.AdditionalService.AddRange(babySeat, gps, pickUp, dvr, personalDriver);
                dbContext.SaveChanges();
            }
            if (!dbContext.VehicleBrand.Any())
            {
                VehicleBrand audi = new VehicleBrand() { Name = "Audi"};
                VehicleBrand bmw = new VehicleBrand() { Name = "BMW" };
                VehicleBrand renault = new VehicleBrand() { Name = "Renault" };
                dbContext.VehicleBrand.AddRange(audi, bmw, renault);
                dbContext.SaveChanges();
            }
            if (!dbContext.Vehicle.Any())
            {
                var gasoline = dbContext.FuelType.Where(f => f.Name == "Gasoline").First();
                var car = dbContext.VehicleType.Where(v => v.Name == "Car").First();
                var borispol = dbContext.Location.Where(l => l.Id == 1).First();
                var sportsCar = dbContext.VehicleClassification.Where(vc => vc.Name == "Sports car").First();
                var audi = dbContext.VehicleBrand.Where(vb => vb.Name == "Audi").First();
                var bmw = dbContext.VehicleBrand.Where(vb => vb.Name == "BMW").First();
                var moto = dbContext.VehicleType.Where(vt => vt.Name == "Moto").First();
                var sportbike = dbContext.VehicleClassification.Where(vc => vc.Name == "Sport bike").First();
                var sikorsky = dbContext.Location.Where(l =>l.Id == 2).First();
                var diesel = dbContext.FuelType.Where(f => f.Name == "Diesel").First();
                Vehicle audiTT = new Vehicle()
                {
                    Name = "TT",
                    YearOfManufactured = 2008,
                    Mileage = 14000,
                    PricePerDay = 2500,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi,
                    IsAvailable = false
                };
                Vehicle audiTTrs = new Vehicle()
                {
                    Name = "TT RS",
                    YearOfManufactured = 2021,
                    Mileage = 12000,
                    PricePerDay = 4500,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi
                };
                Vehicle audiR8 = new Vehicle()
                {
                    Name = "R8",
                    YearOfManufactured = 2020,
                    Mileage = 1000,
                    PricePerDay = 5500,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi
                };
                Vehicle audiR8spyder = new Vehicle()
                {
                    Name = "R8 Spyder",
                    YearOfManufactured = 2021,
                    Mileage = 4000,
                    PricePerDay = 6600,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi
                };
                Vehicle audiTTS = new Vehicle()
                {
                    Name = "TTS",
                    YearOfManufactured = 2018,
                    Mileage = 17000,
                    PricePerDay = 5500,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi
                };
                Vehicle audiRS5sportback = new Vehicle()
                {
                    Name = "RS 5 Sportback",
                    YearOfManufactured = 2020,
                    Mileage = 24000,
                    PricePerDay = 7850,
                    FuelType = gasoline,
                    VehicleClass = sportsCar,
                    NumberOfSeats = 2,
                    Location = borispol,
                    VehicleType = car,
                    Brand = audi
                };
                Vehicle bmwR1250RS = new Vehicle()
                {
                    Name = "R 1250 RS",
                    YearOfManufactured = 2019,
                    Mileage = 1000,
                    PricePerDay = 5800,
                    FuelType = diesel,
                    VehicleClass = sportbike,
                    NumberOfSeats = 1,
                    Location = sikorsky,
                    VehicleType = moto,
                    Brand = bmw,
                    AutomaticTransmission = true
                };
                dbContext.Vehicle.AddRange(audiTT, audiTTS, audiTTrs, audiRS5sportback, audiR8spyder, audiR8, bmwR1250RS);
                dbContext.SaveChanges();
            }
        }
    }
}
