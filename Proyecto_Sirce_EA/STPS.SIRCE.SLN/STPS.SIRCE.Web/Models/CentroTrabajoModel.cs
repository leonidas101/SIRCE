using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STPS.SIRCE.Web.Models
{
    public class CentroTrabajoModel
    {
        public int empresaID { get; set; }
        public int entidadID { get; set; }
        public int municipioID { get; set; }
        public string entidadDescripcion { get; set; }
        public string municipioDescripcion { get; set; }
        public List<CatalogoPOCO> entidades { get; set; }
        public List<CatalogoPOCO> municipios { get; set; }
        public CentroTrabajoPOCO centroTrabajo { get; set; }
        public List<CentroTrabajoPOCO> centrosTrabajoEmpresa { get; set; }

        public void ConsultarCatalogos()
        {
            this.entidades = new UtileriaNeg().ConsultarEntidades();
        }

        public void ConsultarMunicipios()
        {
            this.municipios = new UtileriaNeg().ConsultarMunicipios(this.entidadID);
        }

        public void ConsultarCentrosTrabajo()
        {
            this.centroTrabajo = new CentroTrabajoPOCO();
            this.centroTrabajo.entidadID = this.entidadID;
            this.centroTrabajo.entidadDescripcion = this.entidadDescripcion;
            this.centroTrabajo.municipioID = this.municipioID;
            this.centroTrabajo.municipioDescripcion = this.municipioDescripcion;
            this.centroTrabajo.empresaID = this.empresaID;
            this.centrosTrabajoEmpresa = new UtileriaNeg().ConsultarCentrosTrabajo(centroTrabajo);
            this.centrosTrabajoEmpresa = new UtileriaNeg().ConsultarSCIANCentrosTrabajo(this.centrosTrabajoEmpresa);
        }

        public void ValidaCentroTrabajoSIRCE()
        {
            this.centroTrabajo = new CentroTrabajoNEG().validaCentroTrabajoSIRCE(this.centroTrabajo);
        }
    }
}