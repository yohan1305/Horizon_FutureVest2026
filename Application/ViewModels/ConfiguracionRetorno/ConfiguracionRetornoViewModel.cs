using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ConfiguracionRetorno
{
    public class ConfiguracionRetornoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La tasa mínima es requerida")]
        [Range(1.00, 100.00, ErrorMessage = "La tasa mínima debe estar entre 1% y 100%")]
        public decimal? TasaMinima { get; set; }

        [Required(ErrorMessage = "La tasa máxima es requerida")]
        [Range(1.00, 100.00, ErrorMessage = "La tasa máxima debe estar entre 1% y 100%")]
        public decimal? TasaMaxima { get; set; }


    }
}
