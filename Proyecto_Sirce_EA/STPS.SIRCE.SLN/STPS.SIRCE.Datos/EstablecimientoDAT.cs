using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace STPS.SIRCE.Datos
{
    public class EstablecimientoDAT : AccesableContext
    {
        #region Constructor
        public EstablecimientoDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        /// <summary>
        /// Método para consultar los establecimeintos de una lista
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public List<EstablecimientoPOCO> ConsultarEstablecimientos(ListaCentrosTrabajo entidad, int EmpresaSIRCEID)
        {
            List<EstablecimientoPOCO> constancias = new List<EstablecimientoPOCO>();

            constancias = (from lct in contexto.ListaCentrosTrabajo
                           join c in contexto.Constancias on lct.ListaCentroTrabajoID equals c.ListaCentroTrabajoID
                           join t in contexto.Trabajadores on c.TrabajadorID equals t.TrabajadorID
                           join cu in contexto.Cursos on c.CursoID equals cu.CursoID
                           join e in contexto.EmpresaCentrosTrabajoSIRCE on lct.CentroTrabajoSIRCEID equals e.CentroTrabajoSIRCEID
                           join ct in contexto.CentrosTrabajoSIRCE on lct.CentroTrabajoSIRCEID equals ct.CentroTrabajoSIRCEID
                           join em in contexto.EmpresasSIRCE on e.EmpresaSIRCEID equals em.EmpresaSIRCEID
                           where lct.ListaID == entidad.ListaID
                           && lct.CentroTrabajoSIRCEID != entidad.CentroTrabajoSIRCEID
                           && e.EmpresaSIRCEID == EmpresaSIRCEID
                           && c.Eliminado.Equals(false)
                           select new EstablecimientoPOCO
                           {
                               ConstanciaID = c.TrabajadorID.ToString() + "|" + lct.CentroTrabajoSIRCEID.ToString() + "|" + cu.CursoID.ToString(),
                               trabajadorID = c.TrabajadorID,
                               TrabajadorDescripcion = t.Nombre + " " + t.ApellidoPaterno + " " + t.ApellidoMaterno,
                               cursoID = c.CursoID,
                               CursoDescripcion = cu.Nombre,
                               Eliminado = c.Eliminado,
                               CentroTrabajoSIRCEID = lct.CentroTrabajoSIRCEID,
                               CentroTrabajoID = ct.CentroTrabajoID,
                               Nombre = t.Nombre,
                               ApellidoPaterno = t.ApellidoPaterno,
                               ApellidoMaterno = t.ApellidoMaterno,
                               CURP = t.CURP,
                               centroTrabajoNombre = "",
                               empresaID = em.EmpresaID
                           }).ToList();

            return constancias;
        }

        /// <summary>
        /// Metodo para crear un establecimiento
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool CrearEstablecimiento(ListaCentrosTrabajo entidad, int centroTrabajoSIRCEID)
        {
            //Se recuperan todas las claves ListaCentrosTrabajoID del grid.
            List<int> listaCentroTrabajoID = entidad.Constancias.Select(x => x.ListaCentroTrabajoID).Distinct().ToList();

            //Hay valores existentes
            if (listaCentroTrabajoID.Count > 0)
            {
                //Se deben de actualizar todos los registros que contengan la ListaCentrosTrabajoID
                var lista = (from l in contexto.Constancias
                             where listaCentroTrabajoID.Contains(l.ListaCentroTrabajoID)
                             select l).Distinct().ToList();

                foreach (var item in lista)
                {
                    item.Eliminado = true;
                    // Cambiamos el estado de la entidad y salvamos los cambios
                    contexto.Entry(item).State = EntityState.Modified;
                }
            }
            else
            {
                var listConstancia = (from c in contexto.Constancias
                                      join lct in contexto.ListaCentrosTrabajo on c.ListaCentroTrabajoID equals lct.ListaCentroTrabajoID
                                      where lct.ListaID == entidad.ListaID
                                      && lct.CentroTrabajoSIRCEID != centroTrabajoSIRCEID
                                      && c.Eliminado == false
                                      select c).ToList();
                if (listConstancia.Count > 0)
                {
                    //Actualizo todos los valores a true
                    foreach (var item in listConstancia)
                    {
                        item.Eliminado = true;
                        // Cambiamos el estado de la entidad y salvamos los cambios
                        contexto.Entry(item).State = EntityState.Modified;
                    }
                }
            }

            //Actualizo los valores
            foreach (var item in entidad.Constancias)
            {
                var constancia = (from c in contexto.Constancias
                                  where c.ListaCentroTrabajoID == item.ListaCentroTrabajoID && c.TrabajadorID == item.TrabajadorID && c.CursoID == item.CursoID
                                  select c).FirstOrDefault();

                if (constancia != null)
                {
                    constancia.Eliminado = item.Eliminado;

                    // Cambiamos el estado de la entidad y salvamos los cambios
                    contexto.Entry(constancia).State = EntityState.Modified;
                }
            }

            //Agrego los nuevos
            foreach (var item in entidad.Constancias)
            {
                int existeConstancia = (from c in contexto.Constancias
                                        where c.ListaCentroTrabajoID == item.ListaCentroTrabajoID && c.TrabajadorID == item.TrabajadorID && c.CursoID == item.CursoID
                                        select c).Count();
                if (existeConstancia == 0)
                {
                    contexto.Constancias.Add(item);
                }
            }

            return true;
        }
    }
}
