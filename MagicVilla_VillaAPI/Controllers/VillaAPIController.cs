﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    private readonly ILogger<VillaAPIController> _logger;

    public VillaAPIController(ILogger<VillaAPIController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.LogInformation("Getting all villas");
        return Ok(VillaStore.villaList);
    }

    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
        if (id == 0)
        {
            _logger.LogError("Get Villa Error with id " + id);
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
    {
        if(VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already Exists!");
            return BadRequest(ModelState);
        }
        if (villaDTO == null)
        {
            return BadRequest(villaDTO);
        }
        if(villaDTO.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        villaDTO.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
        VillaStore.villaList.Add(villaDTO);

        return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        VillaStore.villaList.Remove(villa);

        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
    {
        if (villaDTO == null || id != villaDTO.Id)
        {
            return BadRequest();
        }
        var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        villa.Name = villaDTO.Name;
        villa.Occupancy = villaDTO.Occupancy;
        villa.Sqft = villaDTO.Sqft;

        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
