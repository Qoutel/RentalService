using NUnit.Framework;
using RentalService.ViewModels;
using RentalService.Controllers;
using Microsoft.AspNetCore.Identity;
using RentalService.Models;
using RentalService.Interface;

namespace RentalService.Tests
{
    [TestFixture]
    public class RentAmountTests
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IdentityContext _context;
        ApplicationDbContext _dbContext;
        IDbManager dbManager;
        public RentAmountTests(UserManager<User> userManager, SignInManager<User> signInManager, IdentityContext context,
        ApplicationDbContext dbContext, IDbManager _dbManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dbContext = dbContext;
            dbManager = _dbManager;
        }
        [TestCase]
        public void When_PersonalDriver_Expect_PriceMultiplyDays()
        {
            RentViewModel rvm = new RentViewModel()
            {
                SubmissionDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(3),
                VehicleId = 1,
            };
            string[] addServices = new string[] { "GPS", "Video recorder", "Personal driver" };
            VehiclesRentController vrc = new VehiclesRentController(_userManager, _signInManager, _context, _dbContext, dbManager);

            vrc.RentAmountPartial(rvm, addServices);

            Assert.AreEqual(rvm.RentAmount, 13719);
        }
    }
}
