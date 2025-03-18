using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNoController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IVillaNoService _villaNoService;
    private readonly IMapper _mapper;

    public VillaNoController(IVillaNoService villaNoService, IVillaService villaService, IMapper mapper)
    {
        _villaNoService = villaNoService;
        _villaService = villaService;
        _mapper = mapper;
    }
    public async Task<IActionResult> IndexVillaNo()
    {
        List<VillaNoDTO> list = new();

        var response = await _villaNoService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNoDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVillaNo()
    {
        //необходимо объяснение!                                                    //необходимо объяснение!

        VillaNoCreateVM villaNoVM = new();

        var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n=> new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString()
            });
        }

        return View(villaNoVM);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNo(VillaNoCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNoService.CreateAsync<APIResponse>(model.VillaNo, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNo));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }

        var resp = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (resp != null && resp.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
        }

        return View(model);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVillaNo(int villaNo)
    {
        VillaNoUpdateVM villaNoVM = new();
        var response = await _villaNoService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            VillaNoDTO model = JsonConvert.DeserializeObject<VillaNoDTO>(Convert.ToString(response.Result));
            villaNoVM.VillaNo = _mapper.Map<VillaNoUpdatedDTO>(model);
        }

        response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString()
            });

            return View(villaNoVM);
        }

        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNo(VillaNoUpdateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNoService.UpdateAsync<APIResponse>(model.VillaNo, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNo));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }

        var resp = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (resp != null && resp.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
        }

        return View(model);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVillaNo(int villaNo)
    {
        VillaNoDeleteVM villaNoVM = new();
        var response = await _villaNoService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            VillaNoDTO model = JsonConvert.DeserializeObject<VillaNoDTO>(Convert.ToString(response.Result));
            villaNoVM.VillaNo = model;
        }

        response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString()
            });

            return View(villaNoVM);
        }

        return NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNo(VillaNoDeleteVM model)
    {
        var response = await _villaNoService.DeleteAsync<APIResponse>(model.VillaNo.VillaNo, HttpContext.Session.GetString(SD.SessionToken));

        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNo));
        }

        return View(model);
    }
}
