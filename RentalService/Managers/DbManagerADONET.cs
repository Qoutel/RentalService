using System.Data;
using System.Data.SqlClient;
using RentalService.Interface;
using RentalService.Models;

namespace RentalService.Managers
{
    public class DbManagerADONET : IDbManagerADONET
    {
        const string CONNECTION_STRING = "Server = localhost; Database = RentalService; Trusted_Connection = True;";
        public async Task<List<User>> GetUsers()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Users";
            List<User> users = new List<User>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string FirstName, LastName, Adress, Email, Phone;
                DateTime DateOfBirth, CreatedAt;
                bool IsEmailConfirmed;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    FirstName = reader.GetString("FirstName");
                    LastName = reader.GetString("LastName");
                    Adress = reader.GetString("Adress");
                    Email = reader.GetString("Email");
                    Phone = reader.GetString("Phone");
                    DateOfBirth = reader.GetDateTime("DateOfBirth");
                    CreatedAt = reader.GetDateTime("CreatedAt");
                    IsEmailConfirmed = reader.GetBoolean("IsEmailConfirmed");
                    users.Add(new User 
                    { 
                        Id = Id.ToString(), 
                        FirstName = FirstName, 
                        LastName = LastName, Adress = Adress, 
                        Email = Email, PhoneNumber = Phone, 
                        DateOfBirth = DateOfBirth,
                        CreatedAt = CreatedAt,
                        IsEmailConfirmed = IsEmailConfirmed
                    });
                }
            }
            await connection.CloseAsync();
            return users;
        }
        public async Task<List<Vehicle>> GetVehicles()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Vehicles ORDER BY Id";
            List<Vehicle> vehicles = new List<Vehicle>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id, YearOfManufactured, Mileage, NumberOfSeats, FuelTypeId, LocationId, VehicleClassId, BrandId, VehicleTypeId;
                string ModelName;
                decimal PricePerDay;
                bool isAvailable, AutomaticTransmission;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    ModelName = reader.GetString("ModelName");
                    YearOfManufactured = reader.GetInt32("YearOfManufactured");
                    Mileage = reader.GetInt32("Mileage");
                    NumberOfSeats = reader.GetInt32("NumberOfSeats");
                    FuelTypeId = reader.GetInt32("FuelTypeId");
                    LocationId = reader.GetInt32("LocationId");
                    VehicleClassId = reader.GetInt32("VehicleClassId");
                    BrandId = reader.GetInt32("BrandId");
                    VehicleTypeId = reader.GetInt32("VehicleTypeId");
                    PricePerDay = reader.GetDecimal("PricePerDay");
                    isAvailable = reader.GetBoolean("isAvailable");
                    AutomaticTransmission = reader.GetBoolean("AutomaticTransmission");
                    vehicles.Add(new Vehicle
                    {
                        Id = Id,
                        Name = ModelName,
                        YearOfManufactured = YearOfManufactured,
                        Mileage = Mileage,
                        NumberOfSeats = NumberOfSeats,
                        FuelType = await GetFuelTypeById(FuelTypeId),
                        Location = await GetLocationById(LocationId),
                        VehicleClass = await GetVehicleClassificationById(VehicleClassId),
                        Brand = await GetVehicleBrandById(BrandId),
                        VehicleType = await GetVehicleTypeById(VehicleTypeId),
                        PricePerDay = PricePerDay,
                        IsAvailable = isAvailable,
                        AutomaticTransmission = AutomaticTransmission
                    }); ;
                }
            }
            await connection.CloseAsync();
            return vehicles;
        }
        public async Task<List<VehicleType>> GetVehicleTypes()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleTypes";
            List<VehicleType> vehicleTypes = new List<VehicleType>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string Name;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    vehicleTypes.Add(new VehicleType { Id = Id, Name = Name });
                }
            }
            return vehicleTypes;
        }
        public async Task<List<VehicleBrand>> GetVehicleBrands()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleBrands";
            List<VehicleBrand> vehicleBrands = new List<VehicleBrand>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string Name;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    vehicleBrands.Add(new VehicleBrand { Id = Id, Name = Name });
                }
            }
            return vehicleBrands;
        }
        public async Task<List<VehicleClassification>> GetVehicleClassifications()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleClassifications";
            List<VehicleClassification> vehicleClassifications = new List<VehicleClassification>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id, VehicleTypeId;
                string Name;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    VehicleTypeId = reader.GetInt32("VehicleTypeId");
                    vehicleClassifications.Add(new VehicleClassification { Id = Id, Name = Name, VehicleTypeId = VehicleTypeId });
                }
            }
            return vehicleClassifications;
        }
        public async Task<List<FuelType>> GetFuelTypes()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM FuelTypes";
            List<FuelType> fuelTypes = new List<FuelType>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string Name;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    fuelTypes.Add(new FuelType { Id = Id, Name = Name });
                }
            }
            return fuelTypes;
        }
        public async Task<List<Location>> GetLocations()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Locations";
            List<Location> locations = new List<Location>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string Name, Adress;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    Adress = reader.GetString("Adress");
                    locations.Add(new Location { Id = Id, Name = Name, Adress = Adress });
                }
            }
            return locations;
        }
        public async Task<List<AdditionalService>> GetAdditionalServices()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM AdditionalServices";
            List<AdditionalService> additionalServices = new List<AdditionalService>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id;
                string Name;
                decimal Price;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    Name = reader.GetString("Name");
                    Price = reader.GetDecimal("Price");
                    additionalServices.Add(new AdditionalService { Id = Id, Name = Name, Price = Price });
                }
            }
            return additionalServices;
        }
        public async Task<List<Rent>> GetRents()
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Rents ORDER BY Id";
            List<Rent> rents = new List<Rent>();
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                int Id, VehicleId, CustomerId;
                decimal RentAmount;
                DateTime SubmissionDate, ReturnDate;
                while (await reader.ReadAsync())
                {
                    Id = reader.GetInt32("Id");
                    VehicleId = reader.GetInt32("VehicleId");
                    SubmissionDate = reader.GetDateTime("SubmissionDate");
                    ReturnDate = reader.GetDateTime("ReturnDate");
                    RentAmount = reader.GetDecimal("RentAmount");
                    CustomerId = reader.GetInt32("CustomerId");
                    rents.Add(new Rent { Id = Id, Vehicle = await GetVehicleById(VehicleId), SubmissionDate = SubmissionDate, ReturnDate = ReturnDate, RentAmount = RentAmount,
                        CustomerId = CustomerId.ToString() });
                }
            }
            return rents;
        }
        public async Task<User?> GetUserById(int userId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Users WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", userId);
            User? user = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string FirstName, LastName, Adress, Email, Phone;
                    DateTime DateOfBirth, CreatedAt;
                    bool IsEmailConfirmed;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        FirstName = reader.GetString("FirstName");
                        LastName = reader.GetString("LastName");
                        Adress = reader.GetString("Adress");
                        Email = reader.GetString("Email");
                        Phone = reader.GetString("Phone");
                        DateOfBirth = reader.GetDateTime("DateOfBirth");
                        CreatedAt = reader.GetDateTime("CreatedAt");
                        IsEmailConfirmed = reader.GetBoolean("IsEmailConfirmed");
                        user = new User
                        {
                            Id = Id.ToString(),
                            FirstName = FirstName,
                            LastName = LastName,
                            Adress = Adress,
                            Email = Email,
                            PhoneNumber = Phone,
                            DateOfBirth = DateOfBirth,
                            CreatedAt = CreatedAt,
                            IsEmailConfirmed = IsEmailConfirmed
                        };
                    }
                }
            }
            return user;
        }
        public async Task<Vehicle?> GetVehicleById(int vehicleId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Vehicles WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", vehicleId);
            Vehicle? vehicle = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id, YearOfManufactured, Mileage, NumberOfSeats, FuelTypeId, LocationId, VehicleClassId, BrandId, VehicleTypeId;
                    string ModelName;
                    decimal PricePerDay;
                    bool isAvailable, AutomaticTransmission;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        ModelName = reader.GetString("ModelName");
                        YearOfManufactured = reader.GetInt32("YearOfManufactured");
                        Mileage = reader.GetInt32("Mileage");
                        NumberOfSeats = reader.GetInt32("NumberOfSeats");
                        FuelTypeId = reader.GetInt32("FuelTypeId");
                        LocationId = reader.GetInt32("LocationId");
                        VehicleClassId = reader.GetInt32("VehicleClassId");
                        BrandId = reader.GetInt32("BrandId");
                        VehicleTypeId = reader.GetInt32("VehicleTypeId");
                        PricePerDay = reader.GetDecimal("PricePerDay");
                        isAvailable = reader.GetBoolean("isAvailable");
                        AutomaticTransmission = reader.GetBoolean("AutomaticTransmission");
                        vehicle = new Vehicle
                        {
                            Id = Id,
                            Name = ModelName,
                            YearOfManufactured = YearOfManufactured,
                            Mileage = Mileage,
                            NumberOfSeats = NumberOfSeats,
                            FuelType = await GetFuelTypeById(FuelTypeId),
                            Location = await GetLocationById(LocationId),
                            VehicleClass = await GetVehicleClassificationById(VehicleClassId),
                            Brand = await GetVehicleBrandById(BrandId),
                            VehicleType = await GetVehicleTypeById(VehicleTypeId),
                            PricePerDay = PricePerDay,
                            IsAvailable = isAvailable,
                            AutomaticTransmission = AutomaticTransmission
                        };
                    }
                }
            }
            return vehicle;
        }
        public async Task<Rent?> GetRentById(int rentId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Rents WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", rentId);
            Rent? rent = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id, VehicleId, CustomerId;
                    DateTime SubmissionDate, ReturnDate;
                    decimal RentAmount;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        VehicleId = reader.GetInt32("VehicleId");
                        CustomerId = reader.GetInt32("CustomerId");
                        SubmissionDate = reader.GetDateTime("SubmissionDate");
                        ReturnDate = reader.GetDateTime("ReturnDate");
                        RentAmount = reader.GetDecimal("RentAmount");
                        rent = new Rent { Id = Id, Vehicle = await GetVehicleById(VehicleId), CustomerId = CustomerId.ToString(), SubmissionDate = SubmissionDate,
                            RentAmount = RentAmount, ReturnDate = ReturnDate };
                    }
                }
            }
            return rent;
        }
        public async Task<FuelType?> GetFuelTypeById(int fuelTypeId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM FuelTypes WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", fuelTypeId);
            FuelType? fuelType = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        fuelType = new FuelType { Id = Id, Name = Name };
                    }
                }
            }
            return fuelType;
        }
        public async Task<Location?> GetLocationById(int locationId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM Locations WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", locationId);
            Location? location = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name, Adress;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        Adress = reader.GetString("Adress");
                        location = new Location { Id = Id, Name = Name, Adress = Adress };
                    }
                }
            }
            return location;
        }
        public async Task<AdditionalService?> GetAdditionalServiceById(int additionalServiceId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM AdditionalServices WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", additionalServiceId);
            AdditionalService? additionalService = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name;
                    decimal Price;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        Price = reader.GetDecimal("Price");
                        additionalService = new AdditionalService { Id = Id, Name = Name, Price = Price };
                    }
                }
            }
            return additionalService;
        }
        public async Task<VehicleType?> GetVehicleTypeById(int vehicleTypeId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleTypes WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", vehicleTypeId);
            VehicleType? vehicleType = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        vehicleType = new VehicleType { Id = Id, Name = Name };
                    }
                }
            }
            return vehicleType;
        }
        public async Task<VehicleClassification?> GetVehicleClassificationById(int vehicleClassId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleClassifications WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", vehicleClassId);
            VehicleClassification? vehicleClassification = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name;
                    int VehicleTypeId;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        VehicleTypeId = reader.GetInt32("VehicleTypeId");
                        vehicleClassification = new VehicleClassification { Id = Id, Name = Name, VehicleTypeId = VehicleTypeId };
                    }
                }
            }
            return vehicleClassification;
        }
        public async Task<VehicleBrand?> GetVehicleBrandById(int vehicleBrandId)
        {
            await using var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();
            var selectQuery = connection.CreateCommand();
            selectQuery.CommandType = CommandType.Text;
            selectQuery.CommandText = "SELECT * FROM VehicleBrands WHERE Id = @Id";
            selectQuery.Parameters.AddWithValue("@Id", vehicleBrandId);
            VehicleBrand? vehicleBrand = null;
            await using (var reader = await selectQuery.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    int Id;
                    string Name;
                    while (await reader.ReadAsync())
                    {
                        Id = reader.GetInt32("Id");
                        Name = reader.GetString("Name");
                        vehicleBrand = new VehicleBrand { Id = Id, Name = Name };
                    }
                }
            }
            return vehicleBrand;
        }
        public async Task<bool> AddRent(Rent rent)
        {
            if (rent != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into Rents (VehicleId, SubmissionDate, ReturnDate, RentAmount, CustomerId) values (@VehicleId, @SubmissionDate, " +
                    "@ReturnDate, @RentAmount, @CustomerId)";
                selectQuery.Parameters.AddWithValue("@VehicleId", rent.Vehicle.Id);
                selectQuery.Parameters.AddWithValue("@SubmissionDate", rent.SubmissionDate);
                selectQuery.Parameters.AddWithValue("@ReturnDate", rent.ReturnDate);
                selectQuery.Parameters.AddWithValue("@RentAmount", rent.RentAmount);
                selectQuery.Parameters.AddWithValue("@CustomerId", Int32.Parse(rent.CustomerId));
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddVehicleBrand(VehicleBrand vehicleBrand)
        {
            if (vehicleBrand != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into VehicleBrands (Name) values (@Name)";
                selectQuery.Parameters.AddWithValue("@Name", vehicleBrand.Name);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddVehicleClassification(VehicleClassification vehicleClassification)
        {
            if (vehicleClassification != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into VehicleClassifications (Name, VehicleTypeId) values (@Name, @VehicleTypeId)";
                selectQuery.Parameters.AddWithValue("@Name", vehicleClassification.Name);
                selectQuery.Parameters.AddWithValue("@VehicleTypeId", vehicleClassification.VehicleTypeId);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddAdditionalService(AdditionalService additionalService)
        {
            if (additionalService != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into AdditionalServices (Name, Price) values (@Name, @Price)";
                selectQuery.Parameters.AddWithValue("@Name", additionalService.Name);
                selectQuery.Parameters.AddWithValue("@Price", additionalService.Price);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddLocation(Location location)
        {
            if (location != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into Locations (Name, Adress) values (@Name, @Adress)";
                selectQuery.Parameters.AddWithValue("@Name", location.Name);
                selectQuery.Parameters.AddWithValue("@Adress", location.Adress);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddFuelType(FuelType fuelType)
        {
            if (fuelType != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into FuelTypes (Name) values (@Name)";
                selectQuery.Parameters.AddWithValue("@Name", fuelType.Name);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> AddVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "insert into Vehicles (ModelName, YearOfManufactured, Mileage, PricePerDay, IsAvailable, NumberOfSeats, AutomaticTransmission, " +
                    "FuelTypeId, VehicleTypeId, LocationId, VehicleClassId, BrandId) values (@ModelName, @YearOfManufactured, @Mileage, @PricePerDay, @IsAvailable, " +
                    "@NumberOfSeats, @AutomaticTransmission, @FuelTypeId, @VehicleTypeId, @LocationId, @VehicleClassId, @BrandId)";
                selectQuery.Parameters.AddWithValue("@Id", vehicle.Id);
                selectQuery.Parameters.AddWithValue("@ModelName", vehicle.Name);
                selectQuery.Parameters.AddWithValue("@YearOfManufactured", vehicle.YearOfManufactured);
                selectQuery.Parameters.AddWithValue("@Mileage", vehicle.Mileage);
                selectQuery.Parameters.AddWithValue("@PricePerDay", vehicle.PricePerDay);
                selectQuery.Parameters.AddWithValue("@IsAvailable", vehicle.IsAvailable);
                selectQuery.Parameters.AddWithValue("@NumberOfSeats", vehicle.NumberOfSeats);
                selectQuery.Parameters.AddWithValue("@AutomaticTransmission", vehicle.AutomaticTransmission);
                selectQuery.Parameters.AddWithValue("@FuelTypeId", vehicle.FuelType.Id);
                selectQuery.Parameters.AddWithValue("@VehicleTypeId", vehicle.VehicleType.Id);
                selectQuery.Parameters.AddWithValue("@LocationId", vehicle.Location.Id);
                selectQuery.Parameters.AddWithValue("@VehicleClassId", vehicle.VehicleClass.Id);
                selectQuery.Parameters.AddWithValue("@BrandId", vehicle.Brand.Id);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "delete Vehicles where Id = @Id";
                selectQuery.Parameters.AddWithValue("@Id", vehicle.Id);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "update Vehicles (ModelName, YearOfManufactured, Mileage, PricePerDay, IsAvailable, NumberOfSeats, AutomaticTransmission," +
                    "FuelTypeId, LocationId, VehicleClassId, BranId) set (@ModelName, @YearOfManufactured, @Mileage, @PricePerDay, @IsAvailable, @NumberOfSeats, " +
                    "@AutomaticTransmission, @FuelTypeId, @LocationId, @VehicleClassId, @BranId) where Id = @Id";
                selectQuery.Parameters.AddWithValue("@Id", vehicle.Id);
                selectQuery.Parameters.AddWithValue("@ModelName", vehicle.Name);
                selectQuery.Parameters.AddWithValue("@YearOfManufactured", vehicle.YearOfManufactured);
                selectQuery.Parameters.AddWithValue("@Mileage", vehicle.Mileage);
                selectQuery.Parameters.AddWithValue("@PricePerDay", vehicle.PricePerDay);
                selectQuery.Parameters.AddWithValue("@IsAvailable", vehicle.IsAvailable);
                selectQuery.Parameters.AddWithValue("@NumberOfSeats", vehicle.NumberOfSeats);
                selectQuery.Parameters.AddWithValue("@AutomaticTransmission", vehicle.AutomaticTransmission);
                selectQuery.Parameters.AddWithValue("@FuelTypeId", vehicle.FuelType.Id);
                selectQuery.Parameters.AddWithValue("@LocationId", vehicle.Location.Id);
                selectQuery.Parameters.AddWithValue("@VehicleClassId", vehicle.VehicleClass.Id);
                selectQuery.Parameters.AddWithValue("@BranId", vehicle.Brand.Id);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateAdditionalService(AdditionalService additionalService)
        {
            if (additionalService != null)
            {
                await using var connection = new SqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                var selectQuery = connection.CreateCommand();
                selectQuery.CommandType = CommandType.Text;
                selectQuery.CommandText = "update AdditionalServices set Price = @Price where Id = @Id";
                selectQuery.Parameters.AddWithValue("@Id", additionalService.Id);
                selectQuery.Parameters.AddWithValue("@Price", additionalService.Price);
                await selectQuery.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return true;
            }
            return false;
        }
    }
}
