using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System.Globalization;
using STPS.Framework;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Web.Models
{
    public class TrabajadorModel
    {
        public int trabajadorID { get; set; }
        public string curp { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int ocupacionID { get; set; }
        public int entidadFederativaID { get; set; }
        public int municipioID { get; set; }
        public Nullable<int> escolaridadID { get; set; }
        public Nullable<int> institucionesEducativasID { get; set; }
        public Nullable<int> comprobanteEstudioID { get; set; }
        public int centroTrabajoSIRCEID { get; set; }
        public int genero { get; set; }
        public string fechaNacimiento { get; set; }
        public int empresaID { get; set; }
        public int accion { get; set; }
        public List<NormaTrabajadorPOCO> normaTrabajador { get; set; }

        public bool verificaCURP { get; set; }

        public List<CatalogoPOCO> ocupaciones { get; set; }
        public List<CatalogoPOCO> normasCompetencia { get; set; }
        public List<CatalogoPOCO> escolaridades { get; set; }
        public List<CatalogoPOCO> institucioEducativas { get; set; }
        public List<CatalogoPOCO> documentosProbatorios { get; set; }
        public List<CatalogoPOCO> entidadesFederativas { get; set; }
        public List<CatalogoPOCO> municipios { get; set; }
        public List<CatalogoPOCO> generos { get; set; }

        public List<TrabajadorPOCO> listaTrabajadores { get; set; }

        public GridPOCO<TrabajadorPOCO> gridTrabajadores = new GridPOCO<TrabajadorPOCO>();

        public GridPOCO<NormaTrabajadorPOCO> gridNormasEspecialidades = new GridPOCO<NormaTrabajadorPOCO>();

        public List<CatalogoPOCO> acciones { get; set; }

        /// <summary>
        /// Método para obtener todos los catálogos
        /// </summary>
        /// <returns></returns>
        public bool ConsultarCatalogos()
        {
            TrabajadorNEG negocio = new TrabajadorNEG();
            this.ocupaciones = negocio.ConsultarOcupaciones();
            this.escolaridades = negocio.ConsultarEscolaridades();
            this.institucioEducativas = negocio.ConsultarInstitucionesEducativa();
            this.documentosProbatorios = negocio.ConsultarDocumentosProbatorios();
            this.entidadesFederativas = negocio.ConsultarEntidades();
            this.normasCompetencia = negocio.ConsultarNormaCompetencia();
            this.generos = negocio.ConsultarGeneros();
            this.normaTrabajador = new List<NormaTrabajadorPOCO>();
            this.gridNormasEspecialidades.datos = this.normaTrabajador;
            propiedadesGridNormas();
            this.acciones = new AccionNEG().consultarAcciones();
            return true;
        }

        public void ConsultarMunicipiosTrabajador()
        {
            this.municipios = new UtileriaNeg().ConsultarMunicipios(this.entidadFederativaID);
        }

        /// <summary>
        /// Método para obtener los datos del trabajador
        /// </summary>
        /// <returns></returns>
        public bool ConsultarTrabajadores(int centroTrabajoSIRCEID)
        {
            bool resultado = true;
            using (TrabajadorNEG negocio = new TrabajadorNEG())
            {
                Trabajadores trabajadores = new Trabajadores();
                trabajadores.CentroTrabajoSIRCEID = centroTrabajoSIRCEID;
                this.listaTrabajadores = negocio.ConsultarTrabajadoresPOCO(trabajadores.CentroTrabajoSIRCEID);
                this.gridTrabajadores.datos = this.listaTrabajadores;
                trabajadores = null;

                propiedadesGrid();
            }
            return resultado;
        }

        public bool ConsultaTrabajador()
        {
                Trabajadores trabajador = new TrabajadorNEG().ConsultarTrabajador(this.trabajadorID );

                this.curp = trabajador.CURP;
                this.nombre = trabajador.Nombre;
                this.apellidoPaterno = trabajador.ApellidoPaterno;
                this.apellidoMaterno = trabajador.ApellidoMaterno;
                this.ocupacionID = trabajador.OcupacionID;
                this.entidadFederativaID = trabajador.EntidadFederativaID;
                this.municipioID = trabajador.MunicipioID;
                this.escolaridadID = trabajador.EscolaridadID;
                this.institucionesEducativasID = trabajador.InstitucionesEducativasID;
                this.comprobanteEstudioID = trabajador.ComprobanteEstudioID;
                this.centroTrabajoSIRCEID = trabajador.CentroTrabajoSIRCEID;

                this.genero = trabajador.Genero;
                this.fechaNacimiento = trabajador.FechaNacimiento.ToShortDateString();

                this.normaTrabajador = new List<NormaTrabajadorPOCO>();
                foreach (var item in trabajador.NormaTrabajador)
                {
                    NormaTrabajadorPOCO lista = new NormaTrabajadorPOCO();
                    lista.NormaTrabajadorID = item.NormaTrabajadorID;
                    lista.TrabajadorID = item.TrabajadorID;
                    lista.NormaCompetenciaID = item.NormaCompetenciaID;
                    lista.FechaEmision = item.FechaEmision.ToShortDateString();
                    lista.NormaCompetenciaDescripcion = this.normasCompetencia.First(x => x.catalogoID == item.NormaCompetenciaID).catalogoDescripcion;
                    lista.Eliminado = item.Eliminado;

                    this.normaTrabajador.Add(lista);
                }
                ConsultarMunicipiosTrabajador();

                this.gridNormasEspecialidades.datos = this.normaTrabajador;
                propiedadesGridNormas();
            return true;
        }

        /// <summary>
        /// Método para englobar las propiedades del grid.
        /// </summary>
        private void propiedadesGrid()
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            //Se declaran los titulos de la tabla
            this.gridTrabajadores.encabezados = new List<string>();
            this.gridTrabajadores.encabezados.Add("CURP");
            this.gridTrabajadores.encabezados.Add("Nombre del trabajador");
            this.gridTrabajadores.encabezados.Add("Apellido Paterno");
            this.gridTrabajadores.encabezados.Add("Apellido Materno");
            this.gridTrabajadores.encabezados.Add("Fecha de Nacimiento");
            this.gridTrabajadores.encabezados.Add("Ocupación Especificas");
            this.gridTrabajadores.encabezados.Add("Nivel máximo de estudios");
            this.gridTrabajadores.encabezados.Add("Instituciones Educativas");
            this.gridTrabajadores.encabezados.Add("Documentos Probatorios");
            this.gridTrabajadores.encabezados.Add("Genero");

            //se declaran los valores de la tabla
            this.gridTrabajadores.columnas = new List<string>();
            this.gridTrabajadores.columnas.Add("CURP");
            this.gridTrabajadores.columnas.Add("Nombre");
            this.gridTrabajadores.columnas.Add("ApellidoPaterno");
            this.gridTrabajadores.columnas.Add("ApellidoMaterno");
            this.gridTrabajadores.columnas.Add("FechaNacimiento");
            this.gridTrabajadores.columnas.Add("OcupacionDescripcion");
            this.gridTrabajadores.columnas.Add("EscolaridadDescripcion");
            this.gridTrabajadores.columnas.Add("InstitucionesEducativasDescripcion");
            this.gridTrabajadores.columnas.Add("DocumentosProbatoriosDescripcion");
            this.gridTrabajadores.columnas.Add("GeneroDescripcion");

            //se declara el campo clave para recuperar el valor del grid seleccioando
            this.gridTrabajadores.columnaID = "TrabajadorID";

            //se declaran los botones del CRUD
            this.gridTrabajadores.iconosGrid = new List<IconosGridPOCO>();
            //Agregar Trabajador
            IconosGridPOCO agregar = new IconosGridPOCO();
            agregar.icono = "plus";
            agregar.tooltip = "Agregar un Trabajador";
            agregar.callback = "accionarClick";
            agregar.enabled = true;
            agregar.accion = objAccionNEG.consultarValorAccion(acciones, "Crear");
            this.gridTrabajadores.iconosGrid.Add(agregar);

            //Consultar Trabajador
            IconosGridPOCO consultar = new IconosGridPOCO();
            consultar.icono = "search";
            consultar.tooltip = "Consultar un Trabajador";
            consultar.callback = "accionarClick";
            consultar.enabled = false;
            consultar.accion = objAccionNEG.consultarValorAccion(acciones, "Consultar");
            this.gridTrabajadores.iconosGrid.Add(consultar);

            //Editar Trabajador
            IconosGridPOCO editar = new IconosGridPOCO();
            editar.icono = "pencil";
            editar.tooltip = "Editar un Trabajador";
            editar.callback = "accionarClick";
            editar.enabled = false;
            editar.accion = objAccionNEG.consultarValorAccion(acciones, "Editar");
            this.gridTrabajadores.iconosGrid.Add(editar);

            //Editar Trabajador
            IconosGridPOCO eliminar = new IconosGridPOCO();
            eliminar.icono = "remove";
            eliminar.tooltip = "Eliminar un Trabajador";
            eliminar.callback = "accionarClick";
            eliminar.enabled = false;
            eliminar.accion = objAccionNEG.consultarValorAccion(acciones, "Eliminar"); //(int)Enumeradores.TipoCRUD.Eliminar;
            this.gridTrabajadores.iconosGrid.Add(eliminar);
        }

        /// <summary>
        /// Método para englobar las propiedades del grid de las normas.
        /// </summary>
        private void propiedadesGridNormas()
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            //Se declaran los titulos de la tabla
            this.gridNormasEspecialidades.encabezados = new List<string>();
            this.gridNormasEspecialidades.encabezados.Add("Norma o Estandar");
            this.gridNormasEspecialidades.encabezados.Add("Fecha Emision");

            //se declaran los valores de la tabla
            this.gridNormasEspecialidades.columnas = new List<string>();
            this.gridNormasEspecialidades.columnas.Add("NormaCompetenciaDescripcion");
            this.gridNormasEspecialidades.columnas.Add("FechaEmision");

            //se declara el campo clave para recuperar el valor del grid seleccioando
            this.gridNormasEspecialidades.columnaID = "NormaCompetenciaID";

            //se declaran los botones del CRUD
            this.gridNormasEspecialidades.iconosGrid = new List<IconosGridPOCO>();

            //Eliminar Norma Competencia
            IconosGridPOCO eliminar = new IconosGridPOCO();
            eliminar.icono = "remove";
            eliminar.tooltip = "Eliminar una norma o estandar";
            eliminar.callback = "AccionarNormasClick";
            eliminar.enabled = false;
            eliminar.accion = objAccionNEG.consultarValorAccion(acciones, "Editar");
            this.gridNormasEspecialidades.iconosGrid.Add(eliminar);
        }
        
        public bool CrearEntidad(Trabajadores trabajador)
        {
            trabajador.CURP = this.curp;
            trabajador.Nombre = this.nombre;
            trabajador.ApellidoPaterno = this.apellidoPaterno;
            trabajador.ApellidoMaterno = this.apellidoMaterno;
            trabajador.OcupacionID = this.ocupacionID;
            trabajador.EntidadFederativaID = this.entidadFederativaID;
            trabajador.MunicipioID = this.municipioID;
            trabajador.EscolaridadID = this.escolaridadID;
            trabajador.InstitucionesEducativasID = this.institucionesEducativasID;
            trabajador.ComprobanteEstudioID = this.comprobanteEstudioID;
            trabajador.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
            trabajador.Genero = Convert.ToByte(this.generos.First(x => x.catalogoDescripcion.Substring(0, 1).ToUpper() == this.curp.Substring(10, 1).ToString().ToUpper()).catalogoID);
            trabajador.FechaNacimiento = Utilidades.ConvertirFecha(string.Format("{2}/{1}/{0}", this.curp.Substring(4, 2), this.curp.Substring(6, 2), this.curp.Substring(8, 2)));
            trabajador.VerificaCURP = false;

            foreach (var item in this.gridNormasEspecialidades.datos)
            {
                trabajador.NormaTrabajador.Add(new NormaTrabajador
                {
                    TrabajadorID = item.TrabajadorID,
                    NormaTrabajadorID = item.NormaTrabajadorID,
                    NormaCompetenciaID = item.NormaCompetenciaID,
                    FechaEmision = Utilidades.ConvertirFecha(item.FechaEmision),
                    Eliminado = item.Eliminado
                });
            }
            return true;
        }

        public bool ConsultarAcciones()
        {
            //Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            return true;
        }

        public bool PostTrabajador()
        {
            TrabajadorNEG objTrabajadorNeg = new TrabajadorNEG();
            bool success = true;
            Trabajadores trabajador = new Trabajadores();
            if (this.accion != (from a in acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
            {
                trabajador = objTrabajadorNeg.ConsultarTrabajador(this.trabajadorID);
            }
            if (this.accion == (from a in acciones where a.catalogoDescripcion.Equals("Eliminar") select a.catalogoID).FirstOrDefault())
            {
                trabajador.Eliminado = true;
            }
            if (this.accion != (from a in acciones where a.catalogoDescripcion.Equals("Eliminar") select a.catalogoID).FirstOrDefault())
            {
                success = CrearEntidad(trabajador);
            }
            success = objTrabajadorNeg.PostTrabajador(trabajador);
            return success;
        }

        public bool ValidaCURP(int centroTrabajoSIRCEID)
        {
            bool resultado = true;

            using (TrabajadorNEG negocio = new TrabajadorNEG())
            {
                // Validamos la CURP por empresa.
                Trabajadores trabajadores = new Trabajadores();
                trabajadores.CURP = this.curp;
                trabajadores.CentroTrabajoSIRCEID = centroTrabajoSIRCEID;
                negocio.ValidaCURP(trabajadores);
                this.verificaCURP = trabajadores.VerificaCURP;
                trabajadores = null;
            }
            return resultado;
        }
    }
}