using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STPS.SIRCE.Negocio;
using STPS.SIRCE.Entidades;
using System.IO;
using STPS.SIRCE.Entidades.POCOS;
using STPS.Framework.Excel;

namespace STPS.SIRCE.Web.Models
{
    public class CargaMasivaModel
    {
        public CargaMasivaModel() { }

        #region Propiedades
        public int ListaCentroTrabajoID { get; set; }
        public int ListaID { get; set; }
        public int CentroTrabajoSIRCEID { get; set; }
        public int EmpresaID { get; set; }
        public string NombreEmpresa { get; set; }
        public string RFCEmpresa { get; set; }
        public string FolioDC { get; set; }
        public string NombreArchivo { get; set; }
        public List<ConstanciaCMPOCO> ConstanciasValidas { get; set; }
        public List<ConstanciaCMPOCO> ConstanciasError { get; set; }

        public GridPOCO<ConstanciaCMPOCO> gridConstanciasValidas = new GridPOCO<ConstanciaCMPOCO>();

        public GridPOCO<ConstanciaCMPOCO> gridConstanciasError = new GridPOCO<ConstanciaCMPOCO>();

        public GridPOCO<TotalesPOCO> gridTotales = new GridPOCO<TotalesPOCO>();
        public List<CatalogoPOCO> acciones { get; set; }
        public List<TotalesPOCO> Totales { get; set; }
        #endregion

        #region Metodos

        public string GurardarArchivo(HttpPostedFileBase archivo)
        {
            string rutaContenedor = @"~\Content\Temp\";
            string nombreArchivo = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(archivo.FileName), Guid.NewGuid().ToString(), Path.GetExtension(archivo.FileName));
            string rutaArchivo = System.Web.HttpContext.Current.Server.MapPath(rutaContenedor + nombreArchivo);

            archivo.SaveAs(rutaArchivo);

            return nombreArchivo;
        }

        public bool ProcesarArchivo(string nombreArchivo)
        {
            CargaMasivaNEG negocio = new CargaMasivaNEG();
            string rutaContenedor = @"~\Content\Temp\";
            string ruta = System.Web.HttpContext.Current.Server.MapPath(rutaContenedor + NombreArchivo);

            ExcelData datosExcel = negocio.ProcesarArchivo(ruta);

            if (datosExcel.DataRows.Count > 0)
                ProcesarRegistro(datosExcel);

            if (File.Exists(ruta))
                File.Delete(ruta);

            return true;
        }

        public void ProcesarRegistro(ExcelData datosExcel)
        {
            CargaMasivaNEG negocio = new CargaMasivaNEG();
            this.ConstanciasValidas = new List<ConstanciaCMPOCO>();
            this.ConstanciasError = new List<ConstanciaCMPOCO>();
            this.Totales = new List<TotalesPOCO>();

            ConfigurarControlGrid();

            foreach (var fila in datosExcel.DataRows)
            {
                ConstanciaCMPOCO preconstacia = negocio.CrearEntidadConstanciaCMPOCO(fila, this.CentroTrabajoSIRCEID, this.EmpresaID);

                if (preconstacia.TrabajadorID != string.Empty && preconstacia.CursoId != string.Empty)
                {
                    Constancias constancia = negocio.CrearEntidadConstancia(preconstacia.TrabajadorID, preconstacia.CursoId, this.ListaCentroTrabajoID);
                    if (negocio.CrearConstancia(constancia))
                        this.ConstanciasValidas.Add(preconstacia);
                }
                else
                {
                    this.ConstanciasError.Add(preconstacia);
                }
            }

            this.gridConstanciasValidas.datos = this.ConstanciasValidas;
            this.gridConstanciasError.datos = this.ConstanciasError;
            this.Totales = ObtenerTotales();
            this.gridTotales.datos = this.Totales;
        }

        /// <summary>
        /// Método para obtener totales
        /// </summary>
        private List<TotalesPOCO> ObtenerTotales()
        {
            TotalesPOCO registrosProcesados = new TotalesPOCO();
            TotalesPOCO registrosError = new TotalesPOCO();
            TotalesPOCO registrosOk = new TotalesPOCO();

            List<TotalesPOCO> lista = new List<TotalesPOCO>();

            registrosProcesados.ID = "1";
            registrosProcesados.Tipo = "Cursos Trabajadores que integran el archivo.";
            registrosProcesados.Total = (this.ConstanciasValidas.Count + this.ConstanciasError.Count).ToString();

            lista.Add(registrosProcesados);

            registrosError.ID = "2";
            registrosError.Tipo = "Registros incorrectos.";
            registrosError.Total = this.ConstanciasError.Count.ToString();

            lista.Add(registrosError);

            registrosOk.ID = "3";
            registrosOk.Tipo = "Registros correctos.";
            registrosOk.Total = this.ConstanciasValidas.Count.ToString();

            lista.Add(registrosOk);

            return lista;
        }

