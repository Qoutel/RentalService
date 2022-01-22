using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalService.Models;
using RentalService.ViewModels;

namespace RentalService.Controllers
{
    public class AdminPanelController : Controller
    {
        readonly IdentityContext _context;
        readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AdminPanelController(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddVehicle()
        {
            var model = new AddVehicleViewModel();
            SelectList fuelType = new SelectList(_dbContext.FuelType, "Id", "Name");
            ViewBag.FuelType = fuelType;
            SelectList vehicleType = new SelectList(_dbContext.VehicleType, "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            SelectList location = new SelectList(_dbContext.Location, "Id", "Name");
            ViewBag.Location = location;
            SelectList vehicleClass = new SelectList(_dbContext.VehicleClassification, "Id", "Name");
            ViewBag.VehicleClass = vehicleClass;
            SelectList vehicleBrand = new SelectList(_dbContext.VehicleBrand, "Id", "Name");
            ViewBag.VehicleBrand = vehicleBrand;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle(Vehicle model)
        {
            FuelType fuelType = model.FuelType;
            Vehicle vehicle = new Vehicle() { Name = model.Name, YearOfManufactured = model.YearOfManufactured, 
                Mileage = model.Mileage, PricePerDay = model.PricePerDay, FuelType = model.FuelType, 
                VehicleType = model.VehicleType, NumberOfSeats = model.NumberOfSeats, AutomaticTransmission = model.AutomaticTransmission,
                Location = model.Location, VehicleClass = model.VehicleClass, Brand = model.Brand, Photos = model.Photos };
            await _dbContext.Vehicle.AddAsync(vehicle);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
