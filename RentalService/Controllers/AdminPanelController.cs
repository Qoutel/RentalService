using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using RentalService.Interface;
using System.Data;
using System.Data.SqlClient;

namespace RentalService.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminPanelController : Controller
    {
        const string CONNECTION_STRING = "Server = localhost; Database = RentalService; Trusted_Connection = True;";
        private IDbManager dbManager;
        private IDbManagerADONET dbManagerADONET;
        readonly IdentityContext? _context;
        readonly ApplicationDbContext? _dbContext;
        private readonly UserManager<User>? _userManager;
        private readonly SignInManager<User>? _signInManager;
        public AdminPanelController(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context, 
            ApplicationDbContext dbContext, IDbManager _dbManager, IDbManagerADONET _dbManagerADONET)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
            dbManager = _dbManager;
            dbManagerADONET = _dbManagerADONET;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddVehicle()
        {
            SelectList fuelType = new SelectList(await dbManagerADONET.GetFuelTypes(), "Id", "Name");
            ViewBag.FuelTypes = fuelType;
            SelectList vehicleType = new SelectList(await dbManagerADONET.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            SelectList location = new SelectList(await dbManagerADONET.GetLocations(), "Id", "Name");
            ViewBag.Location = location;
            SelectList vehicleClass = new SelectList(await dbManagerADONET.GetVehicleClassifications(), "Id", "Name");
            ViewBag.VehicleClass = vehicleClass;
            SelectList vehicleBrand = new SelectList(await dbManagerADONET.GetVehicleBrands(), "Id", "Name");
            ViewBag.VehicleBrand = vehicleBrand;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle(AddVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                FuelType? fuelType = await dbManagerADONET.GetFuelTypeById(model.FuelTypeId);
                VehicleType? vehicleType = await dbManagerADONET.GetVehicleTypeById(model.VehicleTypeId);
                Location? location = await dbManagerADONET.GetLocationById(model.LocationId);
                VehicleClassification? vehicleClass = await dbManagerADONET.GetVehicleClassificationById(model.VehicleClassId);
                VehicleBrand? brand = await dbManagerADONET.GetVehicleBrandById(model.BrandId);
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
                await dbManagerADONET.AddVehicle(vehicle);
                return RedirectToAction("VehicleManagment");
            }
            return RedirectToAction("AddVehicle");
        }

        public async Task<IActionResult> VehicleManagment()
        {
            List<Vehicle> vehicles = await dbManagerADONET.GetVehicles();
            List<VehicleType> vehicleTypes = await dbManagerADONET.GetVehicleTypes();
            List<FuelType> fuelTypes = await dbManagerADONET.GetFuelTypes();
            List<Location> locations = await dbManagerADONET.GetLocations();
            List<VehicleBrand> vehicleBrands = await dbManagerADONET.GetVehicleBrands();
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
        public async Task<IActionResult> PartialVehicleManagmentFilter(int? vehicleTypeId, int? branId, int? fuelTypeId, int? locationId, int? vehicleId)
        {
            if (vehicleId != null)
            {
                var vehicle = await dbManagerADONET.GetVehicleById(vehicleId.Value);
                if (vehicle != null)
                {
                    await dbManagerADONET.RemoveVehicle(vehicle);
                }
            }
            List<Vehicle> vehicles = await dbManagerADONET.GetVehicles();
            
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
        public async Task<IActionResult> VehicleInfo(int vehicleId)
        {
            var vehicle = await dbManagerADONET.GetVehicleById(vehicleId);
            if (vehicle != null)
            {
                return View(vehicle);
            }
            return RedirectToAction("VehicleManagment");
        }
        public async Task<IActionResult> VehicleEdit(int vehicleId)
        {
            var vehicle = await dbManagerADONET.GetVehicleById(vehicleId);
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
        public async Task<IActionResult> VehicleEdit(EditVehicleViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                var result = await dbManagerADONET.GetVehicleById(vehicle.Id);
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
                    await dbManagerADONET.UpdateVehicle(result);
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
        public async Task<IActionResult> BrandList()
        {
            List<VehicleBrand> brands = await dbManagerADONET.GetVehicleBrands();
            return View(brands);
        }
        [HttpGet]
        public IActionResult AddVehicleBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleBrand(VehicleBrand model)
        {
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Enter 'Name' field");
                return View(model);
            }
            var isSuccess = await dbManagerADONET.AddVehicleBrand(model);
            if (isSuccess)
            {
                return RedirectToAction("BrandList");
            }
            ModelState.AddModelError("", "Brand already exist");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> VehicleClassList()
        {
            var vehicleClasses = await dbManagerADONET.GetVehicleClassifications();
            var vehicleTypes = await dbManagerADONET.GetVehicleTypes();
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
        public async Task<IActionResult> PartialVehicleClassFilter(int? vehicleTypeId)
        {
            var vehicleClasses = await dbManagerADONET.GetVehicleClassifications();
            var vehicleTypes = await dbManagerADONET.GetVehicleTypes();
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
        public async Task<IActionResult> AddVehicleClass()
        {
            SelectList vehicleType = new SelectList(await dbManagerADONET.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleClass(AddVehicleClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Enter required fields");
                return View(model);
            }
            VehicleClassification vehicleClassification = new VehicleClassification() { Name = model.ClassName, VehicleTypeId = model.VehicleTypeId };
            bool isSuccessful = await dbManagerADONET.AddVehicleClassification(vehicleClassification);
            if (isSuccessful)
            {
                return RedirectToAction("VehicleClassList");
            }
            ModelState.AddModelError("", "Class already exist");
            SelectList vehicleType = new SelectList(dbManager.GetVehicleTypes(), "Id", "Name");
            ViewBag.VehicleType = vehicleType;
            return View(model);
        }
        public async Task<IActionResult> AdditionalServicesList()
        {
            var model = await dbManagerADONET.GetAdditionalServices();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddAdditionalService()
        {
            return View();
        }
        public async Task<IActionResult> AddAdditionalService(AdditionalService model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Enter 'Name' field");
                return View(model);
            }
            bool isSuccessful = await dbManagerADONET.AddAdditionalService(model);
            if (isSuccessful)
            {
                return RedirectToAction("AdditionalServicesList");
            }
            ModelState.AddModelError("", "Additional service already exist");
            return View(model);
        }
        public async Task<IActionResult> LocationList()
        {
            var model = await dbManagerADONET.GetLocations();
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Enter 'Name' field");
                return View(model);
            }
            bool isSuccessful = await dbManagerADONET.AddLocation(model);
            if (isSuccessful)
            {
                return RedirectToAction("LocationList");
            }
            ModelState.AddModelError("", "Location already exist");
            return View(model);
        }
        public async Task<IActionResult> FuelTypeList()
        {
            var model = await dbManagerADONET.GetFuelTypes();
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Enter 'Name' field");
                return View(model);
            }
            bool isSuccessful = await dbManagerADONET.AddFuelType(model);
            if (isSuccessful)
            {
                return RedirectToAction("FuelTypeList");
            }
            ModelState.AddModelError("", "Fuel type already exist");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditAdditionalService(int? serviceId)
        {
            if (serviceId != null && serviceId > 0)
            {
                var service = await dbManagerADONET.GetAdditionalServiceById(serviceId.Value);
                if (service != null)
                {
                    return View(service);
                }
            }
            return RedirectToAction("AdditionalServicesList");
        }
        [HttpPost]
        public async Task<IActionResult> EditAdditionalService(AdditionalService model)
        {
            var service = await dbManagerADONET.GetAdditionalServiceById(model.Id);
            if (service != null)
            {
                service.Price = model.Price;
                await dbManagerADONET.UpdateAdditionalService(service);
            }
            return RedirectToAction("AdditionalServicesList");
        }
        public async Task<IActionResult> CurrentRentList()
        {
            var currentRents = await dbManagerADONET.GetRents();
            ViewBag.Customers = await dbManagerADONET.GetUsers();
            return View(currentRents);
        }
        public async Task<IActionResult> RentInfo(int rentId, string customerId)
        {
            var rent = await dbManagerADONET.GetRentById(rentId);
            ViewBag.Customer = await dbManagerADONET.GetUserById(Int32.Parse(customerId));
            if (rent != null)
            {
                return View(rent);
            }
            return RedirectToAction("CurrentRentList");
        }
    }
}
