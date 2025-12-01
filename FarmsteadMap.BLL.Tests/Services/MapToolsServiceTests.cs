// <copyright file="MapToolsServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Services;
    using FarmsteadMap.DAL.Repositories;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MapToolsService"/> class.
    /// </summary>
    public class MapToolsServiceTests
    {
        /// <summary>
        /// Tests that AddTreeElement creates an element with correct properties.
        /// </summary>
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

        /// <summary>
        /// Tests that AddVegetableElement creates an element with correct properties.
        /// </summary>
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

        /// <summary>
        /// Tests that AddFlowerElement creates an element with correct properties.
        /// </summary>
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

        /// <summary>
        /// Tests that AddVegetableElementWithValidation returns the element when validation passes.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
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
            Assert.Equal("veg", result.Data!.Type);
        }

        /// <summary>
        /// Tests that AddVegetableElementWithValidation returns an error when validation fails.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
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

        /// <summary>
        /// Tests that AddMeasurement creates a line with correct properties.
        /// </summary>
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
            Assert.True(result.Length > 0);
        }

        /// <summary>
        /// Tests that AddGardenBed creates a bed with correct properties.
        /// </summary>
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

        /// <summary>
        /// Tests that AddGardenBed creates a bed with the default shape when none is specified.
        /// </summary>
        [Fact]
        public void AddGardenBed_CreatesBedWithDefaultShape()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Act
            var result = service.AddGardenBed(10, 20, 100, 50);

            // Assert
            Assert.Equal("rectangle", result.Shape);
        }

        /// <summary>
        /// Tests that ValidateElementPlacement returns an error when elements overlap.
        /// </summary>
        [Fact]
        public void ValidateElementPlacement_ReturnsError_WhenOverlapExists()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Fix: Added Rotation (required)
            var element = new MapElementDTO
            {
                ElementId = 1,
                Type = "tree",
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var existingElements = new List<MapElementDTO>
            {
                // Fix: Added Rotation (required)
                new MapElementDTO
                {
                    ElementId = 2,
                    Type = "tree",
                    X = 25,
                    Y = 25,
                    Width = 50,
                    Height = 50,
                    Rotation = 0,
                },
            };

            // Act
            var result = service.ValidateElementPlacement(element, existingElements);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Елементи перекриваються", result.Error);
        }

        /// <summary>
        /// Tests that ValidateElementPlacement returns true when different types do not overlap.
        /// </summary>
        [Fact]
        public void ValidateElementPlacement_ReturnsTrue_WhenDifferentTypesNoOverlap()
        {
            // Arrange
            var repoMock = new Mock<IMapRepository>();
            var compatMock = new Mock<ICompatibilityService>();
            var service = new MapToolsService(repoMock.Object, compatMock.Object);

            // Fix: Added Rotation (required)
            var treeElement = new MapElementDTO
            {
                ElementId = 1,
                Type = "tree",
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            // Fix: Added Rotation (required)
            var vegElement = new MapElementDTO
            {
                ElementId = 2,
                Type = "veg",
                X = 60,
                Y = 60,
                Width = 30,
                Height = 30,
                Rotation = 0,
            };

            var existingElements = new List<MapElementDTO> { treeElement };

            // Act
            var result = service.ValidateElementPlacement(vegElement, existingElements);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        /// <summary>
        /// Tests that CalculateDistance returns the correct distance between two points.
        /// </summary>
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
            Assert.Equal(5, distance);
        }

        /// <summary>
        /// Tests that CalculateDistance returns zero when the points are the same.
        /// </summary>
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