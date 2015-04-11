using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class ListaPOCO
    {
        public int listaID { get; set; }
        public int? folioEmpresa { get; set; }
        public string nombreLista { get; set; }
        public int numeroTrabajadores { get; set; }
        public int numeroCursos { get; set; }
        public int numeroConstancias { get; set; }
        public int numeroEstablecimientos { get; set; }
        public string folioDC4 { get; set; }
        public string fechaPresentacion { get; set; }
        public string usuarioPresento { get; set; }
        public string unidadResponsable { get; set; }
        public string entidadFederativa { get; set; }
        public string estatus { get; set; }
        public byte estatusID { get; set; }
        public int unidadResponsableID { get; set; }
        public byte origen { get; set; }
        public int numeroConstanciasTotales { get; set; }
        public string porcentajeAvance { get; set; }
        public string fechaCreacion { get; set; }
        public List<IconosGridPOCO> acciones { get; set; }
    }
}
