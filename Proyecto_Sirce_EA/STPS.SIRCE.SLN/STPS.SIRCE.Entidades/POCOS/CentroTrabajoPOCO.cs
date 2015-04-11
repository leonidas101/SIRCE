using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class CentroTrabajoPOCO
    {
        public int centroTrabajoSirceID { get; set; }

        public int centroTrabajoID { get; set; }

        public int empresaID { get; set; }

        public int empresaSIRCEID { get; set; }

        public string centroTrabajoNombre { get; set; }

        public string rfc { get; set; }

        public string registroIMSS { get; set; }

        public string curp { get; set; }

        public string calleNumero { get; set; }

        public string colonia { get; set; }

        public string codigoPostal { get; set; }

        public string telefono { get; set; }

        public string fax { get; set; }

        public string correoElectronico { get; set; }

        public int actividadEconomicaID { get; set; }

        public string actividadEconomica { get; set; }

        public int tipoContratoID { get; set; }

        public string tipoContrato { get; set; }

        public int entidadID { get; set; }

        public string entidadDescripcion { get; set; }

        public int municipioID { get; set; }

        public string municipioDescripcion { get; set; }
    }
}
