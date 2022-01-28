using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RentalService.Controllers
{
    [Authorize(Roles = "admin")]
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
        [HttpGet]
        public IActionResult AddVehicle()
        {
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
        [HttpPost]
        public async Task<IActionResult> AddVehicle(AddVehicleViewModel model)
        {
            if (ModelState.IsValid)
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
                Vehicle vehicle = new Vehicle()
                {
                    Name = model.Name,
                    YearOfManufactured = model.YearOfManufactured,
                    Mileage = model.Mileage,
                    PricePerDay = model.PricePerDay,
                    FuelType = fuelType,
                    VehicleType = vehicleType,
                    NumberOfSeats = model.NumberOfSeats,
                    AutomaticTransmission = model.AutomaticTransmission,
                    Location = location,
                    VehicleClass = vehicleClass,
                    Brand = brand
                };
                vehicle.Photos.Add(userPassportPhoto);
                await _dbContext.Vehicle.AddAsync(vehicle);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("VehicleManagment");
            }
            return RedirectToAction("AddVehicle");
        }

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
                FuelTypes = new SelectList(ftm, "Id", "Name"), Locations = new SelectList(lm, "Id", "Name"), VehicleBrands = new SelectList(vbm, "Id", "Name") };
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
        public async Task<IActionResult> VehicleInfo(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Include(m => m.Photos)
                .Where(v => v.Id == vehicleId).FirstOrDefaultAsync();
            if (vehicle != null)
            {
                return View(vehicle);
            }
            return RedirectToAction("VehicleManagment");
        }
        public async Task<IActionResult> VehicleEdit(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicle.Include(m => m.Brand).Include(m => m.VehicleType)
                .Include(m => m.VehicleClass).Include(m => m.Location).Include(m => m.FuelType).Where(v => v.Id == vehicleId).FirstOrDefaultAsync();
            if (vehicle != null)
            {
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
                EditVehicleViewModel model = new EditVehicleViewModel() { Name = vehicle.Name, Id = vehicleId, YearOfManufactured = vehicle.YearOfManufactured,
                    Mileage = vehicle.Mileage, PricePerDay = vehicle.PricePerDay, FuelTypeId = vehicle.FuelType.Id, VehicleTypeId = vehicle.VehicleType.Id,
                    NumberOfSeats = vehicle.NumberOfSeats, AutomaticTransmission = vehicle.AutomaticTransmission, LocationId = vehicle.Location.Id,
                    VehicleClassId = vehicle.VehicleClass.Id, BrandId = vehicle.Brand.Id };
                return View(model);
            }
            return RedirectToAction("VehicleManagment");
        }
        [HttpPost]
        public async Task<IActionResult> VehicleEdit(EditVehicleViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                var result = _dbContext.Vehicle.Where(v => v.Id == vehicle.Id).FirstOrDefault();
                if (result != null)
                {
                    FuelType fuelType = _dbContext.FuelType.Where(ft => ft.Id == vehicle.FuelTypeId).First();
                    VehicleType vehicleType = _dbContext.VehicleType.Where(vt => vt.Id == vehicle.VehicleTypeId).First();
                    Location location = _dbContext.Location.Where(l => l.Id == vehicle.LocationId).First();
                    VehicleClassification vehicleClass = _dbContext.VehicleClassification.Where(vc => vc.Id == vehicle.VehicleClassId).First();
                    VehicleBrand brand = _dbContext.VehicleBrand.Where(vb => vb.Id == vehicle.BrandId).First();
                    result.Name = vehicle.Name;
                    result.YearOfManufactured = vehicle.YearOfManufactured;
                    result.Mileage = vehicle.Mileage;
                    result.PricePerDay = vehicle.PricePerDay;
                    result.FuelType = fuelType;
                    result.VehicleType = vehicleType;
                    result.NumberOfSeats = vehicle.NumberOfSeats;
                    result.AutomaticTransmission = vehicle.AutomaticTransmission;
                    result.Location = location;
                    result.VehicleClass = vehicleClass;
                    result.Brand = brand;
                    if (vehicle.Photo != null)
                    {
                        byte[] img = null;
                        using (var reader = new BinaryReader(vehicle.Photo.OpenReadStream()))
                        {
                            img = reader.ReadBytes((int)vehicle.Photo.Length);
                        }
                        VehiclePhoto photo = new VehiclePhoto() { Name = brand.Name + "_" + vehicle.Name, Photo = img };
                        result.Photos.Add(photo);
                    }
                    _dbContext.Vehicle.Update(result);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("VehicleManagment");
                }
            }
            return RedirectToAction("VehicleEdit", new { vehicleId = vehicle.Id });
        }
        [HttpPost]
        public IActionResult DeleteVehiclePhoto(int? photoId, int? id)
        {
            if (photoId != null && photoId > 0)
            {
                var photo = _dbContext.VehiclePhoto.Where(p => p.Id == photoId).FirstOrDefault();
                if (photo != null)
                {
                    _dbContext.VehiclePhoto.Remove(photo);
                    _dbContext.SaveChanges();
                }
            }
            return RedirectToAction("VehicleInfo", new { vehicleId = id });
        }
        public async Task<IActionResult> BrandList()
        {
            var model = await _dbContext.VehicleBrand.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddVehicleBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleBrand(VehicleBrand model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbContext.VehicleBrand.Where(b => b.Name == model.Name).FirstOrDefaultAsync();
                if (result == null)
                {
                    await _dbContext.VehicleBrand.AddAsync(new VehicleBrand() { Name = model.Name });
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("BrandList");
                }
                else
                {
                    ModelState.AddModelError("", "Brand already exist");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> VehicleClassList(int? vehicleTypeId)
        {
            var vehicleClasses = await _dbContext.VehicleClassification.ToListAsync();
            if (vehicleTypeId != null && vehicleTypeId > 0)
            {
                vehicleClasses = vehicleClasses.Where(v => v.VehicleTypeId == vehicleTypeId).ToList();
            }
            var vehicleTypes = await _dbContext.VehicleType.ToListAsync();
            List<VehicleTypeModel> vtm = vehicleTypes.Select(s => new VehicleTypeModel { Id = s.Id, Name = s.Name }).ToList();
            vtm.Insert(0, new VehicleTypeModel { Id = 0, Name = "All" });
            VehicleClassListViewModel model = new VehicleClassListViewModel()
            {
                VehicleClasses = vehicleClasses,
                VehicleTypes = vehicleTypes,
                VehicleTypesFilter = new SelectList(vtm, "Id", "Name"),
            };
            ViewBag.VehicleTypes = await _dbContext.VehicleType.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddVehicleClass()
        {
            SelectList vehicleType = new SelectList(_dbContext.VehicleType, "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleClass(AddVehicleClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbContext.VehicleClassification.Where(c => c.Name == model.ClassName).FirstOrDefaultAsync();
                if (result == null)
                {
                    VehicleClassification vehicleClassification = new VehicleClassification() { Name = model.ClassName, VehicleTypeId = model.VehicleTypeId };
                    await _dbContext.VehicleClassification.AddAsync(vehicleClassification);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("VehicleClassList");
                }
                else
                {
                    ModelState.AddModelError("", "Class already exist");
                }
            }
            SelectList vehicleType = new SelectList(_dbContext.VehicleType, "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View(model);
        }
        public async Task<IActionResult> AdditionalServicesList()
        {
            var model = await _dbContext.AdditionalService.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddAdditionalService()
        {
            return View();
        }
        public async Task<IActionResult> AddAdditionalService(AdditionalService model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbContext.AdditionalService.Where(service => service.Name == model.Name).FirstOrDefaultAsync();
                if (result == null)
                {
                    await _dbContext.AdditionalService.AddAsync(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("AdditionalServicesList");
                }
                else
                {
                    ModelState.AddModelError("", "Additional service already exist");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> LocationList()
        {
            var model = await _dbContext.Location.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddLocation(Location model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbContext.Location.Where(location => location.Name == model.Name).FirstOrDefaultAsync();
                if (result == null)
                {
                    await _dbContext.Location.AddAsync(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("LocationList");
                }
                else
                {
                    ModelState.AddModelError("", "Location already exist");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> FuelTypeList()
        {
            var model = await _dbContext.FuelType.ToListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddFuelType()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFuelType(FuelType model)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbContext.FuelType.Where(fuelType => fuelType.Name == model.Name).FirstOrDefaultAsync();
                if (result == null)
                {
                    await _dbContext.FuelType.AddAsync(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("FuelTypeList");
                }
                else
                {
                    ModelState.AddModelError("", "Fuel type already exist");
                }
            }
            return View(model);
        }
    }
}
