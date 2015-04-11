using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class ListaCentrosTrabajoPOCO
    {
        public int ListaID { get; set; }
        public int CentroTrabajoSIRCEID { get; set; }
        public List<ConstanciaPOCO> Constancias { get; set; }
    }
}
