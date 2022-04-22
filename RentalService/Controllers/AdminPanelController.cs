using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using RentalService.Interface;

namespace RentalService.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPanelController : Controller
    {
        private IDbManager dbManager;
        readonly IdentityContext? _context;
        readonly ApplicationDbContext? _dbContext;
        private readonly UserManager<User>? _userManager;
        private readonly SignInManager<User>? _signInManager;
        public AdminPanelController(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context, 
            ApplicationDbContext dbContext, IDbManager _dbManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
            dbManager = _dbManager;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddVehicle()
        {
            SelectList fuelType = new SelectList(dbManager.GetFuelTypes(), "Id", "Name");
            ViewBag.FuelTypes = fuelType;
            SelectList vehicleType = new SelectList(dbManager.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            SelectList location = new SelectList(dbManager.GetLocations(), "Id", "Name");
            ViewBag.Location = location;
            SelectList vehicleClass = new SelectList(dbManager.GetVehicleClassifications(), "Id", "Name");
            ViewBag.VehicleClass = vehicleClass;
            SelectList vehicleBrand = new SelectList(dbManager.GetVehicleBrands(), "Id", "Name");
            ViewBag.VehicleBrand = vehicleBrand;
            return View();
        }
        [HttpPost]
        public IActionResult AddVehicle(AddVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                FuelType? fuelType = dbManager.GetFuelTypeById(model.FuelTypeId);
                VehicleType? vehicleType = dbManager.GetVehicleTypeById(model.VehicleTypeId);
                Location? location = dbManager.GetLocationById(model.LocationId);
                VehicleClassification? vehicleClass = dbManager.GetVehicleClassificationById(model.VehicleClassId);
                VehicleBrand? brand = dbManager.GetVehicleBrandById(model.BrandId);
                byte[]? img = null;
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
                dbManager.AddVehicle(vehicle);
                return RedirectToAction("VehicleManagment");
            }
            return RedirectToAction("AddVehicle");
        }

        public IActionResult VehicleManagment()
        {
            List<Vehicle> vehicles = dbManager.GetVehicles();
            List<VehicleType> vehicleTypes = dbManager.GetVehicleTypes();
            List<FuelType> fuelTypes = dbManager.GetFuelTypes();
            List<Location> locations = dbManager.GetLocations();
            List<VehicleBrand> vehicleBrands = dbManager.GetVehicleBrands();
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
            return View(model);
        }
        public IActionResult PartialVehicleManagmentFilter(int? vehicleTypeId, int? branId, int? fuelTypeId, int? locationId, int? vehicleId)
        {
            if (vehicleId != null)
            {
                var vehicle = dbManager.GetVehicleById(vehicleId.Value);
                if (vehicle != null)
                {
                    dbManager.RemoveVehicle(vehicle);
                }
            }
            List<Vehicle> vehicles = dbManager.GetVehicles();
            
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
            return PartialView("_VehicleListPartial", vehicles);

        }
        public IActionResult VehicleInfo(int vehicleId)
        {
            var vehicle = dbManager.GetVehicleById(vehicleId);
            if (vehicle != null)
            {
                return View(vehicle);
            }
            return RedirectToAction("VehicleManagment");
        }
        public IActionResult VehicleEdit(int vehicleId)
        {
            var vehicle = dbManager.GetVehicleById(vehicleId);
            if (vehicle != null)
            {
                SelectList fuelType = new SelectList(dbManager.GetFuelTypes(), "Id", "Name");
                ViewBag.FuelTypes = fuelType;
                SelectList vehicleType = new SelectList(dbManager.GetVehicleTypes(), "Id", "Name");
                ViewBag.VehicleType = vehicleType;
                SelectList location = new SelectList(dbManager.GetLocations(), "Id", "Name");
                ViewBag.Location = location;
                SelectList vehicleClass = new SelectList(dbManager.GetVehicleClassifications(), "Id", "Name");
                ViewBag.VehicleClass = vehicleClass;
                SelectList vehicleBrand = new SelectList(dbManager.GetVehicleBrands(), "Id", "Name");
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
        public IActionResult VehicleEdit(EditVehicleViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetVehicleById(vehicle.Id);
                if (result != null)
                {
                    FuelType? fuelType = result.FuelType;
                    VehicleType? vehicleType = result.VehicleType;
                    Location? location = result.Location;
                    VehicleClassification? vehicleClass = result.VehicleClass;
                    VehicleBrand? brand = result.Brand;
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
                    dbManager.UpdateVehicle(result);
                    return RedirectToAction("VehicleManagment");
                }
            }
            return RedirectToAction("VehicleEdit", new { vehicleId = vehicle.Id });
        }
        [HttpPost]
        public IActionResult DeleteVehiclePhoto(int? photoId, int? id)
        {
            if (photoId != null)
            {
                VehiclePhoto? photo = dbManager.GetVehiclePhotoById(photoId.Value);
                if (photo != null)
                {
                    dbManager.RemoveVehiclePhoto(photo);
                }
            }
            return RedirectToAction("VehicleInfo", new { vehicleId = id });
        }
        public IActionResult BrandList()
        {
            var model = dbManager.GetVehicleBrands();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddVehicleBrand()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVehicleBrand(VehicleBrand model)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetVehicleBrands().Where(b => b.Name == model.Name).FirstOrDefault();
                if (result == null)
                {
                    dbManager.AddVehicleBrand(model);
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
        public IActionResult VehicleClassList()
        {
            var vehicleClasses = dbManager.GetVehicleClassifications();
            var vehicleTypes = dbManager.GetVehicleTypes();
            List<VehicleTypeModel> vtm = vehicleTypes.Select(s => new VehicleTypeModel { Id = s.Id, Name = s.Name }).ToList();
            vtm.Insert(0, new VehicleTypeModel { Id = 0, Name = "All" });
            VehicleClassListViewModel model = new VehicleClassListViewModel()
            {
                VehicleClasses = vehicleClasses,
                VehicleTypes = vehicleTypes,
                VehicleTypesFilter = new SelectList(vtm, "Id", "Name"),
            };
            return View(model);
        }
        public IActionResult PartialVehicleClassFilter(int? vehicleTypeId)
        {
            var vehicleClasses = dbManager.GetVehicleClassifications();
            var vehicleTypes = dbManager.GetVehicleTypes();
            if (vehicleTypeId != null && vehicleTypeId > 0)
            {
                vehicleClasses = vehicleClasses.Where(v => v.VehicleTypeId == vehicleTypeId).ToList();
            }
            VehicleClassListViewModel model = new VehicleClassListViewModel()
            {
                VehicleClasses = vehicleClasses,
                VehicleTypes = vehicleTypes,
            };
            return PartialView("_VehicleClassListPartial", model);

        }
        [HttpGet]
        public IActionResult AddVehicleClass()
        {
            SelectList vehicleType = new SelectList(dbManager.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View();
        }
        [HttpPost]
        public ActionResult AddVehicleClass(AddVehicleClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetVehicleClassifications().Where(c => c.Name == model.ClassName).FirstOrDefault();
                if (result == null)
                {
                    VehicleClassification vehicleClassification = new VehicleClassification() { Name = model.ClassName, VehicleTypeId = model.VehicleTypeId };
                    dbManager.AddVehicleClassification(vehicleClassification);
                    return RedirectToAction("VehicleClassList");
                }
                else
                {
                    ModelState.AddModelError("", "Class already exist");
                }
            }
            SelectList vehicleType = new SelectList(dbManager.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View(model);
        }
        public IActionResult AdditionalServicesList()
        {
            var model = dbManager.GetAdditionalServices();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddAdditionalService()
        {
            return View();
        }
        public IActionResult AddAdditionalService(AdditionalService model)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetAdditionalServices().Where(service => service.Name == model.Name).FirstOrDefault();
                if (result == null)
                {
                    dbManager.AddAdditionalService(model);
                    return RedirectToAction("AdditionalServicesList");
                }
                else
                {
                    ModelState.AddModelError("", "Additional service already exist");
                }
            }
            return View(model);
        }
        public IActionResult LocationList()
        {
            var model = dbManager.GetLocations();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddLocation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLocation(Location model)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetLocations().Where(location => location.Name == model.Name).FirstOrDefault();
                if (result == null)
                {
                    dbManager.AddLocation(model);
                    return RedirectToAction("LocationList");
                }
                else
                {
                    ModelState.AddModelError("", "Location already exist");
                }
            }
            return View(model);
        }
        public IActionResult FuelTypeList()
        {
            var model = dbManager.GetFuelTypes();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddFuelType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddFuelType(FuelType model)
        {
            if (ModelState.IsValid)
            {
                var result = dbManager.GetFuelTypes().Where(fuelType => fuelType.Name == model.Name).FirstOrDefault();
                if (result == null)
                {
                    dbManager.AddFuelType(model);
                    return RedirectToAction("FuelTypeList");
                }
                else
                {
                    ModelState.AddModelError("", "Fuel type already exist");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditAdditionalService(int? serviceId)
        {
            if (serviceId != null && serviceId > 0)
            {
                var service = dbManager.GetAdditionalServiceById(serviceId.Value);
                if (service != null)
                {
                    return View(service);
                }
            }
            return RedirectToAction("AdditionalServicesList");
        }
        [HttpPost]
        public IActionResult EditAdditionalService(AdditionalService model)
        {
            var service = dbManager.GetAdditionalServiceById(model.Id);
            if (service != null)
            {
                service.Price = model.Price;
                dbManager.UpdateAdditionalService(service);
            }
            return RedirectToAction("AdditionalServicesList");
        }
        public IActionResult CurrentRentList()
        {
            var currentRents = dbManager.GetRents();
            ViewBag.Customers = _userManager.Users.ToList();
            return View(currentRents);
        }
        public IActionResult RentInfo(int rentId, string customerId)
        {
            var rent = dbManager.GetRentById(rentId);
            ViewBag.Customer = _userManager.Users.Where(u => u.Id == customerId).First();
            if (rent != null)
            {
                return View(rent);
            }
            return RedirectToAction("CurrentRentList");
        }
    }
}
