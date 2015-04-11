using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using STPS.Framework;

namespace STPS.SIRCE.Web.Models
{
    public class CursoModel
    {
        public CursoModel() { }

        public int cursoId { get; set; }
        public string nombre { get; set; }
        public byte duracion { get; set; }
        public string fechaInicio {get;set;}
        public string fechaTermino {get;set;}
        public string numeroRegistroAgente {get;set;}

        public int areaId { get; set; }
        public byte tipoId { get; set; }
        public byte modalidadId { get; set; }
        public int objetivoId { get; set; }



        public int empresaSirceId { get; set; }
        public int Accion { get; set; }

        public Cursos curso { get; set; }
        public List<CursoPOCO> listaCursos { get; set; }
        public List<CatalogoPOCO> areas { get; set; }
        public List<CatalogoPOCO> tiposAgentes { get; set; }
        public List<CatalogoPOCO> modalidades { get; set; }
        public List<CatalogoPOCO> objetivos { get; set; }
        public List<CatalogoPOCO> tiposCRUD { get; set; }

        public GridPOCO<CursoPOCO> gridCursos = new GridPOCO<CursoPOCO>();

        public List<CatalogoPOCO> acciones { get; set; }

        // Encabezados del grid

        public bool ConsultarCatalogos()
        {
            using(CursoNEG negocio = new CursoNEG())
            {
                this.areas = negocio.ConsultarAreasTematicas();
                this.tiposAgentes = new UtileriaNeg().EnumeradorALista<Enumeradores.TipoAgenteCapacitador>();
                this.modalidades = new UtileriaNeg().EnumeradorALista<Enumeradores.ModalidadCapacitacion>();
                this.objetivos = negocio.ConsultarObjetivosCapacitacion();
                this.tiposCRUD = new UtileriaNeg().EnumeradorALista<Enumeradores.TipoCRUD>();
                AccionNEG objAccionNEG = new AccionNEG();
                this.acciones = objAccionNEG.consultarAcciones();
            }

            return true;
        }

        /// <summary>
        /// Método para crear un curso
        /// </summary>
        /// <returns></returns>
        public bool Crear()
        {
            this.curso = CrearEntidad();

            // Mandamos guardar al curso
            using (CursoNEG negocio = new CursoNEG())
            {
                negocio.Crear(this.curso);
            }

            return true;
        }

        /// <summary>
        /// Método para editar un curso
        /// </summary>
        /// <returns></returns>
        public bool Editar()
        {
            this.curso = CrearEntidad();
            // Mandamos guardar al curso
            using (CursoNEG negocio = new CursoNEG())
            {
                negocio.Editar(this.curso);
            }

            return true;
        }

        public Cursos CrearEntidad()
        {
            return new Cursos
            {  //ToDo
                CursoID = this.cursoId,
                EmpresaSIRCEID = this.empresaSirceId,
                Nombre = this.nombre,
                Duracion = this.duracion,
                AreaTematicaID = this.areaId,
                ObjetivoCapacitacionID = this.objetivoId,
                ModalidadCapacitacionID = this.modalidadId,
                TipoAgenteCapacitadorID = this.tipoId,
                FechaInicio = Utilidades.ConvertirFecha(this.fechaInicio),
                FechaTermino = Utilidades.ConvertirFecha(this.fechaTermino),
                RegistroAgenteExterno = this.numeroRegistroAgente
            };
        }



        /// <summary>
        /// Método para eliminar un curso
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Eliminar()
        {
            // Mandamos a eliminar la entidad
            using (CursoNEG negocio = new CursoNEG())
            {
                negocio.Eliminar(this.cursoId);
            }

            return true;
        }

        /// <summary>
        /// Método para consultar los cursos de la empresa
        /// </summary>
        /// <returns></returns>
        public bool ConsultarCursos()
        {

            using (CursoNEG negocio = new CursoNEG())
            {
                // Mandamos a configurar el grid.
                ConfigurarControlGrid();

                // Obtenemos la lista de cursos
                this.listaCursos = negocio.ConsultarCursos(new Cursos { EmpresaSIRCEID = this.empresaSirceId });

                // Cargamos el grid con los cursos.
                this.gridCursos.datos = this.listaCursos;

            }

            return true;
        }

