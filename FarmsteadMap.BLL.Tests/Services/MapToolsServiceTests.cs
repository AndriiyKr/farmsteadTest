using Xunit;
using Moq;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;

namespace FarmsteadMap.BLL.Tests.Services
{
    public class MapToolsServiceTests
    {
        // Тести для дерев (вже є)
        [Fact]
        public void AddTreeElement_CreatesElementWithCorrectProperties()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddTreeElement(1, 10.5, 20.5);

            // Assert
            Assert.Equal("tree", result.Type);
            Assert.Equal(1L, result.ElementId);
            Assert.Equal(10.5, result.X);
            Assert.Equal(20.5, result.Y);
            Assert.Equal(50, result.Width);
            Assert.Equal(50, result.Height);
        }

        // НОВІ ТЕСТИ ДЛЯ ОВОЧІВ
        [Fact]
        public void AddVegetableElement_CreatesElementWithCorrectProperties()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddVegetableElement(1, 15.0, 25.0);

            // Assert
            Assert.Equal("veg", result.Type);
            Assert.Equal(1L, result.ElementId);
            Assert.Equal(15.0, result.X);
            Assert.Equal(25.0, result.Y);
            Assert.Equal(30, result.Width);
            Assert.Equal(30, result.Height);
        }

        // НОВІ ТЕСТИ ДЛЯ КВІТІВ
        [Fact]
        public void AddFlowerElement_CreatesElementWithCorrectProperties()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddFlowerElement(1, 5.0, 5.0);

            // Assert
            Assert.Equal("flower", result.Type);
            Assert.Equal(1L, result.ElementId);
            Assert.Equal(5.0, result.X);
            Assert.Equal(5.0, result.Y);
            Assert.Equal(20, result.Width);
            Assert.Equal(20, result.Height);
        }

        // Тести для валідації овочів
        [Fact]
        public async Task AddVegetableElementWithValidation_ReturnsElement_WhenValidationPasses()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            compatMock.Setup(c => c.ValidateElementPlacementAsync(It.IsAny<MapElementDTO>(), It.IsAny<List<MapElementDTO>>()))
                     .ReturnsAsync(new BaseResponseDTO<bool> { Success = true, Data = true });

            var service = new MapToolsService(repoMock.Object, compatMock.Object);
            var existingElements = new List<MapElementDTO>();

            // Act
            var result = await service.AddVegetableElementWithValidation(1, 15, 25, existingElements);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("veg", result.Data.Type);
        }

        [Fact]
        public async Task AddVegetableElementWithValidation_ReturnsError_WhenValidationFails()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            compatMock.Setup(c => c.ValidateElementPlacementAsync(It.IsAny<MapElementDTO>(), It.IsAny<List<MapElementDTO>>()))
                     .ReturnsAsync(new BaseResponseDTO<bool> { Success = false, Error = "Несумісні овочі" });

            var service = new MapToolsService(repoMock.Object, compatMock.Object);
            var existingElements = new List<MapElementDTO>();

            // Act
            var result = await service.AddVegetableElementWithValidation(1, 15, 25, existingElements);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Несумісні овочі", result.Error);
        }

        // Тести для вимірювань
        [Fact]
        public void AddMeasurement_CreatesLineWithCorrectProperties()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddMeasurement(0, 0, 10, 10);

            // Assert
            Assert.Equal(0, result.StartX);
            Assert.Equal(0, result.StartY);
            Assert.Equal(10, result.EndX);
            Assert.Equal(10, result.EndY);
            Assert.True(result.Length > 0); // Довжина має бути позитивною
        }

        // Тести для грядок
        [Fact]
        public void AddGardenBed_CreatesBedWithCorrectProperties()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddGardenBed(10, 20, 100, 50, "rectangle");

            // Assert
            Assert.Equal(10, result.X);
            Assert.Equal(20, result.Y);
            Assert.Equal(100, result.Width);
            Assert.Equal(50, result.Height);
            Assert.Equal("rectangle", result.Shape);
        }

        [Fact]
        public void AddGardenBed_CreatesBedWithDefaultShape()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddGardenBed(10, 20, 100, 50); // Без вказання shape

            // Assert
            Assert.Equal("rectangle", result.Shape); // Має бути значення за замовчуванням
        }

        // Тести для перевірки перекриття
        [Fact]
        public void ValidateElementPlacement_ReturnsError_WhenOverlapExists()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            var element = new MapElementDTO { X = 0, Y = 0, Width = 50, Height = 50 };
            var existingElements = new List<MapElementDTO>
            {
                new MapElementDTO { X = 25, Y = 25, Width = 50, Height = 50 } // Перекриваються
            };

            // Act
            var result = service.ValidateElementPlacement(element, existingElements);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Елементи перекриваються", result.Error);
        }

        // Тести для різних типів елементів
        [Fact]
        public void ValidateElementPlacement_ReturnsTrue_WhenDifferentTypesNoOverlap()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            var treeElement = new MapElementDTO { Type = "tree", X = 0, Y = 0, Width = 50, Height = 50 };
            var vegElement = new MapElementDTO { Type = "veg", X = 60, Y = 60, Width = 30, Height = 30 };

            var existingElements = new List<MapElementDTO> { treeElement };

            // Act
            var result = service.ValidateElementPlacement(vegElement, existingElements);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        // Існуючі тести (залишаються)
        [Fact]
        public void CalculateDistance_ReturnsCorrectDistance()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var distance = service.CalculateDistance(0, 0, 3, 4);

            // Assert
            Assert.Equal(5, distance); // √(3² + 4²) = 5
        }

        [Fact]
        public void CalculateDistance_ReturnsZero_WhenSamePoint()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var distance = service.CalculateDistance(5, 5, 5, 5);

            // Assert
            Assert.Equal(0, distance);
        }
    }
}