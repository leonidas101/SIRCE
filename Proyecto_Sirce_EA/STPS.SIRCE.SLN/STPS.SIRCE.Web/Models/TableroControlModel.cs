using STPS.Framework;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Entidades.POCOS;
using STPS.SIRCE.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STPS.SIRCE.Web.Models
{
    public class TableroControlModel
    {
        public GridPOCO<ListaPOCO> gridListas { get; set; }
        public int empresaSIRCEID { get; set; }
        public List<CatalogoPOCO> acciones { get; set; }
        public int accion { get; set; }
        public int listaID { get; set; }
        public CentroTrabajoPOCO centroTrabajoDNE { get; set; }

        //Datos de la Lista
        public string nombreLista { get; set; }
        public string fechaPresentacion { get; set; }
        public byte? numeroHombres { get; set; }
        public byte? numeroMujeres { get; set; }
        public int numeroConstancias { get; set; }
        public string nombreRepresentante { get; set; }
        public int unidadResponsableID { get; set; }
        public string usuarioCreacion { get; set; }
        public int centroTrabajoSIRCEID { get; set; }

        public void consultarListas() 
        {
            this.gridListas = new GridPOCO<ListaPOCO>();
            AccionNEG objAccionNEG = new AccionNEG();
            this.acciones = objAccionNEG.consultarAcciones();
            this.gridListas.datos = new TableroControlNEG().consultarListas(this.empresaSIRCEID, this.acciones);
            //Se declaran los titulos de la tabla
            this.gridListas.encabezados = new List<string>();
            this.gridListas.encabezados.Add("Folio Empresa");
            this.gridListas.encabezados.Add("Nombre");
            this.gridListas.encabezados.Add("Trabajadores Asignados");
            this.gridListas.encabezados.Add("Cursos Asignados");
            this.gridListas.encabezados.Add("Número de Constancias");
            this.gridListas.encabezados.Add("Establecimientos Incorporados");
            this.gridListas.encabezados.Add("Folio DC-4");
            this.gridListas.encabezados.Add("Fecha de Presentación");
            this.gridListas.encabezados.Add("Entidad Federativa");
            this.gridListas.encabezados.Add("Avance");
            this.gridListas.encabezados.Add("Estatus");
            //se declaran los valores de la tabla
            this.gridListas.columnas = new List<string>();
            this.gridListas.columnas.Add("folioEmpresa");
            this.gridListas.columnas.Add("nombreLista");
            this.gridListas.columnas.Add("numeroTrabajadores");
            this.gridListas.columnas.Add("numeroCursos");
            this.gridListas.columnas.Add("numeroConstancias");
            this.gridListas.columnas.Add("numeroEstablecimientos");
            this.gridListas.columnas.Add("folioDC4");
            this.gridListas.columnas.Add("fechaPresentacion");
            this.gridListas.columnas.Add("entidadFederativa");
            this.gridListas.columnas.Add("porcentajeAvance");
            this.gridListas.columnas.Add("estatus");

            // Se declaran los botones del CRUD
            this.gridListas.iconosGrid = new List<IconosGridPOCO>();

            // Configuramos el ícono "Crear"
            IconosGridPOCO agregar = new IconosGridPOCO();
            agregar.icono = "plus";
            agregar.tooltip = "Agregar una Lista";
            agregar.callback = "accionarClick";
            agregar.enabled = true;
            agregar.accion = objAccionNEG.consultarValorAccion(acciones, "Crear");
            this.gridListas.iconosGrid.Add(agregar);

            //se declara cual tendra el id del renglon seleccionado
            this.gridListas.columnaID = "listaID";
        }

        public bool postLista()
        {
            TableroControlNEG objTableroControlNeg = new TableroControlNEG();
            ConsecutivoPOCO consecutivos = new ConsecutivoPOCO();
             bool success  = true;
            Listas lista = new Listas();
            if (this.accion != (from a in acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
                lista = objTableroControlNeg.consultarLista(this.listaID); 
            if (this.accion != (from a in acciones where a.catalogoDescripcion.Equals("Eliminar") select a.catalogoID).FirstOrDefault())
            {
                lista.Nombre = this.nombreLista;
                lista.NombreRepresentante = this.nombreRepresentante;
                lista.NumeroConstancias = this.numeroConstancias;
                lista.Hombres = byte.Parse(this.numeroHombres.ToString());
                lista.Mujeres = byte.Parse(this.numeroMujeres.ToString());
                lista.FechaPresentacion = Utilidades.ConvertirFecha(this.fechaPresentacion);
            }
            if (this.accion == (from a in acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
            {
                lista.FechaCreacion = DateTime.Now;
                lista.Estatus = (byte)Enumeradores.EstatusLista.Enproceso;
                lista.Eliminado = false;
                consecutivos.EmpresaSIRCEID = this.empresaSIRCEID;
                consecutivos.ConsecutivoURID = this.unidadResponsableID;
                lista.CentroTrabajoSIRCEID = this.centroTrabajoSIRCEID;
                success =  new TableroControlNEG().obtenerConsecutivo(consecutivos);
                lista.FolioEmpresa = int.Parse(consecutivos.ConsecutivoEmpresa);
                lista.Origen = (int)Enumeradores.OrigenSIRCE.Ventanilla;
                lista.UnidadResponsableID = this.unidadResponsableID;
                lista.UsuarioCreacion = this.usuarioCreacion;
            }
            if (this.accion == (from a in acciones where a.catalogoDescripcion.Equals("Eliminar") select a.catalogoID).FirstOrDefault())
            {
                lista.Eliminado = true;
            }
            success = objTableroControlNeg.postLista(lista);
            if (success && this.accion == (from a in acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
            {
                string direccion = this.centroTrabajoDNE.colonia + " " + this.centroTrabajoDNE.calleNumero + " " + this.centroTrabajoDNE.codigoPostal +
                " " + this.centroTrabajoDNE.entidadDescripcion + " " +this.centroTrabajoDNE.municipioDescripcion;
                Acuses acuse = new Acuses();
                acuse.ListaID = lista.ListaID;
                acuse.FolioDC4 = consecutivos.ConsecutivoDC4;
                acuse.RFC = this.centroTrabajoDNE.rfc;
                acuse.RazonSocial = this.centroTrabajoDNE.centroTrabajoNombre;
                acuse.Direccion = direccion;
                success = new TableroControlNEG().postAcuse(acuse);

                ListaCentrosTrabajo listaCentroTrabajo = new ListaCentrosTrabajo();
                listaCentroTrabajo.ListaID = lista.ListaID;
                listaCentroTrabajo.CentroTrabajoSIRCEID = lista.CentroTrabajoSIRCEID;
                success = new TableroControlNEG().postListaCentroTrabajo(listaCentroTrabajo);
            }
            return success;
        }

        public bool ConsultarLista()
        {
            TableroControlNEG objTableroControlNeg = new TableroControlNEG();
            Listas lista = objTableroControlNeg.consultarLista(this.listaID);
            this.nombreLista = lista.Nombre;
            this.numeroConstancias = lista.NumeroConstancias;
            this.numeroHombres = lista.Hombres;
            this.numeroMujeres = lista.Mujeres;
            this.nombreRepresentante = lista.NombreRepresentante;
            this.fechaPresentacion = Utilidades.ConvertirTexto(lista.FechaPresentacion);
            return true;
        }
    }
}