        /// <summary>
        /// Método para consultar un curso
        /// </summary>
        /// <returns></returns>
        public bool Consultar()
        {
            using (CursoNEG negocio = new CursoNEG())
            {
                this.curso = new Cursos() { CursoID = this.cursoId };
                negocio.Consultar(this.curso);

                this.empresaSirceId = this.curso.EmpresaSIRCEID;
                this.nombre = this.curso.Nombre;
                this.duracion = this.curso.Duracion;
                this.areaId = this.curso.AreaTematicaID;
                this.fechaInicio = this.curso.FechaInicio.ToString("dd/MM/yyyy");
                this.fechaTermino = this.curso.FechaTermino.ToString("dd/MM/yyyy");
                this.tipoId = this.curso.TipoAgenteCapacitadorID;
                this.numeroRegistroAgente = this.curso.RegistroAgenteExterno;
                this.modalidadId = this.curso.ModalidadCapacitacionID;
                this.objetivoId = this.curso.ObjetivoCapacitacionID;

                this.curso = null;
            }

            return true;
        }

        // Método para Generar la estructura y configuración del Grid
        private void ConfigurarControlGrid()
        {
            // 1. Primero agregamos los titulos que debe llevar el Grid
            AgregarTitulosGrid();
            // 2. Segundo agregamos las propiedades de la "CLASE POCO" que va a utilizar el Grid
            AsignarPropiedadesGrid();
            // 3. Tercero asignamos la propiedad del "MODELO" que utilizaremos para colocar el 
            //    identificador del renglón seleccionado en el grid.
            AsignarIdRenglonGrid();
            // 4. Asignar los íconos de acción que se van a utilizar en el grid.
            AsignarIconosGrid();
        }

        // Método para asignar la propiedad del modelo donde se guardará el identificador del renglón seleccionado en el grid
        private void AsignarIdRenglonGrid()
        {
            this.gridCursos.columnaID = "CursoId";
        }

        // Método para asignar el ícono de "Crear" al grid.
        private void AsignarIconosGrid()
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            // Se declaran los botones del CRUD
            this.gridCursos.iconosGrid = new List<IconosGridPOCO>();

            // Configuramos el ícono "Crear"
            IconosGridPOCO agregar = new IconosGridPOCO();
            agregar.icono = "plus";
            agregar.tooltip = "Agregar un Curso";
            agregar.callback = "accionarClick";
            agregar.enabled = true;
            agregar.accion = objAccionNEG.consultarValorAccion(acciones, "Crear");
            this.gridCursos.iconosGrid.Add(agregar);

            // Configuramos el ícono "Consultar"
            IconosGridPOCO consultar = new IconosGridPOCO();
            consultar.icono = "search";
            consultar.tooltip = "Consultar un Curso";
            consultar.callback = "accionarClick";
            consultar.enabled = false;
            consultar.accion = objAccionNEG.consultarValorAccion(acciones, "Consultar");
            this.gridCursos.iconosGrid.Add(consultar);

            // Configuramos el ícono "Editar"
            IconosGridPOCO editar = new IconosGridPOCO();
            editar.icono = "pencil";
            editar.tooltip = "Editar un Curso";
            editar.callback = "accionarClick";
            editar.enabled = false;
            editar.accion = objAccionNEG.consultarValorAccion(acciones, "Editar");
            this.gridCursos.iconosGrid.Add(editar);

            // Configuramos el ícono "Eliminar"
            IconosGridPOCO eliminar = new IconosGridPOCO();
            eliminar.icono = "remove";
            eliminar.tooltip = "Eliminar un Curso";
            eliminar.callback = "accionarClick";
            eliminar.enabled = false;
            eliminar.accion = objAccionNEG.consultarValorAccion(acciones, "Eliminar"); 
            this.gridCursos.iconosGrid.Add(eliminar);
        }

        // Se indican la propiedades de la clase POCO que va a utilizar el grid.
        // Se deben colocar los nombres de las propiedades exactamente igual que como estan declarados en la clase POCO
        private void AsignarPropiedadesGrid()
        {
            this.gridCursos.columnas = new List<string>();
            this.gridCursos.columnas.Add("Nombre");
            this.gridCursos.columnas.Add("Duracion");
            this.gridCursos.columnas.Add("AreaTematica");
            this.gridCursos.columnas.Add("FechaTermino");
        }

        // Se configuran los títulos que debe mostrar el Grid
        private void AgregarTitulosGrid()
        {
            //Se declaran los titulos de la tabla
            this.gridCursos.encabezados = new List<string>();
            this.gridCursos.encabezados.Add("Nombre del curso");
            this.gridCursos.encabezados.Add("Duración (hrs)");
            this.gridCursos.encabezados.Add("Area Temática");
            this.gridCursos.encabezados.Add("Fecha de término");
        }

        public int ConsultarAccion(string accion)
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            return objAccionNEG.consultarValorAccion(this.acciones, accion);
        }
    }
}