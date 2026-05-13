using Application.Dtos.Macroindicador;
using Application.Service;
using Application.ViewModels.Macroindicador;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace Horizon_FutureVest2025.Controllers
{
    public class MacroindicadorController : Controller
    {
        private readonly MacroindicadorService _macroService;

        public MacroindicadorController(FutureVestContext context)
        {
            _macroService = new MacroindicadorService(context);
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _macroService.GetAllAsync();

            var listEntityVms = dtos.Select(m => new MacroindicadorViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Peso = m.Peso,
                EsMejorMasAlto = m.EsMejorMasAlto
            }).ToList();

            return View(listEntityVms);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveMacroindicadorViewModel
            {
                Name = "",
                Peso = null,
                EsMejorMasAlto = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            MacroindicadorDto dto = new()
            {
                Id = 0,
                Name = vm.Name,
                Peso = vm.Peso ?? 0,
                EsMejorMasAlto = vm.EsMejorMasAlto ?? false
            };

            var creado = await _macroService.AddAsync(dto);
            if (!creado)
            {
                ModelState.AddModelError("Peso", "La suma total de los pesos no puede superar 1.");
                return View("Save", vm);
            }

            return RedirectToRoute(new { controller = "Macroindicador", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var dto = await _macroService.GetByIdAsync(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Macroindicador", action = "Index" });
            }

            SaveMacroindicadorViewModel vm = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };

            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            MacroindicadorDto dto = new()
            {
                Id = vm.Id,
                Name = vm.Name,
                Peso = vm.Peso ?? 0,
                EsMejorMasAlto = vm.EsMejorMasAlto ?? false
            };

            var editado = await _macroService.UpdateAsync(dto);
            if (!editado)
            {
                ModelState.AddModelError("Peso", "La suma total de los pesos no puede superar 1.");
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            return RedirectToRoute(new { controller = "Macroindicador", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _macroService.GetByIdAsync(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Macroindicador", action = "Index" });
            }

            DeleteMacroindicadorViewModel vm = new() { Id = dto.Id, Name = dto.Name };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _macroService.DeleteAsync(vm.Id);
            return RedirectToRoute(new { controller = "Macroindicador", action = "Index" });
        }

    }
}
