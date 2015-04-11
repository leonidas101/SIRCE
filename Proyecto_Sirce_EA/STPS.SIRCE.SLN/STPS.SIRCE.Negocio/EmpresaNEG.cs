using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class EmpresaNEG : WorkUnit
    {
        
        #region Constructor
        public EmpresaNEG()
        {
        }

        public EmpresaNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        public EmpresaPOCO ValidaEmpresaSIRCE(EmpresaPOCO empresa)
        {
            EmpresaDAT empresaDAT = new EmpresaDAT(contextoSIRCE);
            EmpresasSIRCE empresaSIRCE = empresaDAT.ConsultarEmpresaDNE(empresa);
            if (empresaSIRCE != null)
               empresa.empresaSIRCEID = empresaSIRCE.EmpresaSIRCEID;
            else
            {
                empresaSIRCE = new EmpresasSIRCE();
                empresaSIRCE.EmpresaID = empresa.empresaID;
                empresaSIRCE.ConsecutivoLista = 1;
                empresaDAT.GuardarEmpresaDNE(empresaSIRCE);
                Save();
                empresa.empresaSIRCEID = empresaSIRCE.EmpresaSIRCEID;
            }
            Dispose();
            return empresa;
        }

    }
}
