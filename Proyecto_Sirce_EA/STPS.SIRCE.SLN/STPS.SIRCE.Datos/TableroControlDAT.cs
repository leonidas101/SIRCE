using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public class TableroControlDAT : AccesableContext
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="contexto"></param>
        public TableroControlDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        public List<ListaPOCO> consultarListas(int empresasSIRCEID)
        {
            DataTable unidadesResponsables = new UtileriaDAT().ConsultarUnidadesResponsables();
            List<CatalogoPOCO> entidades = new UtileriaDAT().ConsultarEntidades(); 
            var listaPOCO = (from l in contexto.Listas
                             join a in contexto.Acuses on l.ListaID equals a.ListaID
                             join ct in contexto.CentrosTrabajoSIRCE on l.CentroTrabajoSIRCEID equals ct.CentroTrabajoSIRCEID
                             join ects in contexto.EmpresaCentrosTrabajoSIRCE on ct.CentroTrabajoSIRCEID equals ects.CentroTrabajoSIRCEID
                             join e in contexto.EmpresasSIRCE on ects.EmpresaSIRCEID equals e.EmpresaSIRCEID
                             join lc in contexto.ListaCentrosTrabajo on l.ListaID equals lc.ListaID into establecimientosTotales
                             where l.Eliminado == false 
                             && e.EmpresaSIRCEID == empresasSIRCEID
                             select new ListaPOCO
                             {
                                 listaID = l.ListaID,
                                 folioEmpresa = l.FolioEmpresa,
                                 nombreLista = l.Nombre,
                                 fechaPresentacion = (DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", l.FechaPresentacion))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", l.FechaPresentacion))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", l.FechaPresentacion)), 4)
                                                               ).Replace(" ", "0"),
                                 numeroEstablecimientos = establecimientosTotales.Count(),
                                 numeroConstancias = (from c in contexto.Constancias
                                                      where establecimientosTotales.Select(x => x.ListaCentroTrabajoID).Contains(c.ListaCentroTrabajoID)
                                                      && c.Eliminado == false
                                                      select c).Count(),
                                 numeroCursos = (from c in contexto.Constancias
                                                 where establecimientosTotales.Select(x => x.ListaCentroTrabajoID).Contains(c.ListaCentroTrabajoID)
                                                 && c.Eliminado == false
                                                 select c).Where(c => c.Eliminado != true).Select(x => x.CursoID).Distinct().Count(),
                                 numeroTrabajadores = (from c in contexto.Constancias
                                                       join t in contexto.Trabajadores on c.TrabajadorID equals t.TrabajadorID
                                                       where establecimientosTotales.Select(x => x.ListaCentroTrabajoID).Contains(c.ListaCentroTrabajoID)
                                                       && c.Eliminado == false
                                                       select t).Where(c => c.Eliminado != true).Select(x => x.CURP).Distinct().Count(),
                                 usuarioPresento = l.UsuarioCreacion,
                                 unidadResponsableID = l.UnidadResponsableID,
                                 folioDC4 = a.FolioDC4,
                                 estatusID = l.Estatus,
                                 origen = l.Origen,
                                 numeroConstanciasTotales = l.NumeroConstancias,
                                 fechaCreacion = (DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", l.FechaCreacion))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", l.FechaCreacion))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", l.FechaCreacion)), 4)
                                                               ).Replace(" ", "0"),
                             }).Distinct().ToList();
            foreach (ListaPOCO item in listaPOCO)
            {
                item.estatus = Enumeradores.GetDescription((Enumeradores.EstatusLista)(int.Parse(item.estatusID.ToString())));
                item.porcentajeAvance = ((double.Parse(item.numeroConstancias.ToString()) 
                                        / double.Parse(item.numeroConstanciasTotales.ToString())) 
                                        * 100).ToString() 
                                        + '%';
                if (item.origen == (byte)Enumeradores.OrigenSIRCE.Internet)
                {

                }
                else if (item.origen == (byte)Enumeradores.OrigenSIRCE.Ventanilla)
                {
                    item.entidadFederativa = (from ur in unidadesResponsables.AsEnumerable()
                                              where ur.Field<int>("unidadResponsableID") == item.unidadResponsableID
                                              select ur.Field<string>("entidadDesc")).FirstOrDefault();
                }

            }
            return listaPOCO;
        }

        public bool obtenerConsecutivo(ConsecutivoPOCO entidad)
        {
            var resultado = contexto.spGetConsecutivos((int)entidad.EmpresaSIRCEID, (int)entidad.ConsecutivoURID);

            if (resultado != null)
            {
                foreach (var item in resultado)
                {
                    entidad.ConsecutivoEmpresa = item.ConsecutivoLista.ToString();
                    entidad.ConsecutivoDC4 = item.ConsecutivoDC4;
                }
            }

            return true;
        }

        public bool postLista(Listas lista)
        {
            if(lista.ListaID==0)
            contexto.Listas.Add(lista);
            else
                contexto.Entry(lista).State = EntityState.Modified;
            return true;
        }

        public bool postAcuse(Acuses acuse)
        {
            contexto.Acuses.Add(acuse);
            return true;
        }

        public Listas ConsultarLita(int listaID)
        {
            var resultado =  (from l in contexto.Listas
                                where l.ListaID == listaID
                                && l.Eliminado == false
                              select l).FirstOrDefault();
            return resultado;
        }

        public bool postListaCentroTrabajo(ListaCentrosTrabajo listaCentroTrabajo)
        {
            contexto.ListaCentrosTrabajo.Add(listaCentroTrabajo);
            return true;
        }
    }
}
