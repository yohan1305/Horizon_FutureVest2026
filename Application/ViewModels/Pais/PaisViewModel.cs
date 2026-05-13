using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Pais
{
    public class PaisViewModel : BasicViewModel<int>
    {
        public required string CodigoIso { get; set; }

    }
}