        /// <summary>
        /// Método para Generar la estructura y configuración del Grid
        /// </summary> 
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

        /// <summary>
        /// Método para asignar la propiedad del modelo donde se guardará el identificador del renglón seleccionado en el grid
        /// </summary>
        private void AsignarIdRenglonGrid()
        {
            this.gridConstanciasValidas.columnaID = "ConstanciaID";
            //this.gridConstanciasError.columnaID = "ConstanciaID";
            this.gridTotales.columnaID = "ID";
        }

        /// <summary>
        ///  Método para asignar el ícono de "Crear" al grid.
        /// </summary>
        private void AsignarIconosGrid()
        {
            /*//Se declaran las acciones a realizar dentro del grid
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();

            // Se declaran los botones del CRUD
            this.gridConstanciasValidas.iconosGrid = new List<IconosGridPOCO>();

            // Configuramos el ícono "Consultar"
            IconosGridPOCO consultar = new IconosGridPOCO();
            consultar.icono = "search";
            consultar.tooltip = "Consultar";
            consultar.callback = "accionarClick";
            consultar.enabled = false;
            consultar.accion = objAccionNEG.consultarValorAccion(acciones, "Consultar");

            this.gridConstanciasValidas.iconosGrid.Add(consultar);
            
            //validar segundo grid
            this.gridConstanciasError.iconosGrid = new List<IconosGridPOCO>();

            // Configuramos el ícono "Consultar"
            IconosGridPOCO consultarError = new IconosGridPOCO();
            consultarError.icono = "search";
            consultarError.tooltip = "Consultar";
            consultarError.callback = "accionarClick";
            consultarError.enabled = false;
            consultarError.accion = objAccionNEG.consultarValorAccion(acciones, "Consultar");

            this.gridConstanciasError.iconosGrid.Add(consultarError);*/
        }

        /// <summary>
        /// Se indican la propiedades de la clase POCO que va a utilizar el grid.
        /// </summary>
        private void AsignarPropiedadesGrid()
        {
            //totales
            this.gridTotales.columnas = new List<string>();
            this.gridTotales.columnas.Add("Tipo");
            this.gridTotales.columnas.Add("Total");

            this.gridConstanciasValidas.columnas = new List<string>();
            this.gridConstanciasValidas.columnas.Add("NombreTrabajador");
            this.gridConstanciasValidas.columnas.Add("ApellidoPaterno");
            this.gridConstanciasValidas.columnas.Add("ApellidoMaterno");
            this.gridConstanciasValidas.columnas.Add("CURP");
            this.gridConstanciasValidas.columnas.Add("EntidadFederativaID");
            this.gridConstanciasValidas.columnas.Add("MunicipioID");
            this.gridConstanciasValidas.columnas.Add("NombreCurso");
            //validar segundo grid 
            this.gridConstanciasError.columnas = gridConstanciasValidas.columnas;
        }

        /// <summary>
        /// Se configuran los títulos que debe mostrar el Grid
        /// </summary>
        private void AgregarTitulosGrid()
        {
            //totales
            this.gridTotales.encabezados = new List<string>();
            this.gridTotales.encabezados.Add("Dato");
            this.gridTotales.encabezados.Add("Total");

            //Se declaran los titulos de la tabla
            this.gridConstanciasValidas.encabezados = new List<string>();
            this.gridConstanciasValidas.encabezados.Add("Nombre");
            this.gridConstanciasValidas.encabezados.Add("Apellido Paterno");
            this.gridConstanciasValidas.encabezados.Add("Apellido Materno");
            this.gridConstanciasValidas.encabezados.Add("CURP");
            this.gridConstanciasValidas.encabezados.Add("Entidad");
            this.gridConstanciasValidas.encabezados.Add("Municipio");
            this.gridConstanciasValidas.encabezados.Add("Curso");
            //validar segundo grid 
            this.gridConstanciasError.encabezados = this.gridConstanciasValidas.encabezados;
        }

        #endregion

    }


}