using Application.Dtos.RankingGeneral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Home
{
    public class RankingGeneralViewModel
    {
        public int AnioSeleccionado { get; set; }                      // Año elegido por el usuario
        public List<int> AñosDisponibles { get; set; } = new();        // Lista de años para el select
        public List<RankingGeneralDto> Ranking { get; set; } = new();  // Resultado del ranking
        public string? Mensaje { get; set; }
    }
}
