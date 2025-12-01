<<<<<<< HEAD
﻿// <copyright file="MapService.cs" company="PlaceholderCompany">
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
=======
﻿using AutoMapper;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.DAL.Data.Models;
using FarmsteadMap.DAL.Repositories;

namespace FarmsteadMap.BLL.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;
        private readonly IMapper _mapper;

        public MapService(IMapRepository mapRepository, IMapper mapper)
        {
            _mapRepository = mapRepository;
            _mapper = mapper;
        }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId)
        {
            try
            {
<<<<<<< HEAD
                var map = await this.mapRepository.GetByIdAsync(mapId);
                return map == null
                    ? BaseResponseDTO<MapDTO>.Fail("Мапу не знайдено")
                    : BaseResponseDTO<MapDTO>.Ok(this.mapper.Map<MapDTO>(map));
            }
            catch (Exception)
=======
                var map = await _mapRepository.GetByIdAsync(mapId);
                return map == null
                    ? BaseResponseDTO<MapDTO>.Fail("Мапу не знайдено")
                    : BaseResponseDTO<MapDTO>.Ok(_mapper.Map<MapDTO>(map));
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<MapDTO>.Fail("Помилка при отриманні мапи");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId)
        {
            try
            {
<<<<<<< HEAD
                var userMaps = await this.mapRepository.GetByUserIdAsync(userId);
                return BaseResponseDTO<List<MapShortDTO>>.Ok(this.mapper.Map<List<MapShortDTO>>(userMaps));
            }
            catch (Exception)
=======
                var userMaps = await _mapRepository.GetByUserIdAsync(userId);
                return BaseResponseDTO<List<MapShortDTO>>.Ok(_mapper.Map<List<MapShortDTO>>(userMaps));
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<List<MapShortDTO>>.Fail("Помилка при отриманні списку карт");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync()
        {
            try
            {
<<<<<<< HEAD
                var treeSorts = await this.mapRepository.GetTreeSortsAsync();
                var vegSorts = await this.mapRepository.GetVegSortsAsync();
                var flowers = await this.mapRepository.GetFlowersAsync();

                var mapElements = new MapElementsDTO
                {
                    TreeSorts = this.mapper.Map<List<TreeSortDTO>>(treeSorts),
                    VegSorts = this.mapper.Map<List<VegSortDTO>>(vegSorts),
                    Flowers = this.mapper.Map<List<FlowerDTO>>(flowers),
=======
                var treeSorts = await _mapRepository.GetTreeSortsAsync();
                var vegSorts = await _mapRepository.GetVegSortsAsync();
                var flowers = await _mapRepository.GetFlowersAsync();

                var mapElements = new MapElementsDTO
                {
                    TreeSorts = _mapper.Map<List<TreeSortDTO>>(treeSorts),
                    VegSorts = _mapper.Map<List<VegSortDTO>>(vegSorts),
                    Flowers = _mapper.Map<List<FlowerDTO>>(flowers)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                };

                return BaseResponseDTO<MapElementsDTO>.Ok(mapElements);
            }
<<<<<<< HEAD
            catch (Exception)
=======
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<MapElementsDTO>.Fail("Помилка при отриманні елементів мапи");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto)
        {
            try
            {
<<<<<<< HEAD
                var map = this.mapper.Map<Map>(createMapDto);
                var mapId = await this.mapRepository.CreateAsync(map);
                return BaseResponseDTO<long>.Ok(mapId);
            }
            catch (Exception)
=======
                var map = _mapper.Map<Map>(createMapDto);
                var mapId = await _mapRepository.CreateAsync(map);
                return BaseResponseDTO<long>.Ok(mapId);
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<long>.Fail("Помилка при створенні мапи");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto)
        {
            try
            {
<<<<<<< HEAD
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
=======
                var existingMap = await _mapRepository.GetByIdAsync(updateMapDto.Id);
                if (existingMap == null)
                    return BaseResponseDTO<bool>.Fail("Мапу не знайдено");

                _mapper.Map(updateMapDto, existingMap);
                var result = await _mapRepository.UpdateAsync(existingMap);

                return BaseResponseDTO<bool>.Ok(result);
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<bool>.Fail("Помилка при оновленні мапи");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<bool>> DeleteMapAsync(long id)
        {
            try
            {
<<<<<<< HEAD
                var result = await this.mapRepository.DeleteAsync(id);
=======
                var result = await _mapRepository.DeleteAsync(id);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                return result
                    ? BaseResponseDTO<bool>.Ok(true)
                    : BaseResponseDTO<bool>.Fail("Мапу не знайдено для видалення");
            }
<<<<<<< HEAD
            catch (Exception)
=======
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<bool>.Fail("Помилка при видаленні мапи");
            }
        }
    }
}