using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Macroindicador
{
    public class MacroindicadorViewModel : BasicViewModel<int>
    {
        public required decimal Peso { get; set; }

        public required bool EsMejorMasAlto { get; set; }
    }
}
