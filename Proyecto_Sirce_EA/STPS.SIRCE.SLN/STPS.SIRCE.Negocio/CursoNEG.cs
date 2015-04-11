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
    public class CursoNEG : WorkUnit
    {

        #region Constructor
        public CursoNEG()
        {
        }

        public CursoNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos


        /// <summary>
        /// Método para crear un curso
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Crear(Cursos entidad)
        {
            CursoDAT datos = new CursoDAT(contextoSIRCE);

            try
            {
                datos.Crear(entidad);
                // 
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return true;

        }

        /// <summary>
        /// Método para editar un curso
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Editar(Cursos entidad)
        {
            CursoDAT datos = new CursoDAT(contextoSIRCE);

            try
            {
                datos.Editar(entidad);
                // 
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return true;

        }

        /// <summary>
        /// Método para eliminar un curso
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool Eliminar(int id)
        {

            CursoDAT datos = new CursoDAT(contextoSIRCE);

            try
            {
                datos.Eliminar(id);
                // Guardamos el cambio
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return true;
        }

        /// <summary>
        /// Método para obtener los cursos de una empresa
        /// </summary>
        /// <param name="entity">Entidad curso con la clave de la empresa</param>
        /// <returns>Lista de CursosPOCO</returns>
        public List<CursoPOCO> ConsultarCursos(Cursos entity)
        {
            List<CursoPOCO> lista = new List<CursoPOCO>();
            try
            {
                CursoDAT datos = new CursoDAT(contextoSIRCE);

                lista = datos.ConsultarCursos(entity);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Método para obtener los cursos de una empresa (ID_DESC)
        /// </summary>
        /// <param name="entity">Entidad curso con la clave de la empresa</param>
        /// <returns>Lista de CursosPOCO</returns>
        public List<CatalogoPOCO> ConsultarCursos(int empresaSirceId)
        {
            List<CatalogoPOCO> lista = new List<CatalogoPOCO>();
            try
            {
                CursoDAT datos = new CursoDAT(contextoSIRCE);

                lista = datos.ConsultarCursos(empresaSirceId);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Método para consultar los datos de una entidad
        /// </summary>
        /// <param name="entitdad"></param>
        /// <returns></returns>
        public bool Consultar(Cursos entitdad)
        {
            CursoDAT datos = new CursoDAT(contextoSIRCE);

            try
            {
                datos.Consultar(entitdad);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
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

            try
            {
                CursoDAT datos = new CursoDAT(contextoSIRCE);

                lista = datos.ConsultarAreasTematicas();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Método para consultar los Objetivos de capacitacion
        /// </summary>
        /// <returns>Lista de ObjetivosCapacitacion</returns>
        public List<CatalogoPOCO> ConsultarObjetivosCapacitacion()
        {
            List<CatalogoPOCO> lista = new List<CatalogoPOCO>();

            try
            {
                CursoDAT datos = new CursoDAT(contextoSIRCE);

                lista = datos.ConsultarObjetivosCapacitacion();

            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return lista;
        }

        /// <summary>
        /// Método para validar un área temática
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarAreaTematica(AreasTematicas entidad)
        {
            CursoDAT datos = new CursoDAT(contextoSIRCE);
            var resultado = true;

            try
            {
                datos.ConsultAreaTematica(entidad);
                if (entidad.Descripcion.Equals(null))
                    resultado = false;
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return resultado;
        }

        /// <summary>
        /// Método para consultar un objetivo de capacitación
        /// </summary>
        /// <returns></returns>
        public bool ConsultarObjetivoCapacitacion(ObjetivosCapacitacion entidad)
        {
            CursoDAT datos = new CursoDAT(contextoSIRCE);
            var resultado = true;

            try
            {
                resultado = datos.ConsultarObjetivoCapacitacion(entidad);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return resultado;
        }

        /// <summary>
        /// Método para consultar un tipo agente
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarTipoAgente(CatalogoPOCO entidad)
        {
            List<CatalogoPOCO> tiposAgente = new UtileriaNeg().EnumeradorALista<Enumeradores.TipoAgenteCapacitador>();
            entidad = tiposAgente.Where(x => x.catalogoID == entidad.catalogoID).FirstOrDefault();

            if (entidad != null)
                return true;
            return false;
        }

        /// <summary>
        /// Método para consultar una modalidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarModalidad(CatalogoPOCO entidad)
        {
            List<CatalogoPOCO> modalidades = new UtileriaNeg().EnumeradorALista<Enumeradores.ModalidadCapacitacion>(); ;
            entidad = modalidades.Where(x => x.catalogoID == entidad.catalogoID).FirstOrDefault();

            if (entidad != null)
                return true;
            return false;
        }
    }
}
