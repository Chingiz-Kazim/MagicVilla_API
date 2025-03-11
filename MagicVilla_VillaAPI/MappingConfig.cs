using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>();
        CreateMap<VillaDTO, Villa>();

        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();


        CreateMap<VillaNumber, VillaNoDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNoUpdatedDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNoCreateDTO>().ReverseMap();
    }
}
