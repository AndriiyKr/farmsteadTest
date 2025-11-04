using Xunit;
using Moq;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Data.Models;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Tests.Services
{
    public class CompatibilityServiceTests
    {
        [Fact]
        public async Task ValidateElementPlacementAsync_ReturnsTrue_WhenElementsCompatible()
        {
            // Arrange
            var newElement = new MapElementDTO { Type = "tree", ElementId = 1, X = 0, Y = 0, Width = 50, Height = 50 };
            var existingElements = new List<MapElementDTO>
            {
                new MapElementDTO { Type = "tree", ElementId = 2, X = 100, Y = 100, Width = 50, Height = 50 }
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.AreTreesIncompatibleAsync(1, 2)).ReturnsAsync(false);

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.ValidateElementPlacementAsync(newElement, existingElements);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ValidateElementPlacementAsync_ReturnsError_WhenTreesIncompatible()
        {
            // Arrange
            var newElement = new MapElementDTO
            {
                Type = "tree",
                ElementId = 1,
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50
            };
            var existingElement = new MapElementDTO
            {
                Type = "tree",
                ElementId = 2,
                X = 2,
                Y = 2,
                Width = 50,
                Height = 50
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.AreTreesIncompatibleAsync(1, 2)).ReturnsAsync(true);
            repoMock.Setup(r => r.GetTreeSortsAsync()).ReturnsAsync(new List<TreeSort>
    {
        new TreeSort { Id = 1, Name = "Apple" },
        new TreeSort { Id = 2, Name = "Walnut" }
    });

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.ValidateElementPlacementAsync(newElement, new List<MapElementDTO> { existingElement });

            // ДЕТАЛЬНА ДІАГНОСТИКА:
            Console.WriteLine($"=== ДІАГНОСТИКА ТЕСТУ ===");
            Console.WriteLine($"Success: {result.Success}");
            Console.WriteLine($"Data: {result.Data}");
            Console.WriteLine($"Error: {result.Error ?? "NULL"}");
            Console.WriteLine($"=========================");

            // Assert
            Assert.False(result.Success, $"Очікувалось false, а отримали: {result.Success}");
            Assert.NotNull(result.Error);
            Assert.Contains("несумісні", result.Error);
        }

        [Fact]
        public async Task CheckCompatibilityAsync_ReturnsCompatible_WhenDifferentTypes()
        {
            // Arrange
            var element1 = new MapElementDTO { Type = "tree", ElementId = 1, X = 0, Y = 0, Width = 50, Height = 50 };
            var element2 = new MapElementDTO { Type = "veg", ElementId = 1, X = 10, Y = 10, Width = 30, Height = 30 };

            var repoMock = new Mock<IMapRepository>();
            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.CheckCompatibilityAsync(element1, element2);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data.IsCompatible);
            Assert.Equal("Різні типи рослин", result.Data.Message);
        }

        [Fact]
        public async Task CheckCompatibilityAsync_ReturnsCompatible_WhenTreesFarApart()
        {
            // Arrange
            var element1 = new MapElementDTO { Type = "tree", ElementId = 1, X = 0, Y = 0, Width = 50, Height = 50 };
            var element2 = new MapElementDTO { Type = "tree", ElementId = 2, X = 500, Y = 500, Width = 50, Height = 50 };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetTreeSortsAsync()).ReturnsAsync(new List<TreeSort>
            {
                new TreeSort { Id = 1, Name = "Apple" },
                new TreeSort { Id = 2, Name = "Pear" }
            });

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.CheckCompatibilityAsync(element1, element2);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data.IsCompatible);
            Assert.Contains("не знаходяться у радіусі", result.Data.Message);
        }

        [Fact]
        public async Task GetIncompatibleTreeIdsAsync_ReturnsList_WhenSuccessful()
        {
            // Arrange
            var treeId = 1L;
            var incompatibleIds = new List<long> { 2, 3, 4 };
            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetIncompatibleTreeIdsAsync(treeId)).ReturnsAsync(incompatibleIds);

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.GetIncompatibleTreeIdsAsync(treeId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(3, result.Data.Count);
            Assert.Contains(2L, result.Data);
        }

        [Fact]
        public async Task GetIncompatibleVegIdsAsync_ReturnsList_WhenSuccessful()
        {
            // Arrange
            var vegId = 1L;
            var incompatibleIds = new List<long> { 2, 3 };
            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetIncompatibleVegIdsAsync(vegId)).ReturnsAsync(incompatibleIds);

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.GetIncompatibleVegIdsAsync(vegId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(2L, result.Data);
        }
    }
}