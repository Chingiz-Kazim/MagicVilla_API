﻿using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

        CreateMap<VillaNoDTO, VillaNoCreateDTO>().ReverseMap();
        CreateMap<VillaNoDTO, VillaNoUpdatedDTO>().ReverseMap();
    }
}
