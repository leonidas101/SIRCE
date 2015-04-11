using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace STPS.SIRCE.Datos
{
    public class ListaCentrosTrabajoDAT : AccesableContext
    {
        #region Constructor
        public ListaCentrosTrabajoDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        /// <summary>
        /// Método para obtener el listaCentroTrabajoID.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public int ObtenerlistaCentroTrabajoID(int listaID, int centroTrabajoSIRCEID)
        {
            int resultado = 0;
            var CentroTrabajoID = contexto.ListaCentrosTrabajo.FirstOrDefault(x => x.ListaID == listaID && x.CentroTrabajoSIRCEID == centroTrabajoSIRCEID);

            if (CentroTrabajoID != null)
            {
                resultado = CentroTrabajoID.ListaCentroTrabajoID;
            }
            return resultado;
        }

        /// <summary>
        /// Guardar un registro
        /// </summary>
        /// <param name="listaCentroTrabajo"></param>
        /// <returns></returns>
        public bool postListaCentroTrabajo(ListaCentrosTrabajo listaCentroTrabajo)
        {
            contexto.ListaCentrosTrabajo.Add(listaCentroTrabajo);
            return true;
        }
    }
}
