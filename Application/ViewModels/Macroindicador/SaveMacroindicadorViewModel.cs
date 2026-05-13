using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Macroindicador
{
    public class SaveMacroindicadorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes Ingresar el nombre del macroindicador")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debes Ingresar el peso")]
        [Range(0.01, 1.00, ErrorMessage = "El peso debe estar entre 0.01 y 1.00")]
        public decimal? Peso { get; set; }


        [Required(ErrorMessage = "Debes indicar si es mejor mas alto")]
        public bool? EsMejorMasAlto { get; set; }

    }
}
