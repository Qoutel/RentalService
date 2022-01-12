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
        ApplicationContext _context;
        ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController (UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context, ApplicationDbContext dbContext)
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userManager.FindByIdAsync(userId);
            var photo = _dbContext.UserPassportPhoto.Where(x => x.Name == user.Email + "PassportPhoto").ToList();
            CabinetViewModel model = new CabinetViewModel() { PassportPhoto = photo, User = user };
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        [HttpPost]
        public async Task<IActionResult> AddPassportPhoto(PassportPhotoViewModel model)
        {
            if (model.Photo != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            else
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Cabinet");
        }
        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            var photo =  _dbContext.UserPassportPhoto.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _dbContext.UserPassportPhoto.Remove(photo);
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Cabinet");
        }
    }
}
