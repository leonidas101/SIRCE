using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class AccionNEG : WorkUnit
    {
        #region Constructor
        public AccionNEG()
        {
        }

        public AccionNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        public List<CatalogoPOCO> consultarAcciones()
        {
            AccionDAT objAccionDAT = new AccionDAT(contextoSIRCE);
            List<CatalogoPOCO> resultSet = objAccionDAT.ConsultarAcciones();
            Dispose();
            return resultSet;
        }

        public int consultarValorAccion(List<CatalogoPOCO> lista, string desc)
        {
            var result = (from t in lista
                          where t.catalogoDescripcion.Equals(desc)
                          select t.catalogoID).FirstOrDefault();
            return result;
        }
    }
}
