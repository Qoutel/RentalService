using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace RentalService.Controllers
{
    public class HomeController : Controller
    {
        readonly IdentityContext _context;
        readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager,
            IdentityContext context, ApplicationDbContext dbContext, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IActionResult> Index(bool BMW, int page = 1)
        {
            int itemsOnPage = 4;
            var vehicles = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Include(m => m.Photos);
            var count = await vehicles.CountAsync();
            var items = await vehicles.Skip((page - 1) * itemsOnPage).Take(itemsOnPage).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, itemsOnPage);
            var brands = await _dbContext.VehicleBrand.ToListAsync();
            HomePageViewModel model = new HomePageViewModel { PageViewModel = pageViewModel, Vehicles = items,
                VehicleBrands = brands, BMW = BMW};

            return View(model);
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
            List<Vehicle> cars = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).ToList();
            return View(cars);
        }
        public IActionResult GetPartial(string[] vehicleType, string[] brand)
        {
            List<Vehicle> cars = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).ToList();
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