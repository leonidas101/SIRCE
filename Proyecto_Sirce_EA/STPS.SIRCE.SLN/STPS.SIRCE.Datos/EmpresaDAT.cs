using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public class EmpresaDAT : AccesableContext
    {
        
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="contexto"></param>
        public EmpresaDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion
        public EmpresasSIRCE ConsultarEmpresaDNE(EmpresaPOCO empresa)
        {
            var empresaSIRCE = (from e in contexto.EmpresasSIRCE
                                where e.EmpresaID == empresa.empresaID
                                select e).FirstOrDefault();
            return empresaSIRCE;
        } 

        public bool GuardarEmpresaDNE(EmpresasSIRCE empresa)
        {
            contexto.EmpresasSIRCE.Add(empresa);
            return true;
        }

        /// <summary>
        /// Método que recupera la clave de la empresa del DNE
        /// </summary>
        /// <param name="empresaSIRCEID"></param>
        /// <returns></returns>
        public int ConsultarEmpresaSIRCE(int empresaSIRCEID)
        {
            int resultado = 0;
            var empresaSIRCE = (from e in contexto.EmpresasSIRCE
                                where e.EmpresaSIRCEID == empresaSIRCEID
                                select e).FirstOrDefault();
            if (empresaSIRCE != null)
            {
                resultado = empresaSIRCE.EmpresaID;
            }
            return resultado;
        }
    }
}
