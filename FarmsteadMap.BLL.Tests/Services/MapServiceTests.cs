// <copyright file="MapServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Profiles;
    using FarmsteadMap.BLL.Services;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MapService"/> class.
    /// </summary>
    public class MapServiceTests
    {
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapServiceTests"/> class.
        /// </summary>
        public MapServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            this.mapper = config.CreateMapper();
        }

        /// <summary>
        /// Tests that GetMapAsync returns a map when it exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetMapAsync_ReturnsMap_WhenMapExists()
        {
            // Arrange
            var mapId = 1L;
            var map = new Map
            {
                Id = mapId,
                Name = "Test Map",
                UserId = 1,
                MapJson = "{}",
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetByIdAsync(mapId)).ReturnsAsync(map);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.GetMapAsync(mapId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(mapId, result.Data!.Id);
            Assert.Equal("Test Map", result.Data!.Name);
        }

        /// <summary>
        /// Tests that GetMapAsync returns an error when the map is not found.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetMapAsync_ReturnsError_WhenMapNotFound()
        {
            // Arrange
            var mapId = 999L;
            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetByIdAsync(mapId)).ReturnsAsync((Map?)null);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.GetMapAsync(mapId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Мапу не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserMapsAsync returns a list of maps for a user.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserMapsAsync_ReturnsMaps_WhenUserHasMaps()
        {
            // Arrange
            var userId = 1L;
            var maps = new List<Map>
            {
                new Map { Id = 1, Name = "Map 1", UserId = userId, MapJson = "{}" },
                new Map { Id = 2, Name = "Map 2", UserId = userId, MapJson = "{}" },
            };
            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(maps);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.GetUserMapsAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data!.Count);
        }

        /// <summary>
        /// Tests that GetMapElementsAsync returns all map elements correctly mapped.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetMapElementsAsync_ReturnsAllElements()
        {
            // Arrange
            var treeSorts = new List<TreeSort>
            {
                new TreeSort
                {
                    Id = 1,
                    Name = "Apple",
                    GroundType = "Universal",
                    TreeId = 1, // Fix: Added TreeId
                    Tree = new Tree
                    {
                        Id = 1, // Fix: Added Id
                        Name = "Apple Tree",
                        Image = "apple.jpg",
                    },
                },
            };

            var vegSorts = new List<VegSort>
            {
                new VegSort
                {
                    Id = 1,
                    Name = "Tomato",
                    GroundType = "Universal",
                    Image = "tomato.png",
                    VegId = 1, // Fix: Added VegId
                    Vegetable = new Vegetable
                    {
                        Id = 1, // Fix: Added Id
                        Name = "Tomato",
                    },
                },
            };

            var flowers = new List<Flower>
            {
                new Flower
                {
                    Id = 1,
                    Name = "Rose",
                    GroundType = "Universal",
                    Image = "rose.png",
                },
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetTreeSortsAsync()).ReturnsAsync(treeSorts);
            repoMock.Setup(r => r.GetVegSortsAsync()).ReturnsAsync(vegSorts);
            repoMock.Setup(r => r.GetFlowersAsync()).ReturnsAsync(flowers);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.GetMapElementsAsync();

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data!.TreeSorts);
            Assert.Single(result.Data!.VegSorts);
            Assert.Single(result.Data!.Flowers);
        }

        /// <summary>
        /// Tests that CreateMapAsync returns the map ID when creation is successful.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CreateMapAsync_ReturnsMapId_WhenSuccessful()
        {
            // Arrange
            var mapId = 1L;
            var createDto = new CreateMapDTO
            {
                Name = "New Map",
                IsPrivate = false,
                UserId = 1,
                MapData = new MapDataDTO { Name = "New Map" },
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<Map>())).ReturnsAsync(mapId);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.CreateMapAsync(createDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(mapId, result.Data);
        }

        /// <summary>
        /// Tests that UpdateMapAsync returns true when the map is successfully updated.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task UpdateMapAsync_ReturnsTrue_WhenMapExists()
        {
            // Arrange
            // Fix: Added IsPrivate and MapData (required in DTO)
            var updateDto = new UpdateMapDTO
            {
                Id = 1,
                Name = "Updated Map",
                IsPrivate = false,
                MapData = new MapDataDTO { Name = "Updated Map" },
            };

            var existingMap = new Map
            {
                Id = 1,
                Name = "Old Map",
                MapJson = "{}",
                UserId = 1,
            };

            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(existingMap);
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Map>())).ReturnsAsync(true);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.UpdateMapAsync(updateDto);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        /// <summary>
        /// Tests that DeleteMapAsync returns true when the map is successfully deleted.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task DeleteMapAsync_ReturnsTrue_WhenMapDeleted()
        {
            // Arrange
            var mapId = 1L;
            var repoMock = new Mock<IMapRepository>();
            repoMock.Setup(r => r.DeleteAsync(mapId)).ReturnsAsync(true);

            var service = new MapService(repoMock.Object, this.mapper);

            // Act
            var result = await service.DeleteMapAsync(mapId);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
        }
    }
}