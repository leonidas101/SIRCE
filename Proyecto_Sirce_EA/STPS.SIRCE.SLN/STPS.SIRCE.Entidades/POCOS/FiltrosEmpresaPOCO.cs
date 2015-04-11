using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class FiltrosEmpresaPOCO
    {
        public string rfc { get; set; }
        public string registroIMSS { get; set; }
        public string nombreRazonSocial { get; set; }
        public string calleNumero { get; set; }
        public int? entidadID { get; set; }
        public int? municipioID { get; set; }
    }
}
