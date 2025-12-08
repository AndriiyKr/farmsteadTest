namespace FarmsteadMap.BLL.Profiles
{
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Data.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<User, UserDTO>().ReverseMap();

            // --- MAP MAPPINGS ---
            this.CreateMap<Map, MapDTO>();
            this.CreateMap<Map, MapShortDTO>();

            // Створення мапи (Create)
            this.CreateMap<CreateMapDTO, Map>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom(src => src.MapJson));

            // Оновлення мапи (Update)
            this.CreateMap<UpdateMapDTO, Map>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom(src => src.MapJson));

            // --- РОСЛИНИ (Plants) ---

            // Дерева
            this.CreateMap<Tree, BasePlantDTO>();

            // Овочі (так як в таблиці Vegetables немає поля Image, мапимо тільки Id та Name)
            // Картинку можна задати дефолтну або обробляти на клієнті
            this.CreateMap<Vegetable, BasePlantDTO>()
                 .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Квіти
            this.CreateMap<Flower, FlowerDTO>();
            CreateMap<TreeSort, SortDTO>();
            CreateMap<VegSort, SortDTO>();
            // --- СОРТИ ---

            // TreeSort mappings
            this.CreateMap<TreeSort, TreeSortDTO>()
                .ForMember(dest => dest.TreeName, opt => opt.MapFrom(src => src.Tree.Name))
                .ForMember(dest => dest.TreeImage, opt => opt.MapFrom(src => src.Tree.Image));

            // VegSort mappings
            this.CreateMap<VegSort, VegSortDTO>()
                .ForMember(dest => dest.VegetableName, opt => opt.MapFrom(src => src.Vegetable.Name));
        }
    }
}