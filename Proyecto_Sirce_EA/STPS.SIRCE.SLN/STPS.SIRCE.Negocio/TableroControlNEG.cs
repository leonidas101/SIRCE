using STPS.Framework;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class TableroControlNEG : WorkUnit
    {

        #region Constructor
        public TableroControlNEG()
        {
        }

        public TableroControlNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos
        
        public List<ListaPOCO> consultarListas(int empresaSIRCEID, List<CatalogoPOCO> acciones)
        {
            TableroControlDAT objTableroControlDAT = new TableroControlDAT(contextoSIRCE);
            List<ListaPOCO> resultSet = objTableroControlDAT.consultarListas(empresaSIRCEID);
            AccionNEG objAccionNEG = new AccionNEG();
            //Se aplican regla de negocio para las acciones del GRID!
            foreach (ListaPOCO item in resultSet)
            {
                item.acciones = new List<IconosGridPOCO>();
                // Configuramos el ícono "Crear"
                IconosGridPOCO crear = new IconosGridPOCO();
                crear.icono = "plus";
                crear.tooltip = "Agregar una Lista";
                crear.callback = "accionarClick";
                crear.enabled = true;
                crear.accion = objAccionNEG.consultarValorAccion(acciones, "Crear");
                item.acciones.Add(crear);
                // Configuramos el ícono "Consultar"
                IconosGridPOCO consultar = new IconosGridPOCO();
                consultar.icono = "search";
                consultar.tooltip = "Consultar una Lista";
                consultar.callback = "accionarClick";
                consultar.enabled = true;
                consultar.accion = objAccionNEG.consultarValorAccion(acciones, "Consultar");
                item.acciones.Add(consultar);
                //Regla para edicion si no han pasado 7 días despues de la creación de la lista.
                #region Regla 7 Días
                if (!((DateTime.Now - Utilidades.ConvertirFecha(item.fechaCreacion)).TotalDays > 7))
                {

                    // Configuramos el ícono "Editar"
                    IconosGridPOCO editar = new IconosGridPOCO();
                    editar.icono = "pencil";
                    editar.tooltip = "Editar una Lista";
                    editar.callback = "accionarClick";
                    editar.enabled = true;
                    editar.accion = objAccionNEG.consultarValorAccion(acciones, "Editar");
                    item.acciones.Add(editar);

                    // Configuramos el ícono "Eliminar"
                    IconosGridPOCO eliminar = new IconosGridPOCO();
                    eliminar.icono = "remove";
                    eliminar.tooltip = "Eliminar una Lista";
                    eliminar.callback = "accionarClick";
                    eliminar.enabled = true;
                    eliminar.accion = objAccionNEG.consultarValorAccion(acciones, "Eliminar");
                    item.acciones.Add(eliminar);
                }
                #endregion
                //Regla para generación de constancias o envio si solamente si la lista no ah sido enviada.
                #region Regla para lista enviada
                if (item.estatusID != (int)Enumeradores.EstatusLista.Enviada)
                {
                    // Configuramos el ícono "Generar Constancias"
                    IconosGridPOCO generarConstancias = new IconosGridPOCO();
                    generarConstancias.icono = "graduation-cap";
                    generarConstancias.tooltip = "Generar Constancias";
                    generarConstancias.callback = "accionarClick";
                    generarConstancias.enabled = true;
                    generarConstancias.accion = objAccionNEG.consultarValorAccion(acciones, "Generar Constancias");
                    item.acciones.Add(generarConstancias);

                    // Configuramos el ícono "Incorporar Establecimientos"
                    IconosGridPOCO incorporarEstablecimientos = new IconosGridPOCO();
                    incorporarEstablecimientos.icono = "building";
                    incorporarEstablecimientos.tooltip = "Incorporar Establecimientos";
                    incorporarEstablecimientos.callback = "accionarClick";
                    incorporarEstablecimientos.enabled = true;
                    incorporarEstablecimientos.accion = objAccionNEG.consultarValorAccion(acciones, "Incorporar Establecimientos");
                    item.acciones.Add(incorporarEstablecimientos);
                }
                #endregion
                if ((item.numeroConstancias / item.numeroConstanciasTotales) == 1)
                {
                    // Configuramos el ícono "Enviar"
                    IconosGridPOCO enviar = new IconosGridPOCO();
                    enviar.icono = "send";
                    enviar.tooltip = "Enviar Lista";
                    enviar.callback = "accionarClick";
                    enviar.enabled = true;
                    enviar.accion = objAccionNEG.consultarValorAccion(acciones, "Enviar");
                    item.acciones.Add(enviar);
                }
                // Configuramos el ícono "Imprimir DC3"
                IconosGridPOCO imprimir = new IconosGridPOCO();
                imprimir.icono = "print";
                imprimir.tooltip = "Imprimir Constancias DC3";
                imprimir.callback = "accionarClick";
                imprimir.enabled = true;
                imprimir.accion = objAccionNEG.consultarValorAccion(acciones, "Imprimir");
                item.acciones.Add(imprimir);
            }
            Dispose();
            return resultSet;
        }

        public bool obtenerConsecutivo(ConsecutivoPOCO entidad)
        {
            TableroControlDAT objTableroControlDAT = new TableroControlDAT(contextoSIRCE);
            objTableroControlDAT.obtenerConsecutivo(entidad);
            Dispose();
            return true;
        }

        public bool postLista(Listas lista)
        {
            TableroControlDAT objTableroControlDAT = new TableroControlDAT(contextoSIRCE);
            objTableroControlDAT.postLista(lista);
            Save();
            Dispose();
            return true;
        }

        public bool postAcuse(Acuses acuse)
        {
            TableroControlDAT objTableroControlDAT = new TableroControlDAT(contextoSIRCE);
            objTableroControlDAT.postAcuse(acuse);
            Save();
            Dispose();
            return true;
        }

        public Listas consultarLista(int listaID)
        {
            TableroControlDAT objTableroControlDAT = new TableroControlDAT(contextoSIRCE);
            Listas lista = objTableroControlDAT.ConsultarLita(listaID);
            return lista;
        }

        public bool postListaCentroTrabajo(ListaCentrosTrabajo listaCentroTrabajo)
        {
            ListaCentrosTrabajoDAT objTableroControlDAT = new ListaCentrosTrabajoDAT(contextoSIRCE);
            objTableroControlDAT.postListaCentroTrabajo(listaCentroTrabajo);
            Save();
            Dispose();
            return true;
        }
    }
}
