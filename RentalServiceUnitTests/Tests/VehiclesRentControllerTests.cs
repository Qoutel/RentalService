using NUnit.Framework;
using RentalService.ViewModels;
using RentalService.Controllers;
using RentalService.Models;
using RentalService.Interface;
using Moq;

namespace RentalServiceUnitTests
{
    [TestFixture]
    public class VehiclesRentControllerTests
    {
        [TestCase]
        public void RentAmountPartial_ShouldCalculateRentAmount_Expect_13719()
        {
            //arrange
            RentViewModel rvm = new RentViewModel()
            {
                SubmissionDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(3),
                VehicleId = 1,
            };

            string[] addServices = new string[] { "GPS", "Video recorder", "Personal driver" };
                        
            Mock<IDbManager> mock = new Mock<IDbManager>();
            mock.Setup(m => m.GetVehicles()).Returns(new List<Vehicle> 
            { 
                new Vehicle 
                {
                    Id = 1,
                    Name = "TT",
                    YearOfManufactured = 2008,
                    Mileage = 14000,
                    PricePerDay = 2500,
                    FuelType = new FuelType { Name = "Gasoline" },
                    VehicleClass = new VehicleClassification { Name = "Sports car", VehicleTypeId = 1 },
                    NumberOfSeats = 2,
                    Location = new Location() { Name = "Kyiv Boryspil International Airport", Adress = "Airport Boryspil, 08307 Boryspil 7, Kyiv Region, Ukraine" },
                    VehicleType = new VehicleType() { Name = "Car" },
                    Brand = new VehicleBrand() { Name = "Audi" },
                    IsAvailable = true
                } 
            });
            mock.Setup(m => m.GetAdditionalServices()).Returns(new List<AdditionalService> 
            {
                new AdditionalService
                {
                    Name = "Video recorder", 
                    Price = 377
                },
                new AdditionalService
                {
                    Name = "GPS", 
                    Price = 400
                },
                new AdditionalService
                {
                    Name = "Personal driver", 
                    Price = 1814
                }
            });
            VehiclesRentController vrc = new VehiclesRentController(mock.Object);

            //act
            vrc.RentAmountPartial(rvm, addServices);

            //assert
            Assert.AreEqual(rvm.RentAmount, 13719);
        }
    }
}
