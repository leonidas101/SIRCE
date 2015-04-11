using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class CentroTrabajoNEG : WorkUnit
    {

        #region Constructor
        public CentroTrabajoNEG()
        {
        }

        public CentroTrabajoNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        public CentroTrabajoPOCO validaCentroTrabajoSIRCE(CentroTrabajoPOCO centroTrabajo)
        {
            CentroTrabajoDAT objCentroTrabajoDat = new CentroTrabajoDAT(contextoSIRCE);
            CentrosTrabajoSIRCE centroTrabajoSIRCE = objCentroTrabajoDat.ConsultarCentrosTrabajoDNE(centroTrabajo);
            if (centroTrabajoSIRCE != null)
                centroTrabajo.centroTrabajoSirceID = centroTrabajoSIRCE.CentroTrabajoSIRCEID;
            else
            {
                centroTrabajoSIRCE = new CentrosTrabajoSIRCE();
                centroTrabajoSIRCE.CentroTrabajoID = centroTrabajo.centroTrabajoID;
                contextoSIRCE.CentrosTrabajoSIRCE.Add(centroTrabajoSIRCE);
                Save();
                centroTrabajo.centroTrabajoSirceID = centroTrabajoSIRCE.CentroTrabajoSIRCEID;
                EmpresaCentrosTrabajoSIRCE empresaCentroTrabajoSIRCE = new EmpresaCentrosTrabajoSIRCE();
                empresaCentroTrabajoSIRCE.EmpresaSIRCEID = centroTrabajo.empresaSIRCEID;
                empresaCentroTrabajoSIRCE.CentroTrabajoSIRCEID = centroTrabajoSIRCE.CentroTrabajoSIRCEID;
                contextoSIRCE.EmpresaCentrosTrabajoSIRCE.Add(empresaCentroTrabajoSIRCE);
                Save();
            }
            Dispose();
            return centroTrabajo;
        }
    }
}
