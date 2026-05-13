using Application.Dtos.RankingGeneral;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Simulacion
{
    public class SimulacionRankingViewModel
    {
        public List<SimulacionMacroindicadorViewModel> MacroindicadoresSimulados { get; set; } = new();

        [Required(ErrorMessage = "Debes seleccionar un año")]
        public int? AnioSeleccionado { get; set; }

        public List<int> AñosDisponibles { get; set; } = new();

        public string? Mensaje { get; set; }

        public List<RankingGeneralDto> ResultadoRanking { get; set; } = new(); // Si se genera

    }
}
