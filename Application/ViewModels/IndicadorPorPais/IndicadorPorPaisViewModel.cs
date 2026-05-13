using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.IndicadorPorPais
{
    public class IndicadorPorPaisViewModel
    {
        public int Id { get; set; }

        public required string NombrePais { get; set; }
        public required string NombreMacroindicador { get; set; }

        public required decimal Valor { get; set; }
        public required int Anio { get; set; }

    }
}
