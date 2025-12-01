// <copyright file="CompatibilityServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Services;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="CompatibilityService"/> class.
    /// </summary>
    public class CompatibilityServiceTests
    {
        /// <summary>
        /// Tests that validation returns true when elements are compatible.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ValidateElementPlacementAsync_ReturnsTrue_WhenElementsCompatible()
        {
            // Arrange
            var newElement = new MapElementDTO
            {
                Type = "tree",
                ElementId = 1,
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var existingElements = new List<MapElementDTO>
            {
                new MapElementDTO
                {
                    Type = "tree",
                    ElementId = 2,
                    X = 100,
                    Y = 100,
                    Width = 50,
                    Height = 50,
                    Rotation = 0,
                },
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.AreTreesIncompatibleAsync(1, 2)).ReturnsAsync(false);

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.ValidateElementPlacementAsync(newElement, existingElements);

            // Assert
            Assert.True(result.Success);

            // Fix xUnit2002/CS1061: Перевіряємо булеве значення напряму
            Assert.True(result.Data);
        }

        /// <summary>
        /// Tests that validation returns an error when trees are incompatible.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
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
                Height = 50,
                Rotation = 0,
            };

            var existingElement = new MapElementDTO
            {
                Type = "tree",
                ElementId = 2,
                X = 2,
                Y = 2,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.AreTreesIncompatibleAsync(1, 2)).ReturnsAsync(true);

            repoMock.Setup(r => r.GetTreeSortsAsync()).ReturnsAsync(new List<TreeSort>
            {
                new TreeSort
                {
                    Id = 1,
                    Name = "Apple",
                    GroundType = "Universal",
                    TreeId = 1,
                    Tree = new Tree { Id = 1, Name = "Apple", Image = "apple.png" },
                },
                new TreeSort
                {
                    Id = 2,
                    Name = "Walnut",
                    GroundType = "Universal",
                    TreeId = 2,
                    Tree = new Tree { Id = 2, Name = "Walnut", Image = "walnut.png" },
                },
            });

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.ValidateElementPlacementAsync(newElement, new List<MapElementDTO> { existingElement });

            // Assert
            Assert.False(result.Success, $"Очікувалось false, а отримали: {result.Success}");
            Assert.NotNull(result.Error);
            Assert.Contains("несумісні", result.Error);
        }

        /// <summary>
        /// Tests that compatibility check returns true for different element types.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CheckCompatibilityAsync_ReturnsCompatible_WhenDifferentTypes()
        {
            // Arrange
            var element1 = new MapElementDTO
            {
                Type = "tree",
                ElementId = 1,
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var element2 = new MapElementDTO
            {
                Type = "veg",
                ElementId = 1,
                X = 10,
                Y = 10,
                Width = 30,
                Height = 30,
                Rotation = 0,
            };

            var repoMock = new Mock<IMapRepository>();
            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.CheckCompatibilityAsync(element1, element2);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.True(result.Data!.IsCompatible);
            Assert.Equal("Різні типи рослин", result.Data.Message);
        }

        /// <summary>
        /// Tests that compatibility check returns true when trees are far apart.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CheckCompatibilityAsync_ReturnsCompatible_WhenTreesFarApart()
        {
            // Arrange
            var element1 = new MapElementDTO
            {
                Type = "tree",
                ElementId = 1,
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var element2 = new MapElementDTO
            {
                Type = "tree",
                ElementId = 2,
                X = 500,
                Y = 500,
                Width = 50,
                Height = 50,
                Rotation = 0,
            };

            var repoMock = new Mock<IMapRepository>();

            repoMock.Setup(r => r.GetTreeSortsAsync()).ReturnsAsync(new List<TreeSort>
            {
                new TreeSort
                {
                    Id = 1,
                    Name = "Apple",
                    GroundType = "Universal",
                    TreeId = 1,
                    Tree = new Tree { Id = 1, Name = "Apple", Image = "apple.png" },
                },
                new TreeSort
                {
                    Id = 2,
                    Name = "Pear",
                    GroundType = "Universal",
                    TreeId = 2,
                    Tree = new Tree { Id = 2, Name = "Pear", Image = "pear.png" },
                },
            });

            var service = new CompatibilityService(repoMock.Object);

            // Act
            var result = await service.CheckCompatibilityAsync(element1, element2);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.True(result.Data!.IsCompatible);
            Assert.Contains("не знаходяться у радіусі", result.Data.Message);
        }

        /// <summary>
        /// Tests that getting incompatible tree IDs returns the correct list.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
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
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data!.Count);
            Assert.Contains(2L, result.Data);
        }

        /// <summary>
        /// Tests that getting incompatible vegetable IDs returns the correct list.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
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
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data!.Count);
            Assert.Contains(2L, result.Data);
        }
    }
}