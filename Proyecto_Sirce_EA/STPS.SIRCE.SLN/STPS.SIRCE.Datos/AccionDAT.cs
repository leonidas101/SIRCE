using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public class AccionDAT : AccesableContext
    {
         #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="contexto"></param>
        public AccionDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        public List<CatalogoPOCO> ConsultarAcciones()
        {
            var resultado = (from a in contexto.Acciones
                             select new CatalogoPOCO
                             {
                                 catalogoID = a.AccionID,
                                 catalogoDescripcion = a.AccionDescripcion
                             }).ToList();
            return resultado;
        }

    }
}
