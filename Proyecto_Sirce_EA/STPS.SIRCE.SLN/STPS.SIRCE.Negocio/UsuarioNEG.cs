using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Datos;

namespace STPS.SIRCE.Negocio
{
    public class UsuarioNEG: WorkUnit
    {

        #region Constructor
        public UsuarioNEG()
        {
        }

        public UsuarioNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        /// <summary>
        /// Método para obtener los datos de un usuario
        /// </summary>
        /// <param name="obj">Objeto de tipo usuario</param>
        public bool ObtenerUsuario(Usuario obj)
        {
            UsuarioDAT miObjeto = new UsuarioDAT(contextoSIRCE);

            return true;

        }

    }
}
