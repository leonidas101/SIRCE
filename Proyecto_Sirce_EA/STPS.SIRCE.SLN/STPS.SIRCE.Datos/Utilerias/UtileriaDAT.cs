using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using System.Data;

namespace STPS.SIRCE.Datos
{
    public class UtileriaDAT
    {
        private OrquestadorConexiones OC = new OrquestadorConexiones();

        #region Conexiones a Catalogos Institucionales
        public List<CatalogoPOCO> ConsultarEntidades()
        {
            List<CatalogoPOCO> entidadesFederativas =
                SerializarCatalogoPoco(OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.CI, Diccionarios.DiccionarioCI["BuscarEntidadesFederativas"]));
            return entidadesFederativas;
        }

        public List<CatalogoPOCO> ConsultarMunicipios(int? municipioID)
        {
            string query = Diccionarios.DiccionarioCI["BuscarMunicipios"];
            query += " where cmu_cve_edorep=" + municipioID.ToString();
            List<CatalogoPOCO> municipios = SerializarCatalogoPoco(OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.CI, query));
            return municipios;
        }

        public List<CentroTrabajoPOCO> ConsultarSCIANCentrosTrabajo(List<CentroTrabajoPOCO> centrosTrabajo)
        {
            string query = Diccionarios.DiccionarioCI["BuscarSCIAN"];
            DataTable resultSet = OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.CI, query);
            foreach (CentroTrabajoPOCO value in centrosTrabajo)
            {
                if (value.actividadEconomicaID != 0)
                    value.actividadEconomica = resultSet.Select("actividadEconomicaID = " + value.actividadEconomicaID.ToString()).First()["actividadEconomica"].ToString();
                else
                    value.actividadEconomica = "No especificada";
            }
            return centrosTrabajo;
        }

        public DataTable ConsultarUnidadesResponsables()
        {
            string query = Diccionarios.DiccionarioCI["ConsultarUnidadResponsable"];
            return OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.CI, query);
        }

        public bool ConsultarEntidad(CatalogoPOCO entidad)
        {
            List<CatalogoPOCO> entidades = ConsultarEntidades();

            var resultado = entidades.Where(x => x.catalogoID == entidad.catalogoID).FirstOrDefault();

            if (resultado != null)
            {
                entidad.catalogoID = resultado.catalogoID;
                entidad.catalogoDescripcion = resultado.catalogoDescripcion;                
                return true;
            }
            return false;
        }

        public bool ConsultarMunicipo(CatalogoPOCO entidad, int idEntidad)
        {
            List<CatalogoPOCO> municipios = ConsultarMunicipios(idEntidad);

            var resultado = municipios.Where(x => x.catalogoID == entidad.catalogoID).FirstOrDefault();

            if (resultado != null)
            {
                entidad.catalogoID = resultado.catalogoID;
                entidad.catalogoDescripcion = resultado.catalogoDescripcion;
                return true;
            }
            return false;
        }
        #endregion

        #region Conexiones a Directorio Nacional de Empresas
        public List<EmpresaPOCO> ConsultarEmpresas(FiltrosEmpresaPOCO filtrosBusquedaEmpresa)
        {
            string query = Diccionarios.DiccionarioDNE["BuscarEmpresas"];
            if (filtrosBusquedaEmpresa.rfc != null)
                query += " and e.emp_rfc like ('%" + filtrosBusquedaEmpresa.rfc + "%') ";
            if (filtrosBusquedaEmpresa.registroIMSS != null)
                query += " and ct.ct_imss_registro like ('%" + filtrosBusquedaEmpresa.registroIMSS + "%') ";
            if (filtrosBusquedaEmpresa.nombreRazonSocial != null)
                query += " and ct.ct_nombre_comercial like ('%" + filtrosBusquedaEmpresa.nombreRazonSocial + "%') ";
            if (filtrosBusquedaEmpresa.calleNumero != null)
                query += " and ct.ct_calle like ('%" + filtrosBusquedaEmpresa.calleNumero + "%') ";
            if (filtrosBusquedaEmpresa.entidadID != null)
                query += " and ct.ct_cve_edorep = " + filtrosBusquedaEmpresa.entidadID + " ";
            if (filtrosBusquedaEmpresa.municipioID != null)
            {
                query += " and ct.ct_cve_municipio = " + filtrosBusquedaEmpresa.municipioID + " ";
            }
            DataTable resultSet = OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.DNE, query);
            List<EmpresaPOCO> empresas = new List<EmpresaPOCO>();
            foreach (DataRow row in resultSet.Rows)
            {
                EmpresaPOCO objEmpresa = new EmpresaPOCO();
                objEmpresa.empresaID = int.Parse(row["empresa_id"].ToString());
                objEmpresa.empresa = row["empresa"].ToString();
                objEmpresa.domicilio = row["domicilio"].ToString();
                objEmpresa.rfc = row["rfc"].ToString();
                objEmpresa.registroIMSS = row["registroIMSS"].ToString();
                empresas.Add(objEmpresa);
            }
            return empresas;
        }

        public List<CentroTrabajoPOCO> ConsultarCentrosTrabajo(CentroTrabajoPOCO centroTrabajo)
        {
            string query = Diccionarios.DiccionarioDNE["BuscarCentrosTrabajo"];
            query += " where e.empresa_id = " + centroTrabajo.empresaID;
            query += " and c.ct_cve_edorep = " + centroTrabajo.entidadID;
            query += " and c.ct_cve_municipio = " + centroTrabajo.municipioID;
            DataTable resultSet = OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.DNE, query);
            List<CentroTrabajoPOCO> centrosTrabajo = new List<CentroTrabajoPOCO>();
            foreach (DataRow row in resultSet.Rows)
            {
                CentroTrabajoPOCO objCentroTrabajo = new CentroTrabajoPOCO();
                objCentroTrabajo.centroTrabajoID = int.Parse(row["centroTrabajoID"].ToString());
                objCentroTrabajo.centroTrabajoNombre = row["centroTrabajoNombre"].ToString();
                objCentroTrabajo.rfc = row["rfc"].ToString();
                objCentroTrabajo.registroIMSS = row["registroIMSS"].ToString();
                objCentroTrabajo.curp = row["curp"].ToString();
                objCentroTrabajo.calleNumero = row["calleNumero"].ToString();
                objCentroTrabajo.colonia = row["colonia"].ToString();
                objCentroTrabajo.codigoPostal = row["codigoPostal"].ToString();
                objCentroTrabajo.telefono = row["telefono"].ToString();
                objCentroTrabajo.fax = row["fax"].ToString();
                objCentroTrabajo.correoElectronico = row["correoElectronico"].ToString();
                objCentroTrabajo.actividadEconomicaID = int.Parse(row["actividadEconomicaID"].ToString());
                objCentroTrabajo.tipoContrato = row["tipoContrato"].ToString();
                objCentroTrabajo.entidadDescripcion = centroTrabajo.entidadDescripcion;
                objCentroTrabajo.municipioDescripcion = centroTrabajo.municipioDescripcion;
                centrosTrabajo.Add(objCentroTrabajo);
            }
            return centrosTrabajo;
        }

        /// <summary>
        /// Método apra obtener los datos de un centro de trabajo.
        /// </summary>
        /// <param name="EmpresaID"></param>
        /// <param name="listCentroTrabajoID"></param>
        /// <returns></returns>
        public List<CentroTrabajoPOCO> ConsultarCentrosTrabajoPorEstablecimiento(int EmpresaID, List<int> listCentroTrabajoID)
        {
            string query = Diccionarios.DiccionarioDNE["BuscarCentrosTrabajo"];
            query += " where e.empresa_id = " + EmpresaID;
            query += " and c.centro_trabajo_id in (" + recuperarClavesCentrosTrabajo(listCentroTrabajoID) + ")";
            DataTable resultSet = OC.ObtenerConsulta(Enumeradores.ConexionesSatelitales.DNE, query);
            List<CentroTrabajoPOCO> centrosTrabajo = new List<CentroTrabajoPOCO>();
            foreach (DataRow row in resultSet.Rows)
            {
                CentroTrabajoPOCO objCentroTrabajo = new CentroTrabajoPOCO();
                objCentroTrabajo.centroTrabajoID = int.Parse(row["centroTrabajoID"].ToString());
                objCentroTrabajo.centroTrabajoNombre = row["centroTrabajoNombre"].ToString();
                objCentroTrabajo.rfc = row["rfc"].ToString();
                objCentroTrabajo.registroIMSS = row["registroIMSS"].ToString();
                objCentroTrabajo.curp = row["curp"].ToString();
                objCentroTrabajo.calleNumero = row["calleNumero"].ToString();
                objCentroTrabajo.colonia = row["colonia"].ToString();
                objCentroTrabajo.codigoPostal = row["codigoPostal"].ToString();
                objCentroTrabajo.telefono = row["telefono"].ToString();
                objCentroTrabajo.fax = row["fax"].ToString();
                objCentroTrabajo.correoElectronico = row["correoElectronico"].ToString();
                objCentroTrabajo.actividadEconomicaID = int.Parse(row["actividadEconomicaID"].ToString());
                objCentroTrabajo.tipoContrato = row["tipoContrato"].ToString();
                centrosTrabajo.Add(objCentroTrabajo);
            }
            return centrosTrabajo;
        }
        #endregion

        private string recuperarClavesCentrosTrabajo(List<int> listCentroTrabajoID)
        {
            StringBuilder resultado = new StringBuilder();

            foreach (var item in listCentroTrabajoID)
            {
                if (resultado.Length > 0)
                { 
                    resultado.Append(","); 
                }
                resultado.Append(item);
            }

            return resultado.ToString();
        }

        #region Metodos Comunes
        private List<CatalogoPOCO> SerializarCatalogoPoco(DataTable resultSet)
        {
            List<CatalogoPOCO> objResultSet = new List<CatalogoPOCO>();
            foreach (DataRow row in resultSet.Rows)
            {
                CatalogoPOCO objCatalogo = new CatalogoPOCO();
                objCatalogo.catalogoID = int.Parse(row["CatalogoID"].ToString());
                objCatalogo.catalogoDescripcion = row["CatalogoDescripcion"].ToString();
                objResultSet.Add(objCatalogo);
            }
            return objResultSet;
        }
        #endregion

    }
}
