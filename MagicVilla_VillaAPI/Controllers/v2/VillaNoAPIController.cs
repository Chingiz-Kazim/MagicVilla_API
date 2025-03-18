using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/VillaNoAPI")]
[ApiController]
[ApiVersion("2.0")]
public class VillaNoAPIController : ControllerBase
{
    private readonly IVillaNoRepository _villaNoRepository;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaNoAPIController(IVillaNoRepository villaNoRepository, IMapper mapper, IVillaRepository dbVilla)
    {
        _villaNoRepository = villaNoRepository;
        _mapper = mapper;
        _response = new();
        _dbVilla = dbVilla;
    }

    //[MapToApiVersion("2.0")]
    [HttpGet("GetString")]
    public IEnumerable<string> Get()
    {
        return new string[] { "Шарашкина", "Контора" };
    }
}
