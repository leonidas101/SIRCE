using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades.POCOS
{
    public class SesionPOCO
    {
        public UsuarioPOCO usuario { get; set; }

        public EmpresaPOCO empresa { get; set; }

        public CentroTrabajoPOCO centroTrabajo { get; set; }
    }
}
