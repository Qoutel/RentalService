using Microsoft.AspNetCore.Mvc;
using RentalService.Models;

namespace RentalService.Controllers
{
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddVehicle()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            return View();
        }
    }
}
