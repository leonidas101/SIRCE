using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class EmpresaPOCO
    {
        public int empresaSIRCEID { get; set; }

        public int empresaID { get; set; }

        public string empresa { get; set; }

        public string domicilio { get; set; }

        public string rfc { get; set; }

        public string registroIMSS { get; set; }

        public byte directorioOrigen { get; set; }
    }
}
