using Application.Service;
using Application.ViewModels.Home;
using Horizon_FutureVest2025.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using Persistence.Repositories;
using System.Diagnostics;

namespace Horizon_FutureVest2025.Controllers
{
    public class HomeController : Controller
    {
        private readonly FutureVestContext _context;

        public HomeController(FutureVestContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int anio)
        {
            // Obtener todos los años disponibles
            var añosDisponibles = (await new IndicadorPorPaisRepository(_context)
                .GetAllAsync())
                .Select(i => i.Anio)
                .Distinct()
                .OrderByDescending(a => a)
                .ToList();

            // Si no se seleccionó año, usar el más reciente
            var añoSeleccionado = anio != 0 ? anio : añosDisponibles.FirstOrDefault();

            
            var rankingService = new RankingGeneralService(_context);

            // Calcular el ranking
            var (ranking, mensaje) = await rankingService.CalcularRankingAsync(añoSeleccionado);

            // Armar el ViewModel
            var viewModel = new RankingGeneralViewModel
            {
                AnioSeleccionado = añoSeleccionado,
                AñosDisponibles = añosDisponibles,
                Ranking = ranking,
                Mensaje = mensaje
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
