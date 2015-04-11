using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.Framework;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Negocio
{
    public class EstablecimientoNEG : WorkUnit
    {
        #region Constructor
        public EstablecimientoNEG() { }

        public EstablecimientoNEG(SIRCEEntities context)
            : base(context)
        {
        }

        #endregion

        /// <summary>
        /// Método para obtener los establecimiento.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public List<EstablecimientoPOCO> ConsultarEstablecimiento(ListaCentrosTrabajo entidad, int EmpresaSIRCEID)
        {
            List<EstablecimientoPOCO> lista = new List<EstablecimientoPOCO>();
            try
            {
                //Obtengo todos los trabajadores y sus centros de trabajos
                EstablecimientoDAT datos = new EstablecimientoDAT(contextoSIRCE);
                lista = datos.ConsultarEstablecimientos(entidad, EmpresaSIRCEID);

                if (lista.Count == 0)
                {
                    return lista;
                }

                //Obtengo la clave original de los centros de trabajo del DNE
                var centrosTrabajoSIRCE = (from lt in lista
                                           select new
                                           {
                                               CentroTrabajoID = lt.CentroTrabajoID,
                                               CentroTrabajoSIRCEID = lt.CentroTrabajoSIRCEID
                                           }).Distinct().ToList();

                List<int> listCentrosTrabajos = new List<int>();
                Dictionary<int, int> dCentrosTrabajo = new Dictionary<int, int>();
                foreach (var item in centrosTrabajoSIRCE)
                {
                    listCentrosTrabajos.Add(item.CentroTrabajoID);
                    dCentrosTrabajo.Add(item.CentroTrabajoSIRCEID, item.CentroTrabajoID);
                }
                //Obtengo la clave de la empresa del DNE.
                EmpresaDAT empresaDAT = new EmpresaDAT(contextoSIRCE);

                int empresaID = empresaDAT.ConsultarEmpresaSIRCE(EmpresaSIRCEID);

                //Recupero la descripción de los centros de trabajo del DNE
                List<CentroTrabajoPOCO> listCentroTrabajoPOCO = new List<CentroTrabajoPOCO>();

                if (listCentrosTrabajos.Count == 0)
                {
                    return lista;
                }

                listCentroTrabajoPOCO = new UtileriaDAT().ConsultarCentrosTrabajoPorEstablecimiento(empresaID, listCentrosTrabajos);

                foreach (var item in lista)
                {
                    CatalogoCompuestoPOCO catalogoCompuestoPOCO = new CatalogoCompuestoPOCO();
                    string descripcionCentroTrabajo = string.Empty;

                    //Recupero la descripción del Centro de Trabajo del DNE
                    foreach (KeyValuePair<int, int> centroTrabajoSIRCEID in dCentrosTrabajo)
                    {
                        if (item.CentroTrabajoSIRCEID == centroTrabajoSIRCEID.Key)
                        {
                            descripcionCentroTrabajo = listCentroTrabajoPOCO.FirstOrDefault(x => x.centroTrabajoID == centroTrabajoSIRCEID.Value).centroTrabajoNombre;
                            break;
                        }
                    }

                    item.TrabajadorDescripcion = string.Format("{0} - {1} {2} {3} / {4}", item.CURP, item.Nombre, item.ApellidoPaterno, item.ApellidoMaterno, descripcionCentroTrabajo);
                    item.centroTrabajoNombre = descripcionCentroTrabajo;
                }
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return lista;
        }

        /// <summary>
        /// Método para obtener el listaCentroTrabajoID.
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public int ObtenerlistaCentroTrabajoID(int listaID, int centroTrabajoSIRCEID)
        {
            List<EstablecimientoPOCO> lista = new List<EstablecimientoPOCO>();
            int resultado = 0;
            try
            {
                ListaCentrosTrabajoDAT datos = new ListaCentrosTrabajoDAT(contextoSIRCE);
                resultado = datos.ObtenerlistaCentroTrabajoID(listaID, centroTrabajoSIRCEID);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para crear un establecimiento
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool CrearEstablecimiento(ListaCentrosTrabajo entidad, int centroTrabajoSIRCEID)
        {
            EstablecimientoDAT datos = new EstablecimientoDAT(contextoSIRCE);
            try
            {
                datos.CrearEstablecimiento(entidad, centroTrabajoSIRCEID);
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return true;
        }
    }
}
