using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalService.ViewModels;
using RentalService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.IO;

namespace RentalService.Controllers
{
    public class AccountController : Controller
    {
        readonly IdentityContext _context;
        readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController (UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, PhoneNumber = model.PhoneNumber,  FirstName = model.FirstName,
                    LastName = model.LastName, DateOfBirth = model.DateOfBirth, Adress = model.Adress, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cabinet()
        {
            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);
            string passportPhotoName = user.Email + "PassportPhoto";
            string driverLicensePhotoName = user.Email + "DriverLicensePhoto";
            string identificationCodePhotoName = user.Email + "IdentificationCodePhoto";
            var passportPhoto = _dbContext.UserPassportPhoto.Where(x => x.Name == passportPhotoName).ToList();
            var driverLicensePhoto = _dbContext.UserDriverLicensePhoto.Where(x => x.Name == driverLicensePhotoName).ToList();
            var identificationCodePhoto = _dbContext.UserIdentificationCodePhoto.Where(x => x.Name == identificationCodePhotoName).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            CabinetViewModel model = new CabinetViewModel() { PassportPhoto = passportPhoto, User = user, 
                DriversLicensePhoto = driverLicensePhoto, IdentificationCodePhoto = identificationCodePhoto, UserRoles = userRoles};
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            string userId = _userManager.GetUserId(User);
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return View("SuccessfulChangePassword");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"ModelState error, count: {ModelState.ErrorCount}");
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPassportPhoto(UploadPhotoViewModel model)
        {
            if (model.Photo != null)
            {
                string userId = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userId);
                byte[] img = null;
                using (var reader = new BinaryReader(model.Photo.OpenReadStream()))
                {
                    img = reader.ReadBytes((int)model.Photo.Length);
                }
                UserPassportPhoto userPassportPhoto = new UserPassportPhoto() { Name = user.Email + "PassportPhoto", Photo = img };
                user.PassportPhotos.Add(userPassportPhoto);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Cabinet");
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeletePassportPhoto(int id)
        {
            var photo =  _dbContext.UserPassportPhoto.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _dbContext.UserPassportPhoto.Remove(photo);
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Cabinet");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddDriverLicensePhoto(UploadPhotoViewModel model)
        {
            if (model.Photo != null)
            {
                string userId = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userId);
                byte[] img = null;
                using (var reader = new BinaryReader(model.Photo.OpenReadStream()))
                {
                    img = reader.ReadBytes((int)model.Photo.Length);
                }
                UserDriverLicensePhoto userDriverLicensePhoto = new UserDriverLicensePhoto() 
                { 
                    Name = user.Email + "DriverLicensePhoto", 
                    Photo = img 
                };
                user.DriverLicensePhotos.Add(userDriverLicensePhoto);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Cabinet");
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeleteDriverLicensePhoto(int id)
        {
            var photo = _dbContext.UserDriverLicensePhoto.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _dbContext.UserDriverLicensePhoto.Remove(photo);
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Cabinet");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddIdentificationCodePhoto(UploadPhotoViewModel model)
        {
            if (model.Photo != null)
            {
                string userId = _userManager.GetUserId(User);
                User user = await _userManager.FindByIdAsync(userId);
                byte[] img = null;
                using (var reader = new BinaryReader(model.Photo.OpenReadStream()))
                {
                    img = reader.ReadBytes((int)model.Photo.Length);
                }
                UserIdentificationCodePhoto photo = new UserIdentificationCodePhoto() { Name = user.Email + "IdentificationCodePhoto", 
                    Photo = img};
                user.IdentificationCodePhoto.Add(photo);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Cabinet");
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeleteIdentificationCodePhoto(int id)
        {
            var photo = _dbContext.UserIdentificationCodePhoto.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _dbContext.UserIdentificationCodePhoto.Remove(photo);
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Cabinet");
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}
