using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;

namespace STPS.SIRCE.Datos
{
    public class UsuarioDAT:AccesableContext
    {

        #region Constructor
        public UsuarioDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        public string atributo;
        private string atPrivado;

        /// <summary>
        /// Método para obtener los datos de un usuario
        /// </summary>
        /// <param name="obj">Objeto de tipo usuario</param>
        public static void ObtenerUsuarioEstatico(Usuario obj)
        {
            
        }


        private void MetodoPrivado()
        {

            string cadena = "";
        }

        public bool ObtenerUsuario(Usuario obj)
        {
            

            return true;
        }
    }
}
