using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public class CentroTrabajoDAT : AccesableContext
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="contexto"></param>
        public CentroTrabajoDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        public CentrosTrabajoSIRCE ConsultarCentrosTrabajoDNE(CentroTrabajoPOCO centroTrabajo)
        {
            var centroTrabajoSIRCE = (from c in contexto.CentrosTrabajoSIRCE
                                join ec in contexto.EmpresaCentrosTrabajoSIRCE on centroTrabajo.empresaSIRCEID equals ec.EmpresaSIRCEID
                                join e in contexto.EmpresasSIRCE on ec.EmpresaSIRCEID equals e.EmpresaSIRCEID
                                where c.CentroTrabajoID == centroTrabajo.centroTrabajoID
                                select c).FirstOrDefault();
            return centroTrabajoSIRCE;
        } 
    }
}
