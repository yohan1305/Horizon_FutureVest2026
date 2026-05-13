using Application.Dtos.Pais;
using Application.Service;
using Application.ViewModels.Pais;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace Horizon_FutureVest2025.Controllers
{
    public class PaisController : Controller
    {
        private readonly PaisService _paisService;

        public PaisController(FutureVestContext context)
        {
            _paisService = new PaisService(context);
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _paisService.GetAllAsync();

            var listEntityVms = dtos.Select(p => new PaisViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CodigoIso = p.CodigoIso
            }).ToList();

            return View(listEntityVms);
        }

        public IActionResult Create()
        {
            return View("Save", new SavePaisViewModel
            {
                Name = "",
                CodigoIso = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            PaisDto dto = new()
            {
                Id = 0,
                Name = vm.Name,
                CodigoIso = vm.CodigoIso
            };

            await _paisService.AddAsync(dto);
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var dto = await _paisService.GetByIdAsync(id);

            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Pais", action = "Index" });
            }

            SavePaisViewModel vm = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                CodigoIso = dto.CodigoIso
            };

            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            PaisDto dto = new()
            {
                Id = vm.Id,
                Name = vm.Name,
                CodigoIso = vm.CodigoIso
            }; ;

            await _paisService.UpdateAsync(dto);
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _paisService.GetByIdAsync(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Pais", action = "Index" });
            }

            DeletePaisViewModel vm = new() { Id = dto.Id, Name = dto.Name };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePaisViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _paisService.DeleteAsync(vm.Id);
            return RedirectToRoute(new { controller = "Pais", action = "Index" });
        }

    }
}
