using Application.Dtos.Simulacion;
using Application.Service;
using Application.ViewModels.Simulacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Context;
using Persistence.Entities;

namespace Horizon_FutureVest2025.Controllers
{
    public class SimulacionController : Controller
    {
        private readonly SimulacionMacroindicadorService _simulacionService;
        private readonly IndicadorPorPaisService _indicadorService;
        private readonly PaisService _paisService;

        public SimulacionController(FutureVestContext context)
        {
            _simulacionService = new SimulacionMacroindicadorService(context);
            _indicadorService = new IndicadorPorPaisService(context);
            _paisService = new PaisService(context);
        }

        public async Task<IActionResult> Index()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var años = await _indicadorService.GetAñosDisponiblesAsync();

            var vm = new SimulacionRankingViewModel
            {
                MacroindicadoresSimulados = simulados.Select(s => new SimulacionMacroindicadorViewModel
                {
                    Id = s.Id,
                    MacroindicadorId = s.MacroindicadorId,
                    Name = s.NombreMacroindicador,
                    PesoSimulacion = s.PesoSimulacion
                }).ToList(),
                AñosDisponibles = años,
                AnioSeleccionado = años.FirstOrDefault()
            };

            return View(vm);
        }

        public async Task<IActionResult> Agregar()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var usados = simulados.Select(s => s.MacroindicadorId).ToList();
            var disponibles = await _simulacionService.GetMacroindicadoresDisponiblesAsync(usados);

            var vm = new AgregarSimulacionMacroindicadorViewModel
            {
                MacroindicadoresDisponibles = disponibles.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(AgregarSimulacionMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.MacroindicadoresDisponibles = await GetSelectListAsync();
                return View(vm);
            }

            var dto = new SimulacionMacroindicadorInputDto
            {
                MacroindicadorId = vm.MacroindicadorId,
                PesoSimulacion = vm.PesoSimulacion
            };

            var ok = await _simulacionService.CrearAsync(dto);
            if (!ok)
            {
                ModelState.AddModelError("", "No se pudo agregar el macroindicador. Verifica que no esté duplicado y que el peso total no supere 1.");
                vm.MacroindicadoresDisponibles = await GetSelectListAsync();
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var simulados = await _simulacionService.GetAllAsync();
            var actual = simulados.FirstOrDefault(s => s.Id == id);
            if (actual == null) return NotFound();

            var vm = new EditarSimulacionMacroindicadorViewModel
            {
                Id = actual.Id,
                PesoSimulacion = actual.PesoSimulacion,
                Name = actual.NombreMacroindicador
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarSimulacionMacroindicadorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new SimulacionMacroindicadorInputDto
            {
                MacroindicadorId = vm.Id,
                PesoSimulacion = vm.PesoSimulacion
            };

            var ok = await _simulacionService.UpdateAsync(vm.Id, dto);
            if (!ok)
            {
                ModelState.AddModelError("", "No se pudo actualizar el peso. Verifica que la suma total no supere 1.");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var simulados = await _simulacionService.GetAllAsync();
            var actual = simulados.FirstOrDefault(s => s.Id == id);
            if (actual == null) return NotFound();

            var vm = new SimulacionMacroindicadorViewModel
            {
                Id = actual.Id,
                MacroindicadorId = actual.MacroindicadorId,
                Name = actual.NombreMacroindicador,
                PesoSimulacion = actual.PesoSimulacion
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(SimulacionMacroindicadorViewModel vm)
        {
            await _simulacionService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SimularRanking(SimulacionRankingViewModel vm)
        {
            var paises = await _paisService.GetAllAsync();
            var indicadoresDto = await _indicadorService.GetAllAsync();
            var indicadores = indicadoresDto.Select(i => new Persistence.Entities.IndicadorPorPais
            {
                Id = i.Id,
                PaisId = i.PaisId,
                MacroindicadorId = i.MacroindicadorId,
                Valor = i.Valor,
                Anio = i.Anio
            }).ToList();

            var resultado = await _simulacionService.GenerarSimulacionAsync(vm.AnioSeleccionado!.Value, indicadores, paises.Select(p => new Persistence.Entities.Pais
            {
                Id = p.Id,
                Name = p.Name,
                CodigoIso = p.CodigoIso
            }).ToList());

            vm.ResultadoRanking = resultado.Ranking;
            vm.Mensaje = resultado.Mensaje;

            var simulados = await _simulacionService.GetAllAsync();
            vm.MacroindicadoresSimulados = simulados.Select(s => new SimulacionMacroindicadorViewModel
            {
                Id = s.Id,
                MacroindicadorId = s.MacroindicadorId,
                Name = s.NombreMacroindicador,
                PesoSimulacion = s.PesoSimulacion
            }).ToList();

            vm.AñosDisponibles = await _indicadorService.GetAñosDisponiblesAsync();

            return View("Index", vm);
        }

        private async Task<List<SelectListItem>> GetSelectListAsync()
        {
            var simulados = await _simulacionService.GetAllAsync();
            var usados = simulados.Select(s => s.MacroindicadorId).ToList();
            var disponibles = await _simulacionService.GetMacroindicadoresDisponiblesAsync(usados);

            return disponibles.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();
        }




    }
}
