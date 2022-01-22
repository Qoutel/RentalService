using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddVehicle()
        {
            var model = new AddVehicleViewModel();
            SelectList fuelType = new SelectList(_dbContext.FuelType, "Id", "Name");
            ViewBag.FuelTypes = fuelType;
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
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddVehicle(AddVehicleViewModel model)
        {
            FuelType fuelType = _dbContext.FuelType.Where(ft => ft.Id == model.FuelTypeId).First();
            VehicleType vehicleType = _dbContext.VehicleType.Where(vt => vt.Id == model.VehicleTypeId).First();
            Location location = _dbContext.Location.Where(l => l.Id == model.LocationId).First();
            VehicleClassification vehicleClass = _dbContext.VehicleClassification.Where(vc => vc.Id == model.VehicleClassId).First();
            VehicleBrand brand = _dbContext.VehicleBrand.Where(vb => vb.Id == model.BrandId).First();
            byte[] img = null;
            using (var reader = new BinaryReader(model.Photo.OpenReadStream()))
            {
                img = reader.ReadBytes((int)model.Photo.Length);
            }
            VehiclePhoto userPassportPhoto = new VehiclePhoto() { Name = brand.Name + "_" + model.Name, Photo = img };
            Vehicle vehicle = new Vehicle() { Name = model.Name, YearOfManufactured = model.YearOfManufactured,
                Mileage = model.Mileage, PricePerDay = model.PricePerDay, FuelType = fuelType,
                VehicleType = vehicleType, NumberOfSeats = model.NumberOfSeats, AutomaticTransmission = model.AutomaticTransmission,
                Location = location, VehicleClass = vehicleClass, Brand = brand };
            vehicle.Photos.Add(userPassportPhoto);
            await _dbContext.Vehicle.AddAsync(vehicle);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("VehicleManagment");
        }
        [Authorize(Roles = "admin")]
        public IActionResult VehicleManagment(int? vehicleTypeId, int? branId, int? fuelTypeId, int? locationId, int? vehicleId)
        {
            if (vehicleId != null)
            {
                var vehicle = _dbContext.Vehicle.Where(v => v.Id == vehicleId).FirstOrDefault();
                if (vehicle != null)
                {
                    _dbContext.Vehicle.Remove(vehicle);
                    _dbContext.SaveChanges();
                }
            }
            List<Vehicle> vehicles = _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).ToList();
            List<VehicleType> vehicleTypes = _dbContext.VehicleType.ToList();
            List<FuelType> fuelTypes = _dbContext.FuelType.ToList();
            List<Location> locations = _dbContext.Location.ToList();
            List<VehicleBrand> vehicleBrands = _dbContext.VehicleBrand.ToList();
            List<VehicleTypeModel> vtm = vehicleTypes.Select(s => new VehicleTypeModel { Id = s.Id, Name = s.Name }).ToList();
            vtm.Insert(0, new VehicleTypeModel { Id = 0, Name = "All" });
            List<FuelTypeModel> ftm = fuelTypes.Select(s => new FuelTypeModel { Id = s.Id, Name = s.Name }).ToList();
            ftm.Insert(0, new FuelTypeModel { Id = 0, Name = "All" });
            List<LocationModel> lm = locations.Select(s => new LocationModel { Id = s.Id, Name = s.Name }).ToList();
            lm.Insert(0, new LocationModel { Id = 0, Name = "All" });
            List<VehicleBrandModel> vbm = vehicleBrands.Select(s => new VehicleBrandModel { Id = s.Id, Name = s.Name }).ToList();
            vbm.Insert(0, new VehicleBrandModel { Id = 0, Name = "All" });
            VehicleManagmentViewModel model = new VehicleManagmentViewModel { Vehicles = vehicles, VehicleTypes = new SelectList(vtm, "Id", "Name"),
                FuelTypes = new SelectList (ftm, "Id", "Name"), Locations = new SelectList(lm,"Id","Name"), VehicleBrands = new SelectList(vbm, "Id", "Name") };
            if (vehicleTypeId != null && vehicleTypeId > 0)
            {
                vehicles = vehicles.Where(v => v.VehicleType.Id == vehicleTypeId).ToList();
            }
            if (branId != null && branId > 0)
            {
                vehicles = vehicles.Where(v => v.Brand.Id == branId).ToList();
            }
            if (fuelTypeId != null && fuelTypeId > 0)
            {
                vehicles = vehicles.Where(v => v.FuelType.Id == fuelTypeId).ToList();
            }
            if (locationId != null && locationId > 0)
            {
                vehicles = vehicles.Where(v => v.Location.Id == locationId).ToList();
            }
            model.Vehicles = vehicles;
            return View(model);
        }
    }
}
