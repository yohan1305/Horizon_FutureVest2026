using Application.Dtos.IndicadorPorPais;
using Application.Service;
using Application.ViewModels.IndicadorPorPais;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Context;

namespace Horizon_FutureVest2025.Controllers
{
    public class IndicadorPorPaisController : Controller
    {
        private readonly IndicadorPorPaisService _service;
        private readonly PaisService _paisService;
        private readonly MacroindicadorService _macroService;

        public IndicadorPorPaisController(FutureVestContext context)
        {
            _service = new IndicadorPorPaisService(context);
            _paisService = new PaisService(context);
            _macroService = new MacroindicadorService(context);
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();

            var listVm = dtos.Select(i => new IndicadorPorPaisViewModel
            {
                Id = i.Id,
                NombrePais = i.NombrePais ?? "",
                NombreMacroindicador = i.NombreMacroindicador ?? "",
                Valor = i.Valor,
                Anio = i.Anio
            }).ToList();

            ViewBag.Paises = await GetPaisesSelectList();
            return View(listVm);
        }

        [HttpGet]
        public async Task<IActionResult> Filtrar(int paisId, int? anio)
        {
            var dtos = await _service.FiltrarAsync(paisId, anio);

            var listVm = dtos.Select(i => new IndicadorPorPaisViewModel
            {
                Id = i.Id,
                NombrePais = i.NombrePais ?? "",
                NombreMacroindicador = i.NombreMacroindicador ?? "",
                Valor = i.Valor,
                Anio = i.Anio
            }).ToList();

            ViewBag.Paises = await GetPaisesSelectList();
            return View("Index", listVm);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Paises = await GetPaisesSelectList();
            ViewBag.Macroindicadores = await GetMacroSelectList();

            return View("Save", new SaveIndicadorPorPaisViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveIndicadorPorPaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = await GetPaisesSelectList();
                ViewBag.Macroindicadores = await GetMacroSelectList();
                return View("Save", vm);
            }

            IndicadorPorPaisDto dto = new()
            {
                Id = 0,
                PaisId = vm.PaisId ?? 0,
                MacroindicadorId = vm.MacroindicadorId ?? 0,
                Valor = vm.Valor ?? 0,
                Anio = vm.Anio ?? 0
            };

            var creado = await _service.AddAsync(dto);
            if (!creado)
            {
                ModelState.AddModelError("MacroindicadorId", "Ya existe un indicador para este macroindicador en ese año.");
                ViewBag.Paises = await GetPaisesSelectList();
                ViewBag.Macroindicadores = await GetMacroSelectList();
                return View("Save", vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return RedirectToAction("Index");

            SaveIndicadorPorPaisViewModel vm = new()
            {
                Id = dto.Id,
                PaisId = dto.PaisId,
                MacroindicadorId = dto.MacroindicadorId,
                Valor = dto.Valor,
                Anio = dto.Anio,
                NombrePais = dto.NombrePais,
                NombreMacroindicador = dto.NombreMacroindicador
            };

            ViewBag.Paises = await GetPaisesSelectList();
            ViewBag.Macroindicadores = await GetMacroSelectList();
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveIndicadorPorPaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                ViewBag.Paises = await GetPaisesSelectList();
                ViewBag.Macroindicadores = await GetMacroSelectList();
                return View("Save", vm);
            }

            IndicadorPorPaisDto dto = new()
            {
                Id = vm.Id,
                PaisId = vm.PaisId ?? 0,
                MacroindicadorId = vm.MacroindicadorId ?? 0,
                Valor = vm.Valor ?? 0,
                Anio = vm.Anio ?? 0
            };

            await _service.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return RedirectToAction("Index");

            DeleteIndicadorPorPaisViewModel vm = new()
            {
                Id = dto.Id,
                NombrePais = dto.NombrePais,
                NombreMacroindicador = dto.NombreMacroindicador,
                Anio = dto.Anio
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteIndicadorPorPaisViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _service.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        private async Task<List<SelectListItem>> GetPaisesSelectList()
        {
            var paises = await _paisService.GetAllAsync();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetMacroSelectList()
        {
            var macros = await _macroService.GetAllAsync();
            return macros.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();
        }

    }
}
