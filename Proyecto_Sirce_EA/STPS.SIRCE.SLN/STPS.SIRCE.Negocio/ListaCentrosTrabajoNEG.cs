using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.Framework;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Negocio
{
    public class ListaCentrosTrabajoNEG : WorkUnit
    {
        #region Constructor
        public ListaCentrosTrabajoNEG() { }

        public ListaCentrosTrabajoNEG(SIRCEEntities context)
            : base(context)
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
            try
            {
                ListaCentrosTrabajoDAT datos = new ListaCentrosTrabajoDAT(contextoSIRCE);
                resultado = datos.ObtenerlistaCentroTrabajoID(listaID, centroTrabajoSIRCEID);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
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
            try
            {
                ListaCentrosTrabajoDAT datos = new ListaCentrosTrabajoDAT(contextoSIRCE);
                datos.postListaCentroTrabajo(listaCentroTrabajo);
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return true;
        }
    }
}
