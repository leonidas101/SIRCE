using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class CursoPOCO
    {
        public string AreaTematica { get; set; }
        public int AreaTematicaId { get; set; }
        public int CursoId { get; set; }
        public string FechaTermino { get; set; }
        public string Nombre { get; set; }

        public int Duracion { get; set; }
    }
}
