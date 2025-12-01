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

        /// <summary>
        /// Initializes a new instance of the <see cref="MapService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data access.</param>
        /// <param name="mapper">The object mapper.</param>
        public MapService(IMapRepository mapRepository, IMapper mapper)
        {
            this.mapRepository = mapRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId)
        {
            try
            {
                var map = await this.mapRepository.GetByIdAsync(mapId);
                return map == null
                    ? BaseResponseDTO<MapDTO>.Fail("Мапу не знайдено")
                    : BaseResponseDTO<MapDTO>.Ok(this.mapper.Map<MapDTO>(map));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return BaseResponseDTO<List<MapShortDTO>>.Fail("Помилка при отриманні списку карт");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync()
        {
            try
            {
                var treeSorts = await this.mapRepository.GetTreeSortsAsync();
                var vegSorts = await this.mapRepository.GetVegSortsAsync();
                var flowers = await this.mapRepository.GetFlowersAsync();

                var mapElements = new MapElementsDTO
                {
                    TreeSorts = this.mapper.Map<List<TreeSortDTO>>(treeSorts),
                    VegSorts = this.mapper.Map<List<VegSortDTO>>(vegSorts),
                    Flowers = this.mapper.Map<List<FlowerDTO>>(flowers),
                };

                return BaseResponseDTO<MapElementsDTO>.Ok(mapElements);
            }
            catch (Exception)
            {
                return BaseResponseDTO<MapElementsDTO>.Fail("Помилка при отриманні елементів мапи");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto)
        {
            try
            {
                var map = this.mapper.Map<Map>(createMapDto);
                var mapId = await this.mapRepository.CreateAsync(map);
                return BaseResponseDTO<long>.Ok(mapId);
            }
            catch (Exception)
            {
                return BaseResponseDTO<long>.Fail("Помилка при створенні мапи");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto)
        {
            try
            {
                var existingMap = await this.mapRepository.GetByIdAsync(updateMapDto.Id);
                if (existingMap == null)
                {
                    return BaseResponseDTO<bool>.Fail("Мапу не знайдено");
                }

                this.mapper.Map(updateMapDto, existingMap);
                var result = await this.mapRepository.UpdateAsync(existingMap);

                return BaseResponseDTO<bool>.Ok(result);
            }
            catch (Exception)
            {
                return BaseResponseDTO<bool>.Fail("Помилка при оновленні мапи");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> DeleteMapAsync(long id)
        {
            try
            {
                var result = await this.mapRepository.DeleteAsync(id);
                return result
                    ? BaseResponseDTO<bool>.Ok(true)
                    : BaseResponseDTO<bool>.Fail("Мапу не знайдено для видалення");
            }
            catch (Exception)
            {
                return BaseResponseDTO<bool>.Fail("Помилка при видаленні мапи");
            }
        }
    }
}