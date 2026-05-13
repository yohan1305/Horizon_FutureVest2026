using Application.Service;
using Application.ViewModels.ConfiguracionRetorno;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace Horizon_FutureVest2025.Controllers
{
    public class ConfiguracionRetornoController : Controller
    {
        private readonly ConfiguracionRetornoService _service;

        public ConfiguracionRetornoController(FutureVestContext context)
        {
            _service = new ConfiguracionRetornoService(context);
        }

        public async Task<IActionResult> Index()
        {
            var dto = await _service.GetAsync();

            // Si no hay configuración, crearla con valores por defecto del mandato
            if (dto == null)
            {
                await _service.CrearAsync(2.00m, 15.00m); // ✅ Esto es 2% y 15% // 2% y 15%
                dto = await _service.GetAsync(); // Recargar después de crear
            }

            var vm = new ConfiguracionRetornoViewModel
            {
                Id = dto.Id,
                TasaMinima = dto.TasaMinima,
                TasaMaxima = dto.TasaMaxima
            };

            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(ConfiguracionRetornoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Index", vm);

            if (vm.TasaMinima >= vm.TasaMaxima)
            {
                ModelState.AddModelError("TasaMinima", "La tasa mínima debe ser menor que la tasa máxima.");
                return View("Index", vm);
            }

            var dto = new Application.Dtos.ConfiguracionRetorno.ConfiguracionRetornoDto
            {
                Id = vm.Id,
                TasaMinima = vm.TasaMinima ?? 0,
                TasaMaxima = vm.TasaMaxima ?? 0
            };

            await _service.UpdateAsync(dto);
            TempData["Mensaje"] = "Tasas actualizadas correctamente.";
            return RedirectToAction("Index");
        }
    }
}