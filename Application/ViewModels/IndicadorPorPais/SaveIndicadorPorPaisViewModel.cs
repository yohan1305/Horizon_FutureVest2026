using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.IndicadorPorPais
{
    public class SaveIndicadorPorPaisViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un país")]
        public int? PaisId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un macroindicador")]
        public int? MacroindicadorId { get; set; }
        [Required(ErrorMessage = "Debes ingresar el valor del indicador")]
        [Range(-999999.9999, 999999.9999, ErrorMessage = "El valor debe estar entre -999999.9999 y 999999.9999")]
        public decimal? Valor { get; set; }

        [Required(ErrorMessage = "Debes ingresar el año")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? Anio { get; set; }

        // Para mostrar en edición
        public string? NombrePais { get; set; }
        public string? NombreMacroindicador { get; set; }

    }
}
