using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public VillaAPIController(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
        return Ok(_mapper.Map<List<VillaDTO>>(villaList));
    }

    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<VillaDTO>(villa));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO)
    {
        if(await _db.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already Exists!");
            return BadRequest(ModelState);
        }
        if (createDTO == null)
        {
            return BadRequest(createDTO);
        }
        //if(villaDTO.Id > 0)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}

        Villa model = _mapper.Map<Villa>(createDTO);
        //автоматически преобразует вместо того что было ниже
        //Villa model = new Villa()
        //{
        //    Name = createDTO.Name,
        //    Details = createDTO.Details,
        //    Amenity = createDTO.Amenity,
        //    ImageURL = createDTO.ImageURL,
        //    Occupancy = createDTO.Occupancy,
        //    Rate = createDTO.Rate,
        //    Sqft = createDTO.Sqft,
        //    CreatedDate = DateTime.Now
        //};

        await _db.Villas.AddAsync(model);
        await _db.SaveChangesAsync();

        return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        _db.Villas.Remove(villa);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
    {
        if (updateDTO == null || id != updateDTO.Id)
        {
            return BadRequest();
        }

        Villa model = _mapper.Map<Villa>(updateDTO);
        //автоматически преобразует вместо того что было ниже
        //Villa model = new Villa()
        //{
        //    Id = updateDTO.Id,
        //    Name = updateDTO.Name,
        //    Details = updateDTO.Details,
        //    Amenity = updateDTO.Amenity,
        //    ImageURL = updateDTO.ImageURL,
        //    Occupancy = updateDTO.Occupancy,
        //    Rate = updateDTO.Rate,
        //    Sqft = updateDTO.Sqft,
        //    UpdatedDate = DateTime.Now
        //};

        _db.Villas.Update(model);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

        if (villa == null)
        {
            return BadRequest();
        }

        VillaUpdateDTO updatedDTO = _mapper.Map<VillaUpdateDTO>(villa);

        patchDTO.ApplyTo(updatedDTO, ModelState);

        Villa model = _mapper.Map<Villa>(updatedDTO);

        _db.Villas.Update(model);
        await _db.SaveChangesAsync();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}
