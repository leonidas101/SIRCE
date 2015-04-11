using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Entidades.POCOS;
using STPS.SIRCE.Negocio;
using STPS.Framework;

namespace STPS.SIRCE.Web.Models
{
    public class EstablecimientoModel
    {
        public EstablecimientoModel() { }

        public ListaCentrosTrabajo establecimiento { get; set; }

        public int listaCentroTrabajoID { get; set; }
        public int listaID { get; set; }
        public int centroTrabajoSIRCEID { get; set; }

        public int EmpresaSIRCEID { get; set; }
        public int Accion { get; set; }

        public List<EstablecimientoPOCO> listaEstablecimientos { get; set; }
        public List<CatalogoCompuestoPOCO> listaTrabajadores { get; set; }
        public List<CatalogoPOCO> listaCursos { get; set; }

        public GridPOCO<EstablecimientoPOCO> gridEstablecimientos = new GridPOCO<EstablecimientoPOCO>();

        public List<CatalogoPOCO> acciones { get; set; }

        public ListaPOCO lista { get; set;  }

        /// <summary>
        /// Método para obtener los trabajadores con su centro de trabajo al que pertenecen
        /// </summary>
        /// <returns></returns>
        public bool ConsultarTrabajadores(int EmpresaSIRCEID, int centroTrabajoSIRCEID)
        {
            using (TrabajadorNEG negocio = new TrabajadorNEG())
            {
                //Obtenemos lista de trabajadores
                this.listaTrabajadores = negocio.ConsultarTrabajadoresCatalogoCompuestoPOCO(EmpresaSIRCEID, centroTrabajoSIRCEID);
            }
            return true;
        }

        /// <summary>
        /// Método para obtener los cursos de una empresa
        /// </summary>
        /// <param name="centroTrabajoSIRCEID"></param>
        /// <returns></returns>
        public bool ConsultarCursos(int empresaSirceId)
        {
            using (CursoNEG negocio = new CursoNEG())
            {
                // Obtenemos la lista de cursos
                this.listaCursos = negocio.ConsultarCursos(empresaSirceId);
            }

            return true;
        }

        /// <summary>
        /// Método para obtener el establecimiento de una lista
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarEstablecimientos()
        {
            using (EstablecimientoNEG negocio = new EstablecimientoNEG())
            {
                ListaCentrosTrabajo listaCentrosTrabajo = new ListaCentrosTrabajo();

                listaCentrosTrabajo.ListaID = this.listaID;
                listaCentrosTrabajo.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
                listaCentrosTrabajo.ListaCentroTrabajoID = negocio.ObtenerlistaCentroTrabajoID(this.listaID, this.centroTrabajoSIRCEID);

                this.listaCentroTrabajoID = listaCentrosTrabajo.ListaCentroTrabajoID;
                this.listaEstablecimientos = negocio.ConsultarEstablecimiento(listaCentrosTrabajo, this.EmpresaSIRCEID);
                this.gridEstablecimientos.datos = this.listaEstablecimientos;
                ConfigurarControlGrid();
            }

            return true;
        }

        /// <summary>
        /// Método para obtner los datos de la lista.
        /// </summary>
        /// <returns></returns>
        public bool ConsultarLista()
        {
            TableroControlNEG objTableroControlNeg = new TableroControlNEG();
            Listas listaEntidad = objTableroControlNeg.consultarLista(this.listaID);

            if (listaEntidad != null)
            {
                this.lista = new ListaPOCO();
                this.lista.nombreLista = listaEntidad.Nombre;
                this.lista.fechaPresentacion = Utilidades.ConvertirTexto(listaEntidad.FechaPresentacion);
                this.lista.numeroConstancias = listaEntidad.NumeroConstancias;
            }
            return true;
        }

        /// <summary>
        /// Método para agregar un establecimiento
        /// </summary>
        /// <returns></returns>
        public bool CrearEstablecimiento(int centroTrabajoSIRCEID)
        {
            this.establecimiento = CrearEntidad();

            using (EstablecimientoNEG negocio = new EstablecimientoNEG())
            {
                negocio.CrearEstablecimiento(this.establecimiento, centroTrabajoSIRCEID);
            }
            return true;
        }

        /// <summary>
        /// Método para crear una entidad de establecimiento
        /// </summary>
        /// <returns></returns>
        public ListaCentrosTrabajo CrearEntidad()
        {
            ListaCentrosTrabajo listaCentrosTrabajo = new ListaCentrosTrabajo();
            int listaCentroTrabajoID = 0;
            ListaCentrosTrabajoNEG centroTrabajo = new ListaCentrosTrabajoNEG();

            using (EstablecimientoNEG negocio = new EstablecimientoNEG())
            {
                
                listaCentrosTrabajo.ListaID = this.listaID;
                listaCentrosTrabajo.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
                listaCentrosTrabajo.ListaCentroTrabajoID = listaCentroTrabajoID;
                
                foreach (var item in this.gridEstablecimientos.datos)
                {
                    listaCentroTrabajoID = negocio.ObtenerlistaCentroTrabajoID(this.listaID, item.CentroTrabajoSIRCEID);
                    if (listaCentroTrabajoID == 0)
                    {
                        //Debo de crear el registro en ListaCentrosTrabajo
                        ListaCentrosTrabajo entidad = new ListaCentrosTrabajo();
                        entidad.ListaID = this.listaID;
                        entidad.CentroTrabajoSIRCEID = item.CentroTrabajoSIRCEID;
                        centroTrabajo.postListaCentroTrabajo(entidad);
                        listaCentroTrabajoID = entidad.ListaCentroTrabajoID;
                    }

                    listaCentrosTrabajo.Constancias.Add(new Constancias
                    {
                        ConstanciaID = 0, //item.ConstanciaID,
                        ListaCentroTrabajoID = listaCentroTrabajoID,
                        TrabajadorID = item.trabajadorID,
                        CursoID = item.cursoID,
                        Eliminado = item.Eliminado
                    });
                }
            }
            return listaCentrosTrabajo;
        }

        /// <summary>
        /// Método para englobar las propiedades del grid de las normas.
        /// </summary>
        private void ConfigurarControlGrid()
        {
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            //Se declaran los titulos de la tabla
            this.gridEstablecimientos.encabezados = new List<string>();
            this.gridEstablecimientos.encabezados.Add("Trabajador");
            this.gridEstablecimientos.encabezados.Add("Curso");

            //se declaran los valores de la tabla
            this.gridEstablecimientos.columnas = new List<string>();
            this.gridEstablecimientos.columnas.Add("TrabajadorDescripcion");
            this.gridEstablecimientos.columnas.Add("CursoDescripcion");

            //se declara el campo clave para recuperar el valor del grid seleccioando
            this.gridEstablecimientos.columnaID = "ConstanciaID";

            //se declaran los botones del CRUD
            this.gridEstablecimientos.iconosGrid = new List<IconosGridPOCO>();

            //Editar Trabajador
            IconosGridPOCO crear = new IconosGridPOCO();
            crear.icono = "remove";
            crear.tooltip = "Eliminar un establecimiento";
            crear.callback = "accionarClick";
            crear.enabled = false;
            crear.accion = objAccionNEG.consultarValorAccion(acciones, "Eliminar");
            this.gridEstablecimientos.iconosGrid.Add(crear);
        }

        public int ConsultarAccion(string accion)
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            return objAccionNEG.consultarValorAccion(this.acciones, accion);
        }
    }
}