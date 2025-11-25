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


    [HttpGet("GetString")]
    public IEnumerable<string> Get()
    {
        return new string[] { "Binx", "Bonne" };
    }

    //[MapToApiVersion("1.0")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillasNumber()
    {
        try
        {
            IEnumerable<VillaNumber> villaList = await _villaNoRepository.GetAllAsync(includeProperties: "Villa");
            _response.Result = _mapper.Map<List<VillaNoDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }


    [HttpGet("{number:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int number)
    {
        try
        {
            if (number == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaNoRepository.GetAsync(v => v.VillaNo == number);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNoDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNoCreateDTO createDTO)
    {
        try
        {
            if (await _villaNoRepository.GetAsync(v => v.VillaNo == createDTO.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa # already Exists!");
                return BadRequest(ModelState);
            }
            if (await _dbVilla.GetAsync(v => v.Id == createDTO.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(createDTO);

            await _villaNoRepository.CreateAsync(model);

            _response.Result = _mapper.Map<VillaNoDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = model.VillaNo }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpDelete("{number:int}", Name = "DeleteVillaNumber")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int number)
    {
        try
        {
            if (number == 0)
            {
                return BadRequest();
            }

            var villa = await _villaNoRepository.GetAsync(v => v.VillaNo == number);
            if (villa == null)
            {
                return NotFound();
            }

            await _villaNoRepository.RemoveAsync(villa);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPut("{number:int}", Name = "UpdateVillaNumber")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int number, [FromBody] VillaNoUpdatedDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || number != updateDTO.VillaNo)
            {
                return BadRequest();
            }
            if (await _dbVilla.GetAsync(v => v.Id == updateDTO.VillaID) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

            await _villaNoRepository.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPatch("{number:int}", Name = "UpdatePartialVillaNumber")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVillaNumber(int number, JsonPatchDocument<VillaNoUpdatedDTO> patchDTO)
    {
        if (patchDTO == null || number == 0)
        {
            return BadRequest();
        }

        var villa = await _villaNoRepository.GetAsync(v => v.VillaNo == number, tracked: false);

        if (villa == null)
        {
            return BadRequest();
        }

        VillaNoUpdatedDTO updatedDTO = _mapper.Map<VillaNoUpdatedDTO>(villa);

        patchDTO.ApplyTo(updatedDTO, ModelState);

        VillaNumber model = _mapper.Map<VillaNumber>(updatedDTO);

        await _villaNoRepository.UpdateAsync(model);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
