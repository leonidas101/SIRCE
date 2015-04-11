using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class NormaTrabajadorPOCO
    {
        public int NormaTrabajadorID { get; set; }
        public int TrabajadorID { get; set; }
        public int NormaCompetenciaID { get; set; }
        public string FechaEmision { get; set; }
        public bool Eliminado { get; set; }

        public string NormaCompetenciaDescripcion { get; set; }
    }
}
