using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.IndicadorPorPais
{
    public class IndicadorPorPaisDto 
    {
        public int Id { get; set; }

        public required int PaisId { get; set; }
        public required int MacroindicadorId { get; set; }

        public required decimal Valor { get; set; }
        public required int Anio { get; set; }

        public string? NombrePais { get; set; }
        public string? NombreMacroindicador { get; set; }


    }
}
