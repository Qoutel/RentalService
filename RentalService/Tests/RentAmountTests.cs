using NUnit.Framework;
using RentalService.ViewModels;
using RentalService.Controllers;
using Microsoft.AspNetCore.Identity;
using RentalService.Models;
using RentalService.Interface;
using Moq;

namespace RentalService.Tests
{
    [TestFixture]
    public class RentAmountTests
    {
        Mock<IDbManager> _dbManagerMock = new Mock<IDbManager>();
        IDbManager _dbManager;
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
            Mock<UserManager<User>> mock1 = new Mock<UserManager<User>>();
            Mock<SignInManager<User>> mock2 = new Mock<SignInManager<User>>();
            Mock<IdentityContext> mock3 = new Mock<IdentityContext>();
            Mock<ApplicationDbContext> mock4 = new Mock<ApplicationDbContext>();
            Mock<IDbManager> mock5 = new Mock<IDbManager>();
            mock5.Setup(m => m.GetVehicles()).Returns(new List<Vehicle>());
            mock5.Setup(m => m.GetAdditionalServices()).Returns(new List<AdditionalService>());
            VehiclesRentController vrc = new VehiclesRentController(mock5.Object);

            vrc.RentAmountPartial(rvm, addServices);

            Assert.AreEqual(rvm.RentAmount, 1000);
        }
    }
}
