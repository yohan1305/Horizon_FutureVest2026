using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Pais
{
    public class SavePaisViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes ingresar el nombre del país")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "Debes ingresar el código ISO")]
        [StringLength(5, ErrorMessage = "Máximo 5 caracteres")]
        public required string CodigoIso { get; set; }

    }
}
