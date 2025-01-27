﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalService.Models;
using RentalService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using RentalService.Interface;
using System.Linq;

namespace RentalService.Controllers
{
    public class VehiclesRentController : Controller
    {
        private IDbManager dbManager;
        readonly IdentityContext? _context;
        readonly ApplicationDbContext? _dbContext;
        private readonly UserManager<User>? _userManager;
        private readonly SignInManager<User>? _signInManager;
        public VehiclesRentController(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context, 
            ApplicationDbContext dbContext, IDbManager _dbManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
            dbManager = _dbManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VehicleInfo(int vehicleId)
        {
            var vehicle = dbManager.GetVehicleById(vehicleId);
            if (vehicle != null)
            {
                return View(vehicle);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult Rent(int vehicleId)
        {
            var vehicle = dbManager.GetVehicleById(vehicleId);
            string userId = _userManager.GetUserId(User);
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            var additionalServices = dbManager.GetAdditionalServices();
            if (vehicle != null)
            {
                RentViewModel model = new RentViewModel()
                {
                    VehicleId = vehicleId,
                };
                ViewBag.Customer = user;
                ViewBag.Vehicle = vehicle;
                ViewBag.AdditionalServices = additionalServices;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Rent(RentViewModel rentalVehicle)
        {
            string userId = _userManager.GetUserId(User);
            var vehicle = dbManager.GetVehicleById(rentalVehicle.VehicleId);
            Rent rent = new Rent()
            {
                CustomerId = userId,
                Vehicle = vehicle,
                SubmissionDate = rentalVehicle.SubmissionDate,
                ReturnDate = rentalVehicle.ReturnDate,
                RentAmount = rentalVehicle.RentAmount
            };
            dbManager.AddRent(rent);
            return RedirectToAction("SuccessfulRent", rent);
        }
        public IActionResult RentAmountPartial(RentViewModel rentalVehicle, string[] addServices)
        {
            var rentDays = rentalVehicle.ReturnDate.Subtract(rentalVehicle.SubmissionDate).Days;
            var vehicle = dbManager.GetVehicleById(rentalVehicle.VehicleId);
            if (vehicle != null)
            {
                decimal rentAmount = rentDays * vehicle.PricePerDay;
                AdditionalService tempServ;
                List<AdditionalService> addServ = new List<AdditionalService>();
                foreach (var service in addServices)
                {
                    tempServ = dbManager.GetAdditionalServices().Where(s => s.Name.Contains(service)).First();
                    if (tempServ.Name == "Personal driver")
                    {
                        rentAmount += tempServ.Price * rentDays;
                    }
                    else
                    {
                        rentAmount += tempServ.Price;
                    }
                    addServ.Add(tempServ);
                }
                rentalVehicle.RentAmount = rentAmount;
                rentalVehicle.AdditionalServices = addServ;
            }
            return PartialView("_RentAmountPartial", rentalVehicle);
        }
        public IActionResult SuccessfulRent (Rent rent)
        {
            string userId = _userManager.GetUserId(User);
            User user = _userManager.Users.Where(u => u.Id == userId).First();
            ViewBag.Customer = user;
            ViewBag.Rent = dbManager.GetRentById(rent.Id);
            return View();
        }
    }
}
