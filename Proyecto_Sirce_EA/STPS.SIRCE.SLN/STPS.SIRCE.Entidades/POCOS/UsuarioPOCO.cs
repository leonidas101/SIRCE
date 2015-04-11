using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades.POCOS
{
    public class UsuarioPOCO
    {
        public int unidadResponsable { get; set; }
        public int usuarioActiveDirectoryID { get; set; }
        public string usuarioActiveDirectoryNombre { get; set; }
        public string usuarioActiveDirectoryCorreo { get; set; }
    }
}
