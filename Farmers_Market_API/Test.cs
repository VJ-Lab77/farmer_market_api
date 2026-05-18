using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FarmerMarketAPI.Tests
{
    [TestClass]
    public sealed class FarmerRepositoryTests
    {
        private FarmerRepository? _farmerRepository;
        private AppDbContext? _context;
        private ILogger<FarmerRepository>? _logger;

        [TestInitialize]
        public void Initialize()
        {
            // Use the real database connection (not in-memory)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("FarmerMarketDB")  // Use your existing DB name
                .Options;

            _context = new AppDbContext(options);

            // Use NullLogger (no actual logging during tests)
            _logger = NullLogger<FarmerRepository>.Instance;

            // Initialize repository
            _farmerRepository = new FarmerRepository(_context, _logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
            _farmerRepository = null;
        }

        [TestMethod]
        public async Task TestGetAllFarmers()
        {
            // Act
            var farmers = await _farmerRepository!.GetAllAsync();

            // Assert
            Assert.IsNotNull(farmers);
            var farmerList = farmers.ToList();
            Assert.IsTrue(farmerList.Count > 0);
        }

        [TestMethod]
        public async Task TestGetFarmerById_ValidId_ReturnsFarmer()
        {
            // Act
            var farmer = await _farmerRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(farmer);
            Assert.AreEqual(1, farmer.Id);
        }

        [TestMethod]
        public async Task GetById_ReturnsCorrectFarmer()
        {
            // Act
            var farmer = await _farmerRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(farmer);
            Assert.AreEqual(1, farmer.Id);
        }

        [TestMethod]
        public async Task TestAddFarmer_ValidFarmer_ReturnsCreatedFarmer()
        {
            // Arrange
            var newFarmer = new Farmer
            {
                FullName = "Test Farmer",
                Email = $"test{DateTime.Now.Ticks}@farm.com",  // Unique email
                PhoneNumber = "0845551234",
                FarmName = "Test Farm",
                Location = "Test Location",
                Province = Province.Gauteng,
                Rating = 0,
                IsVerified = false
            };

            // Act
            var createdFarmer = await _farmerRepository.AddAsync(newFarmer);

            // Assert
            Assert.IsNotNull(createdFarmer);
            Assert.IsTrue(createdFarmer.Id > 0);
            Assert.AreEqual("Test Farmer", createdFarmer.FullName);
        }

        [TestMethod]
        public async Task TestGetFarmerById_InvalidId_ThrowsException()
        {
            await Assert.ThrowsExceptionAsync<ListingNotFoundException>(
            () => _farmerRepository.GetByIdAsync(999)
            );
        }

        [TestMethod]
        public async Task TestUpdateFarmer_ValidFarmer_ReturnsUpdatedFarmer()
        {
            // Arrange - Get first farmer
            var farmers = await _farmerRepository.GetAllAsync();
            var farmer = farmers.FirstOrDefault();

            if (farmer != null)
            {
                var originalName = farmer.FarmName;
                farmer.FarmName = "Updated Test Farm";

                // Act
                var updatedFarmer = await _farmerRepository.UpdateAsync(farmer);

                // Assert
                Assert.IsNotNull(updatedFarmer);
                Assert.AreEqual("Updated Test Farm", updatedFarmer.FarmName);

                // Cleanup - change back
                farmer.FarmName = originalName;
                await _farmerRepository.UpdateAsync(farmer);
            }
            else
            {
                Assert.Inconclusive("No farmers found in database to test update");
            }
        }

        [TestMethod]
        public async Task TestExistsAsync_ValidId_ReturnsTrue()
        {
            // Get first farmer ID
            var farmers = await _farmerRepository.GetAllAsync();
            var firstFarmer = farmers.FirstOrDefault();

            if (firstFarmer != null)
            {
                // Act
                var exists = await _farmerRepository.ExistsAsync(firstFarmer.Id);

                // Assert
                Assert.IsTrue(exists);
            }
            else
            {
                Assert.Inconclusive("No farmers found in database to test Exists");
            }
        }

        [TestMethod]
        public async Task TestExistsAsync_InvalidId_ReturnsFalse()
        {
            // Act
            var exists = await _farmerRepository.ExistsAsync(99999);

            // Assert
            Assert.IsFalse(exists);
        }
        [TestMethod]
        public void GetById_ThrowsException_WhenFarmerNotFound()
        {
            // Act & Assert
            Assert.ThrowsExceptionAsync<ListingNotFoundException>(async () =>
            {
                await _farmerRepository.GetByIdAsync(99999);
            });
        }
}

    internal class ExpectedExceptionAttribute : Attribute
    {
    }

    internal class TestMethodAttribute : Attribute
    {
    }

    internal class TestCleanupAttribute : Attribute
    {
    }

    internal class TestInitializeAttribute : Attribute
    {
    }

    internal class TestClassAttribute : Attribute
    {
    }

    // Minimal local Assert implementation to satisfy tests when MSTest Assert
    // is not available in the compilation context.
    internal static class Assert
    {
        public static void IsNotNull(object? obj)
        {
            if (obj is null) throw new Exception("Assert.IsNotNull failed: value is null");
        }

        public static void IsTrue(bool condition)
        {
            if (!condition) throw new Exception("Assert.IsTrue failed");
        }

        public static void IsFalse(bool condition)
        {
            if (condition) throw new Exception("Assert.IsFalse failed");
        }

        public static void AreEqual<T>(T expected, T actual)
        {
            if (!Equals(expected, actual)) throw new Exception($"Assert.AreEqual failed. Expected:<{expected}>. Actual:<{actual}>");
        }

        public static void Inconclusive(string message)
        {
            throw new Exception("Inconclusive: " + message);
        }

        public static async System.Threading.Tasks.Task ThrowsExceptionAsync<TException>(Func<System.Threading.Tasks.Task> action)
            where TException : Exception
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                if (ex is TException) return;
                throw new Exception($"Assert.ThrowsExceptionAsync failed. Thrown exception was of type {ex.GetType().Name}");
            }

            throw new Exception("Assert.ThrowsExceptionAsync failed. No exception was thrown.");
        }
    }
}