using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class TrabajadorPOCO
    {
        public int TrabajadorID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CURP { get; set; }
        public int OcupacionID { get; set; }
        public string OcupacionDescripcion { get; set; }
        public int EntidadFederativaID { get; set; }
        public int MunicipioID { get; set; }
        public List<NormaTrabajadorPOCO> NormaTrabajador {get; set; }

        public int? EscolaridadID { get; set; }
        public string EscolaridadDescripcion { get; set; }
        public int? InstitucionesEducativasID { get; set; }
        public string InstitucionesEducativasDescripcion { get; set;}
        public int? DocumentosProbatoriosID { get; set; }
        public string DocumentosProbatoriosDescripcion { get; set; }

        public byte Genero { get; set; }
        public string GeneroDescripcion { get; set; }
        public string FechaNacimiento { get; set; }
        public bool Eliminado { get; set; }
        public int EmpresaID { get; set; }

        public int CentroTrabajoSIRCEID {get; set; }
        public int EntidadFederativaIDCT { get; set; }
        public int MuniciopioCTID {get; set; }
    }
}
