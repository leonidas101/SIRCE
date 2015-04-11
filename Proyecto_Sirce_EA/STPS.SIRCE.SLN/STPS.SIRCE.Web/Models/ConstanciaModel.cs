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
    public class ConstanciaModel
    {
        public ConstanciaModel() { }

        public ListaCentrosTrabajo constancia { get; set; }

        public int listaCentroTrabajoID { get; set; }
        public int listaID { get; set; }
        public int centroTrabajoSIRCEID { get; set; }

        public int EmpresaSIRCEID { get; set; }
        public int Accion { get; set; }

        public List<ConstanciaPOCO> listaConstancias { get; set; }
        public List<CatalogoCompuestoPOCO> listaTrabajadores { get; set; }
        public List<CatalogoPOCO> listaCursos { get; set; }

        public GridPOCO<ConstanciaPOCO> gridConstancias = new GridPOCO<ConstanciaPOCO>();

        public List<CatalogoPOCO> acciones { get; set; }

        public ListaPOCO lista { get; set; }

        /// <summary>
        /// Método para obtener los trabajadores de un centro de trabajo
        /// </summary>
        /// <returns></returns>
        public bool ConsultarTrabajadores(int EmpresaSIRCEID, int centroTrabajoSIRCEID)
        {
            using (TrabajadorNEG negocio = new TrabajadorNEG())
            {
                //Obtenemos lista de trabajadores
                this.listaTrabajadores = negocio.ConsultarTrabajadoresCatalogoPOCO(EmpresaSIRCEID, centroTrabajoSIRCEID);
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
        /// Método para obtener las constancias de una lista
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarConstancias()
        {
            using (ConstanciaNEG negocio = new ConstanciaNEG())
            {
                ListaCentrosTrabajo listaCentrosTrabajo = new ListaCentrosTrabajo();

                listaCentrosTrabajo.ListaID = this.listaID;
                listaCentrosTrabajo.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
                listaCentrosTrabajo.ListaCentroTrabajoID = negocio.ObtenerlistaCentroTrabajoID(this.listaID, this.centroTrabajoSIRCEID);

                this.listaCentroTrabajoID = listaCentrosTrabajo.ListaCentroTrabajoID;
                this.listaConstancias = negocio.ConsultarConstancias(listaCentrosTrabajo, this.EmpresaSIRCEID);
                this.gridConstancias.datos = this.listaConstancias;
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
        /// Método para agregar una constancia
        /// </summary>
        /// <returns></returns>
        public bool CrearConstancia()
        {
            this.constancia = CrearEntidad();

            using (ConstanciaNEG negocio = new ConstanciaNEG())
            {
                negocio.CrearConstancia(this.constancia);
            }
            return true;
        }

        /// <summary>
        /// Método para crear una entidad constancia
        /// </summary>
        /// <returns></returns>
        public ListaCentrosTrabajo CrearEntidad()
        {
            ListaCentrosTrabajo listaCentrosTrabajo = new ListaCentrosTrabajo();

            using (ConstanciaNEG negocio = new ConstanciaNEG())
            {
                listaCentrosTrabajo.ListaID = this.listaID;
                listaCentrosTrabajo.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
                listaCentrosTrabajo.ListaCentroTrabajoID = negocio.ObtenerlistaCentroTrabajoID(this.listaID, this.centroTrabajoSIRCEID);

                foreach (var item in this.gridConstancias.datos)
                {
                    listaCentrosTrabajo.Constancias.Add(new Constancias
                    {
                        ConstanciaID = 0, //item.ConstanciaID,
                        ListaCentroTrabajoID = listaCentrosTrabajo.ListaCentroTrabajoID,
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
            this.gridConstancias.encabezados = new List<string>();
            this.gridConstancias.encabezados.Add("Trabajador");
            this.gridConstancias.encabezados.Add("Curso");

            //se declaran los valores de la tabla
            this.gridConstancias.columnas = new List<string>();
            this.gridConstancias.columnas.Add("TrabajadorDescripcion");
            this.gridConstancias.columnas.Add("CursoDescripcion");

            //se declara el campo clave para recuperar el valor del grid seleccioando
            this.gridConstancias.columnaID = "ConstanciaID";

            //se declaran los botones del CRUD
            this.gridConstancias.iconosGrid = new List<IconosGridPOCO>();

            //Editar Trabajador
            IconosGridPOCO crear = new IconosGridPOCO();
            crear.icono = "remove";
            crear.tooltip = "Eliminar una Constancia";
            crear.callback = "accionarClick";
            crear.enabled = false;
            crear.accion = objAccionNEG.consultarValorAccion(acciones, "Eliminar");
            this.gridConstancias.iconosGrid.Add(crear);
        }

        public int ConsultarAccion(string accion)
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            return objAccionNEG.consultarValorAccion(this.acciones, accion);
        }
    }
}
