using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalService.Interface;
using RentalService.Models;
using RentalService.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace RentalService.Controllers
{
    public class HomeController : Controller
    {
        private IDbManager dbManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<HomeController> logger, IDbManager _dbManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            dbManager = _dbManager;
        }
        public IActionResult Index(int page = 1)
        {
            int itemsOnPage = Int32.Parse(RentalService.Resource.itemsOnHomePage);
            var vehicles = dbManager.GetVehicles();
            var count = vehicles.Count();
            PageViewModel pageViewModel = new PageViewModel(count, page, itemsOnPage);
            var vehicleTypes = dbManager.GetVehicleTypes();
            var brands = dbManager.GetVehicleBrands();
            var fuelTypes = dbManager.GetFuelTypes();
            var locations = dbManager.GetLocations();
            HomePageViewModel model = new HomePageViewModel { PageViewModel = pageViewModel, Vehicles = vehicles,
                VehicleBrands = brands, FuelTypes = fuelTypes, Locations = locations, VehicleTypes = vehicleTypes};

            return View(model);
        }
        public IActionResult PartialVehicleList(bool isAutoTrans, int[] vehicleType, int[] brand, int[] fuelType, int[] location)
        {

            var vehicles = dbManager.GetVehicles();
            List<Vehicle> vehicleTypeFiltered = new List<Vehicle>();
            List<Vehicle> brandFiltered = new List<Vehicle>();
            List<Vehicle> fuelTypeFiltered = new List<Vehicle>();
            List<Vehicle> locationFiltered = new List<Vehicle>();
            List<Vehicle> autoTransFiltered = new List<Vehicle>();
            if (vehicleType.Length != 0)
            {
                foreach (int vt in vehicleType)
                {
                    var filtered = vehicles.Where(v => v.VehicleType.Id == vt).ToList();
                    vehicleTypeFiltered.AddRange(filtered);
                }
            }
            else
            {
                vehicleTypeFiltered = vehicles;
            }
            if (brand.Length != 0)
            {
                foreach (int b in brand)
                {
                    var filtered = vehicles.Where(v => v.Brand.Id == b).ToList();
                    brandFiltered.AddRange(filtered);
                }
            }
            else
            {
                brandFiltered = vehicles;
            }
            if (fuelType.Length != 0)
            {
                foreach (int ft in fuelType)
                {
                    var filtered = vehicles.Where(v => v.FuelType.Id == ft).ToList();
                    fuelTypeFiltered.AddRange(filtered);
                }
            }
            else
            {
                fuelTypeFiltered = vehicles;
            }
            if (location.Length != 0)
            {
                foreach (int l in location)
                {
                    var filtered = vehicles.Where(v => v.Location.Id == l).ToList();
                    locationFiltered.AddRange(filtered);
                }
            }
            else
            {
                locationFiltered = vehicles;
            }
            if (isAutoTrans)
            {
                var filtered = vehicles.Where(v => v.AutomaticTransmission == true).ToList();
                autoTransFiltered.AddRange(filtered);
            }
            else
            {
                autoTransFiltered = vehicles;
            }
            var filteredVehicles = vehicleTypeFiltered.Intersect(brandFiltered);
            filteredVehicles = filteredVehicles.Intersect(fuelTypeFiltered);
            filteredVehicles = filteredVehicles.Intersect(locationFiltered);
            filteredVehicles = filteredVehicles.Intersect(autoTransFiltered);
            return PartialView("_PartialVehicleList", filteredVehicles.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult CarList()
        {
            List<Vehicle> cars = dbManager.GetVehicles();
            return View(cars);
        }
        public IActionResult GetPartial(string[] vehicleType, string[] brand)
        {
            List<Vehicle> cars = dbManager.GetVehicles();
            List<Vehicle> vehicleTypeFiltered = new List<Vehicle>();
            List<Vehicle> brandFiltered = new List<Vehicle>();
            if (vehicleType.Length != 0)
            {
                foreach (string vt in vehicleType)
                {
                    var filtered = cars.Where(v => v.VehicleType.Name.Contains(vt)).ToList();
                    vehicleTypeFiltered.AddRange(filtered);
                }
            }
            else
            {
                vehicleTypeFiltered = cars;
            }
            if (brand.Length != 0)
            {
                foreach (string b in brand)
                {
                    var filtered = cars.Where(v => v.Brand.Name.Contains(b)).ToList();
                    brandFiltered.AddRange(filtered);
                }
            }
            else
            {
                brandFiltered = cars;
            }
            var filteredCars = vehicleTypeFiltered.Intersect(brandFiltered);
            return PartialView("_CarListPartial", filteredCars.ToList());
        }
    }
}