using MagicVilla_Web.Controllers;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM;

public class VillaNoCreateVM
{
    public VillaNoCreateVM()
    {
        VillaNo = new VillaNoCreateDTO();
    }

    public VillaNoCreateDTO VillaNo {  get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}
