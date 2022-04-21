using NUnit.Framework;
using RentalService.Controllers;
using RentalService.Models;
using RentalService.Interface;
using Moq;

namespace RentalServiceUnitTests
{
    [TestFixture]
    public class AdminPanelControllerTests
    {
        [TestCase]
        public void AddFuelType_ShouldReturnInvalidModelState_IfFuelTypeAlreadyExists()
        {
            //arrange
            FuelType ft = new FuelType 
            {
                Id = 11,
                Name = "Diesel"
            };

            Mock<IDbManager> mock = new Mock<IDbManager>();
            mock.Setup(m => m.GetFuelTypes()).Returns(new List<FuelType>
            {
                new FuelType
                {
                    Id = 11,
                    Name = "Diesel"
                }
            });

            AdminPanelController apc = new AdminPanelController(mock.Object);

            //act
            apc.AddFuelType(new FuelType 
            {
                Id = 11,
                Name = "Diesel"
            });

            //assert
            Assert.AreEqual(apc.ModelState.ValidationState.ToString(), "Invalid");
        }
    }
}
