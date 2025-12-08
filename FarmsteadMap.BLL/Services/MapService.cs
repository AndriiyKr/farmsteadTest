// <copyright file="MapService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;

    /// <summary>
    /// Service responsible for managing map data operations.
    /// </summary>
    public class MapService : IMapService
    {
        private readonly IMapRepository mapRepository;
        private readonly IMapper mapper;
        private readonly ILoggerService logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data access.</param>
        /// <param name="mapper">The object mapper.</param>
        public MapService(IMapRepository mapRepository, IMapper mapper, ILoggerService logger)
        {
            this.mapRepository = mapRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId)
        {
            try
            {
                var map = await this.mapRepository.GetByIdAsync(mapId);
                if (map == null)
                {
                    this.logger.LogWarning($"Мапу {mapId} не знайдено.");
                    return BaseResponseDTO<MapDTO>.Fail("Мапу не знайдено");
                }

                return BaseResponseDTO<MapDTO>.Ok(this.mapper.Map<MapDTO>(map));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка при отриманні мапи {mapId}", ex);
                return BaseResponseDTO<MapDTO>.Fail("Помилка при отриманні мапи");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId)
        {
            try
            {
                var userMaps = await this.mapRepository.GetByUserIdAsync(userId);
                return BaseResponseDTO<List<MapShortDTO>>.Ok(this.mapper.Map<List<MapShortDTO>>(userMaps));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка при отриманні списку карт", ex);
                return BaseResponseDTO<List<MapShortDTO>>.Fail("Помилка при отриманні списку карт");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync()
        {
            try
            {
                var trees = await this.mapRepository.GetTreesAsync();
                var vegs = await this.mapRepository.GetVegetablesAsync();
                var flowers = await this.mapRepository.GetFlowersAsync();

                var treeInc = await this.mapRepository.GetTreeIncompatibilitiesAsync();
                var vegInc = await this.mapRepository.GetVegIncompatibilitiesAsync();

                var dto = new MapElementsDTO
                {
                    Trees = this.mapper.Map<List<BasePlantDTO>>(trees),
                    Vegetables = this.mapper.Map<List<BasePlantDTO>>(vegs),
                    Flowers = this.mapper.Map<List<FlowerDTO>>(flowers),

                    TreeIncompatibilities = treeInc.Select(x => new IncompatibilityDTO { Id1 = x.Tree1Id, Id2 = x.Tree2Id }).ToList(),
                    VegIncompatibilities = vegInc.Select(x => new IncompatibilityDTO { Id1 = x.Veg1Id, Id2 = x.Veg2Id }).ToList()
                };

                return BaseResponseDTO<MapElementsDTO>.Ok(dto);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error getting map elements", ex);
                return BaseResponseDTO<MapElementsDTO>.Fail("Помилка завантаження елементів");
            }
        }

        public async Task<BaseResponseDTO<List<SortDTO>>> GetTreeSortsAsync(long treeId)
        {
            try
            {
                // ВИПРАВЛЕНО: передаємо treeId у репозиторій
                var sorts = await this.mapRepository.GetTreeSortsAsync(treeId);

                // Фільтрація тут більше не потрібна, репозиторій поверне тільки потрібні
                return BaseResponseDTO<List<SortDTO>>.Ok(this.mapper.Map<List<SortDTO>>(sorts));
            }
            catch (Exception ex) { return BaseResponseDTO<List<SortDTO>>.Fail(ex.Message); }
        }

        public async Task<BaseResponseDTO<List<SortDTO>>> GetVegSortsAsync(long vegId)
        {
            try
            {
                // ВИПРАВЛЕНО: передаємо vegId у репозиторій
                var sorts = await this.mapRepository.GetVegSortsAsync(vegId);

                return BaseResponseDTO<List<SortDTO>>.Ok(this.mapper.Map<List<SortDTO>>(sorts));
            }
            catch (Exception ex) { return BaseResponseDTO<List<SortDTO>>.Fail(ex.Message); }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto)
        {
            try
            {
                var map = this.mapper.Map<Map>(createMapDto);

                // --- ВИПРАВЛЕННЯ: Явно присвоюємо UserId ---
                // Це гарантує, що ID не буде 0, якщо мапер його пропустив
                map.UserId = createMapDto.UserId;
                // -------------------------------------------

                // Якщо JSON не передали, створюємо дефолтну пусту мапу
                if (string.IsNullOrEmpty(map.MapJson))
                {
                    map.MapJson = "{\"viewport\":{\"zoom\":1.0,\"offsetX\":0,\"offsetY\":0},\"landscape\":[],\"objects\":[]}";
                }

                var mapId = await this.mapRepository.CreateAsync(map);
                this.logger.LogEvent($"Створено нову мапу ID: {mapId}, Назва: {createMapDto.Name}, UserID: {map.UserId}");
                return BaseResponseDTO<long>.Ok(mapId);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка при створенні мапи (UserId={createMapDto.UserId})", ex);
                return BaseResponseDTO<long>.Fail($"Помилка при створенні мапи: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        // ... imports
        public async Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto)
        {
            try
            {
                var existingMap = await this.mapRepository.GetByIdAsync(updateMapDto.Id);
                if (existingMap == null)
                {
                    return BaseResponseDTO<bool>.Fail("Map not found");
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateMapDto.Name))
                    existingMap.Name = updateMapDto.Name;

                // Explicitly update the JSON data
                if (!string.IsNullOrEmpty(updateMapDto.MapJson))
                    existingMap.MapJson = updateMapDto.MapJson;

                if (updateMapDto.IsPrivate.HasValue)
                    existingMap.IsPrivate = updateMapDto.IsPrivate.Value;

                var result = await this.mapRepository.UpdateAsync(existingMap);
                return BaseResponseDTO<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error updating map {updateMapDto.Id}", ex);
                return BaseResponseDTO<bool>.Fail("Error saving map");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> DeleteMapAsync(long id)
        {
            try
            {
                var result = await this.mapRepository.DeleteAsync(id);
                if (result)
                {
                    this.logger.LogEvent($"Мапу {id} успішно видалено.");
                    return BaseResponseDTO<bool>.Ok(true);
                }
                else
                {
                    this.logger.LogWarning($"Спроба видалити неіснуючу мапу {id}.");
                    return BaseResponseDTO<bool>.Fail("Мапу не знайдено для видалення");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка при видаленні мапи {id}", ex);
                return BaseResponseDTO<bool>.Fail("Помилка при видаленні мапи");
            }
        }
    }
}