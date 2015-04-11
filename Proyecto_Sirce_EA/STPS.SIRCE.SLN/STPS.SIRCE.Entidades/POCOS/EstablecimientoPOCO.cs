using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class EstablecimientoPOCO
    {
        public string ConstanciaID { get; set; }
        public int trabajadorID { get; set; }
        public string TrabajadorDescripcion { get; set; }
        public int cursoID { get; set; }
        public string CursoDescripcion { get; set; }
        public bool Eliminado { get; set; }
        public int CentroTrabajoSIRCEID { get; set; }
        public int CentroTrabajoID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CURP { get; set; }
        public string centroTrabajoNombre { get; set; }
        public int empresaID { get; set; }
    }
}