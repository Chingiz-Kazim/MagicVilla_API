using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto;

public class VillaNoDTO
{
    [Required]
    public int VillaNo { get; set; }
    public string SpecialDetails { get; set; }
}
