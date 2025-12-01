<<<<<<< HEAD
// <copyright file="AutoMapperProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Profiles
{
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Data.Models;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between entity models and data transfer objects.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// Configures the object mappings.
        /// </summary>
        public AutoMapperProfile()
        {
            this.CreateMap<User, UserDTO>().ReverseMap();

            // Map mappings
            this.CreateMap<Map, MapDTO>();
            this.CreateMap<Map, MapShortDTO>();

            this.CreateMap<CreateMapDTO, Map>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom((src, dest) =>
                    System.Text.Json.JsonSerializer.Serialize(src.MapData, (System.Text.Json.JsonSerializerOptions?)null)));

            this.CreateMap<UpdateMapDTO, Map>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.MapJson, opt => opt.MapFrom((src, dest) =>
                    System.Text.Json.JsonSerializer.Serialize(src.MapData, (System.Text.Json.JsonSerializerOptions?)null)));

            // TreeSort mappings
            this.CreateMap<TreeSort, TreeSortDTO>()
=======
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .ForMember(dest => dest.TreeName, opt => opt.MapFrom(src => src.Tree.Name))
                .ForMember(dest => dest.TreeImage, opt => opt.MapFrom(src => src.Tree.Image));

            // VegSort mappings
<<<<<<< HEAD
            this.CreateMap<VegSort, VegSortDTO>()
                .ForMember(dest => dest.VegetableName, opt => opt.MapFrom(src => src.Vegetable.Name));

            // Flower mappings
            this.CreateMap<Flower, FlowerDTO>();
=======
            CreateMap<VegSort, VegSortDTO>()
                .ForMember(dest => dest.VegetableName, opt => opt.MapFrom(src => src.Vegetable.Name));

            // Flower mappings
            CreateMap<Flower, FlowerDTO>();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }
    }
}