using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.IndicadorPorPais
{
    public class DeleteIndicadorPorPaisViewModel
    {
        public int Id { get; set; }
        public string? NombrePais { get; set; }
        public string? NombreMacroindicador { get; set; }
        public int Anio { get; set; }

    }
}
