using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public static class Diccionarios
    {

        #region DiccionarioDNE
        public static readonly Dictionary<string, string> DiccionarioDNE
            = new Dictionary<string, string>
        {
            #region Seleccion Empresas
                        { "BuscarEmpresas", "select distinct e.empresa_id, ct.centro_trabajo_id, ct.ct_colonia + ' '+ " 
                        + "ct_calle as domicilio, e.emp_rfc as rfc, ct.ct_imss_registro as registroIMSS, " +
                        "ct.ct_nombre_comercial as empresa " +
                        "from empresa e " +
                        "inner join centro_trabajo ct on ct.empresa_id=e.empresa_id " +
                        "where ct.ct_nombre_comercial = e.emp_nombre " },
            #endregion
            #region Seleccion de Centros de Trabajo
                        { "BuscarCentrosTrabajo", "select c.centro_trabajo_id as centroTrabajoID, c.ct_nombre_comercial as centroTrabajoNombre, " 
                        + "e.emp_rfc as rfc, c.ct_imss_registro as registroIMSS, '' as curp, c.ct_calle + ' ' + c.ct_num_exterior " +
                        "+' ' + c.ct_num_interior as calleNumero, c.ct_colonia as colonia, c.ct_cp as codigoPostal, " +
                        "c.ct_telefono as telefono, c.ct_fax as fax, c.ct_email as correoElectronico, " +
                        "isnull(c.ct_actividad_scian, 0) as actividadEconomicaID, CASE WHEN(c.ct_contrato_colectivo = 1) THEN 'Contrato Colectivo' " +
                        "WHEN(c.ct_contrato_individual = 1) THEN 'Contrato Individual' WHEN(c.ct_contrato_ley = 1) THEN 'Contrato de Ley' " +
                        "ElSE 'Contrato Indefinido' END AS tipoContrato from centro_trabajo c inner join empresa e on e.empresa_id = c.empresa_id "}
            #endregion
        };
        #endregion

        #region DiccionarioCI
        public static readonly Dictionary<string, string> DiccionarioCI = new Dictionary<string, string>
        {
            #region Buscar Entidades Federativas
                        { "BuscarEntidadesFederativas", "select cen_cve_edorep as catalogoID, cen_descripcion as catalogoDescripcion " 
                        + "from cat_entidades"},
            #endregion
            #region Buscar Municipios
                        { "BuscarMunicipios", "select cmu_cve_municipio as catalogoID, cmu_descripcion as catalogoDescripcion " 
                        + "from cat_municipios"},
            #endregion
            #region Buscar SCIAN
                        { "BuscarSCIAN", "select cae_id as actividadEconomicaID, cae_descripcion as actividadEconomica  from cat_scian "},
            #endregion
            #region Buscar Unidades Responsables
                        { "ConsultarUnidadResponsable", "select cur.cur_cve_ur as unidadResponsableID, cur.cur_descripcion as unidadResponsableDesc, "
                        + "ce.cen_descripcion as entidadDesc from cat_unidad_respon cur inner join cat_entidades ce on ce.cen_cve_edorep = cur.cur_cve_edorep "}
            #endregion
        };
        #endregion

        #region DiccionarioSIRCE
        public static readonly Dictionary<string, string> DiccionarioEnumeradores = new Dictionary<string, string>
        {
            #region Seleccion Instituciones Educativas
                        { "BuscarInstitucionesEducativas", "" },{ "1", "Pública" },{ "2", "Privada" }
            #endregion
        };
        #endregion
    }
}
