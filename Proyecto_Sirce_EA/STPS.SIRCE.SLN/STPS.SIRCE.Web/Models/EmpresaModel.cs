using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System.Text.RegularExpressions;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Web.Models
{
    public class EmpresaModel
    {
        public string rfc { get; set; }
        public string registroIMSS { get; set; }
        public string nombreRazonSocial { get; set; }
        public string calleNumero { get; set; }
        public int? entidadID { get; set; }
        public int? municipioID { get; set; }
        public List<CatalogoPOCO> entidades { get; set; }
        public List<CatalogoPOCO> municipios { get; set; }
        public GridPOCO<EmpresaPOCO> gridEmpresas  { get; set; }
        public EmpresaPOCO empresa { get; set; }


        public void ConsultarCatalogos(){
            this.entidades = new UtileriaNeg().ConsultarEntidades();
        }

        public void ConsultarMunicipios()
        {
            this.municipios = new UtileriaNeg().ConsultarMunicipios(this.entidadID);
        }

        public void ConsultarEmpresas()
        {
            this.gridEmpresas = new GridPOCO<EmpresaPOCO>();
            FiltrosEmpresaPOCO filtrosBusqueda = new FiltrosEmpresaPOCO();
            filtrosBusqueda.rfc = this.rfc;
            filtrosBusqueda.registroIMSS = this.registroIMSS;
            filtrosBusqueda.nombreRazonSocial = this.nombreRazonSocial;
            filtrosBusqueda.calleNumero = this.calleNumero;
            filtrosBusqueda.entidadID = this.entidadID;
            filtrosBusqueda.municipioID = this.municipioID;
            this.gridEmpresas.datos = new UtileriaNeg().ConsultarEmpresas(filtrosBusqueda);
            //Se declaran los titulos de la tabla
            this.gridEmpresas.encabezados = new List<string>();
            this.gridEmpresas.encabezados.Add("Empresa");
            this.gridEmpresas.encabezados.Add("Domicilio");
            this.gridEmpresas.encabezados.Add("RFC");
            this.gridEmpresas.encabezados.Add("Registro patronal del IMSS");
            //se declaran los valores de la tabla
            this.gridEmpresas.columnas = new List<string>();
            this.gridEmpresas.columnas.Add("empresa");
            this.gridEmpresas.columnas.Add("domicilio");
            this.gridEmpresas.columnas.Add("rfc");
            this.gridEmpresas.columnas.Add("registroIMSS");
            //se declara cual tendra el id del renglon seleccionado
            this.gridEmpresas.columnaID = "empresaID";
            //se inicializa empresa
            this.empresa = new EmpresaPOCO();
            this.empresa.directorioOrigen = (int)Enumeradores.DirectorioOrigen.DNE;
        }

        public void ValidaEmpresaSIRCE()
        {
            this.empresa = new EmpresaNEG().ValidaEmpresaSIRCE(this.empresa);
        }
    }
}