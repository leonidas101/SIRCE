using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;

namespace STPS.SIRCE.Datos
{
    public class CursoDAT : AccesableContext
    {


        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="contexto"></param>
        public CursoDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion


        /// <summary>
        /// Método para obtener los cursos de una empresa
        /// </summary>
        /// <param name="entidad">Entidad curso con la clave de la empresa</param>
        /// <returns>Lista de CursosPOCO</returns>
        public List<CursoPOCO> ConsultarCursos(Cursos entidad)
        {

            var lista = from c in contexto.Cursos
                        join a in contexto.AreasTematicas on c.AreaTematicaID equals a.AreaTematicaID
                        where c.EmpresaSIRCEID == entidad.EmpresaSIRCEID
                        && c.Eliminado == false
                        select new CursoPOCO
                        {
                            CursoId = c.CursoID,
                            Nombre = c.Nombre,
                            Duracion = c.Duracion,
                            AreaTematicaId = c.AreaTematicaID,
                            //FechaTermino = c.FechaTermino.ToString("dd/"),
                            AreaTematica = a.Descripcion,
                            FechaTermino = (DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", c.FechaTermino))), 2)
                                            + "/"
                                            + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", c.FechaTermino))), 2)
                                            + "/"
                                            + DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", c.FechaTermino)), 4)
                                           ).Replace(" ", "0")
                        };

            return lista.ToList();
        }

        /// <summary>
        /// Método para obtener todos los cursos (ID-Descripcion)
        /// </summary>
        /// <param name="empresaID">Clave de la empresa</param>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarCursos(int empresaSIRCEID)
        {
            var cursosPoco = (from t in ConsultarCursos(new Cursos { EmpresaSIRCEID = empresaSIRCEID })
                              select new CatalogoPOCO
                              {
                                  catalogoID = t.CursoId,
                                  catalogoDescripcion = t.Nombre
                              }).ToList();

            return cursosPoco;
        }

        /// <summary>
        /// Método para consultar los datos de una entidad
        /// </summary>
        /// <param name="entitdad"></param>
        /// <returns></returns>
        public bool Consultar(Cursos entitdad)
        {
            var resultado = (from c in contexto.Cursos
                             where c.CursoID == entitdad.CursoID
                             && c.Eliminado == false
                             select c
                            ).FirstOrDefault();
            if (resultado != null)
            {
                entitdad.CursoID = resultado.CursoID;
                entitdad.EmpresaSIRCEID = resultado.EmpresaSIRCEID;
                entitdad.Nombre = resultado.Nombre;
                entitdad.Duracion = resultado.Duracion;
                entitdad.AreaTematicaID = resultado.AreaTematicaID;
                entitdad.FechaInicio = resultado.FechaInicio;
                entitdad.FechaTermino = resultado.FechaTermino;
                entitdad.TipoAgenteCapacitadorID = resultado.TipoAgenteCapacitadorID;
                entitdad.RegistroAgenteExterno = resultado.RegistroAgenteExterno;
                entitdad.ModalidadCapacitacionID = resultado.ModalidadCapacitacionID;
                entitdad.ObjetivoCapacitacionID = resultado.ObjetivoCapacitacionID;
                entitdad.Eliminado = resultado.Eliminado;
            }

            return true;
        }


        /// <summary>
        /// Método para consultar las Areas Temáticas
        /// </summary>
        /// <returns>Lista de AreasTematicas</returns>
        public List<CatalogoPOCO> ConsultarAreasTematicas()
        {
            List<CatalogoPOCO> lista = new List<CatalogoPOCO>();

            lista = (from l in contexto.AreasTematicas
                     select new CatalogoPOCO
                     {
                         catalogoID = l.AreaTematicaID,
                         catalogoDescripcion = l.Clave + "-" + l.Descripcion
                     }).ToList();

            return lista;

        }

        /// <summary>
        /// Método para consultar los Objetivos de capacitacion
        /// </summary>
        /// <returns>Lista de ObjetivosCapacitacion</returns>
        public List<CatalogoPOCO> ConsultarObjetivosCapacitacion()
        {
            List<CatalogoPOCO> lista = new List<CatalogoPOCO>();

            lista = (from l in contexto.ObjetivosCapacitacion
                     select new CatalogoPOCO
                     {
                         catalogoID = l.ObjetivoCapacitacionID,
                         catalogoDescripcion = l.Descripcion
                     }).ToList();

            return lista;
        }


        /// <summary>
        /// Método para editar un curso
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Editar(Cursos entidad)
        {

            contexto.Entry(entidad).State = EntityState.Modified;

            return true;
        }


        /// <summary>
        /// Método para crear un curso nuevo
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Crear(Cursos entidad)
        {
            contexto.Cursos.Add(entidad);

            return true;
        }


        /// <summary>
        /// Método para eliminar un curso
        /// </summary>
        /// <param name="id">Clave de la entidad a borrar</param>
        /// <returns></returns>
        public bool Eliminar(int id)
        {
            // Buscamos el registro que vamos a eliminar
            var curso = contexto.Cursos.Find(id);
            // Catualizamos la propiedad en verdadero
            curso.Eliminado = true;
            // Cambiamos el estado de la entidad y salvamos los cambios
            contexto.Entry(curso).State = EntityState.Modified;

            return true;
        }

        /// <summary>
        /// Método para consultar un área temática
        /// </summary>
        /// <param name="entidad"></param>
        public bool ConsultAreaTematica(AreasTematicas entidad)
        {
            if (contexto.AreasTematicas.Any())
            {
                var resultado = contexto.AreasTematicas.Where(x => x.AreaTematicaID == entidad.AreaTematicaID).FirstOrDefault();
                if (resultado != null)
                {
                    entidad.AreaTematicaID = resultado.AreaTematicaID;
                    entidad.Descripcion = resultado.Descripcion;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Método para consultar un objetivo capaciotación
        /// </summary>
        /// <returns></returns>
        public bool ConsultarObjetivoCapacitacion(ObjetivosCapacitacion entidad)
        {
            if (contexto.ObjetivosCapacitacion.Any())
            {
                var resultado = contexto.ObjetivosCapacitacion.Where(x => x.ObjetivoCapacitacionID == entidad.ObjetivoCapacitacionID).FirstOrDefault();
                if (resultado != null)
                {
                    entidad.ObjetivoCapacitacionID = resultado.ObjetivoCapacitacionID;
                    entidad.Descripcion = resultado.Descripcion;

                    return true;
                }
            }

            return false;
        }
    }
}
