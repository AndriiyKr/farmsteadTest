using AutoMapper;
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

        public async Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId)
        {
            try
            {
                var map = await _mapRepository.GetByIdAsync(mapId);
                return map == null
                    ? BaseResponseDTO<MapDTO>.Fail("Мапу не знайдено")
                    : BaseResponseDTO<MapDTO>.Ok(_mapper.Map<MapDTO>(map));
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapDTO>.Fail("Помилка при отриманні мапи");
            }
        }

        public async Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId)
        {
            try
            {
                var userMaps = await _mapRepository.GetByUserIdAsync(userId);
                return BaseResponseDTO<List<MapShortDTO>>.Ok(_mapper.Map<List<MapShortDTO>>(userMaps));
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<MapShortDTO>>.Fail("Помилка при отриманні списку карт");
            }
        }

        public async Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync()
        {
            try
            {
                var treeSorts = await _mapRepository.GetTreeSortsAsync();
                var vegSorts = await _mapRepository.GetVegSortsAsync();
                var flowers = await _mapRepository.GetFlowersAsync();

                var mapElements = new MapElementsDTO
                {
                    TreeSorts = _mapper.Map<List<TreeSortDTO>>(treeSorts),
                    VegSorts = _mapper.Map<List<VegSortDTO>>(vegSorts),
                    Flowers = _mapper.Map<List<FlowerDTO>>(flowers)
                };

                return BaseResponseDTO<MapElementsDTO>.Ok(mapElements);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapElementsDTO>.Fail("Помилка при отриманні елементів мапи");
            }
        }

        public async Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto)
        {
            try
            {
                var map = _mapper.Map<Map>(createMapDto);
                var mapId = await _mapRepository.CreateAsync(map);
                return BaseResponseDTO<long>.Ok(mapId);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<long>.Fail("Помилка при створенні мапи");
            }
        }

        public async Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto)
        {
            try
            {
                var existingMap = await _mapRepository.GetByIdAsync(updateMapDto.Id);
                if (existingMap == null)
                    return BaseResponseDTO<bool>.Fail("Мапу не знайдено");

                _mapper.Map(updateMapDto, existingMap);
                var result = await _mapRepository.UpdateAsync(existingMap);

                return BaseResponseDTO<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<bool>.Fail("Помилка при оновленні мапи");
            }
        }

        public async Task<BaseResponseDTO<bool>> DeleteMapAsync(long id)
        {
            try
            {
                var result = await _mapRepository.DeleteAsync(id);
                return result
                    ? BaseResponseDTO<bool>.Ok(true)
                    : BaseResponseDTO<bool>.Fail("Мапу не знайдено для видалення");
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<bool>.Fail("Помилка при видаленні мапи");
            }
        }
    }
}