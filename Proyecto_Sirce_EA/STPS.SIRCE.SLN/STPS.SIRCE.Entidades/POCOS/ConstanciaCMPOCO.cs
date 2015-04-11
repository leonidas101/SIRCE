using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades.POCOS
{
    public class ConstanciaCMPOCO
    {
        #region Curso
        public string AreaTematica { get; set; }
        public string AreaTematicaId { get; set; }
        public string CursoId { get; set; }
        public string FechaInicio { get; set; }
        public string FechaTermino { get; set; }
        public string NombreCurso { get; set; }
        public string Duracion { get; set; }
        public string RegistroAgenteExterno { get; set; }
        public string CursoIdValido { get; set; }
        public string AreaTematicaIdValido { get; set; }
        public string FechaInicioValida { get; set; }
        public string FechaTerminoValida { get; set; }
        public string NombreCursoValido { get; set; }
        public string DuracionValida { get; set; }
        public string TipoAgenteCapacitadorID { get; set; }
        public string TipoAgenteCapacitadorValido { get; set; }
        public string ModalidadCapacitacionID { get; set; }
        public string ModalidadCapacitacionValido { get; set; }
        public string ObjetivoCapacitacionID { get; set; }
        public string ObjetivoCapacitacionValido { get; set; }
        #endregion

        #region Trabajador
        public string TrabajadorID { get; set; }
        public string NombreTrabajador { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CURP { get; set; }
        public string OcupacionID { get; set; }
        public string OcupacionDescripcion { get; set; }
        public string EntidadFederativaID { get; set; }
        public string MunicipioID { get; set; }
        public List<NormaTrabajadorPOCO> NormaTrabajador { get; set; }
        public string EscolaridadID { get; set; }
        public string EscolaridadDescripcion { get; set; }
        public string InstitucionesEducativasID { get; set; }
        public string InstitucionesEducativasDescripcion { get; set; }
        public string DocumentosProbatoriosID { get; set; }
        public string DocumentosProbatoriosDescripcion { get; set; }
        public string Genero { get; set; }
        public string GeneroDescripcion { get; set; }
        public string FechaNacimiento { get; set; }
        public bool Eliminado { get; set; }
        public int EmpresaID { get; set; }
        public string CentroTrabajoSIRCEID { get; set; }
        public int EntidadFederativaIDCT { get; set; }
        public int MuniciopioCTID { get; set; }
        public string TrabajadorIDValido { get; set; }
        public string NombreTrabajadorValido { get; set; }
        public string ApellidoPaternoValido { get; set; }
        public string ApellidoMaternoValido { get; set; }
        public string CURPValido { get; set; }
        public string OcupacionIdValido { get; set; }
        public string EntidadFederativaIdValido { get; set; }
        public string MunicipioIdValido { get; set; }
        public string EscolaridadIdValido { get; set; }
        public string InstitucionesEducativasIdValido { get; set; }
        public string DocumentosProbatoriosIdValido { get; set; }
        #endregion
        public bool TrabajadorDatosValidos { get; set; }
        public bool CursoDatosValidos { get; set; }
        public int ConstanciaID { get; set; }
        public string CentroTrabajoIdCarga { get; set; }
        public virtual ListaCentrosTrabajo ListaCentrosTrabajo { get; set; }
    }
}
