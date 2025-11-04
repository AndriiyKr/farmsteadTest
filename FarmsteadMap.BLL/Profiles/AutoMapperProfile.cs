using AutoMapper;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.DAL.Data.Models;

namespace FarmsteadMap.BLL.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            // Map mappings
            CreateMap<Map, MapDTO>();
            CreateMap<Map, MapShortDTO>();

            CreateMap<CreateMapDTO, Map>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom((src, dest) =>
                    System.Text.Json.JsonSerializer.Serialize(src.MapData)));

            CreateMap<UpdateMapDTO, Map>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom((src, dest) =>
                    System.Text.Json.JsonSerializer.Serialize(src.MapData)));

            // TreeSort mappings
            CreateMap<TreeSort, TreeSortDTO>()
                .ForMember(dest => dest.TreeName, opt => opt.MapFrom(src => src.Tree.Name))
                .ForMember(dest => dest.TreeImage, opt => opt.MapFrom(src => src.Tree.Image));

            // VegSort mappings
            CreateMap<VegSort, VegSortDTO>()
                .ForMember(dest => dest.VegetableName, opt => opt.MapFrom(src => src.Vegetable.Name));

            // Flower mappings
            CreateMap<Flower, FlowerDTO>();
        }
    }
}