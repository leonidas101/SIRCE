using STPS.Framework;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class TrabajadorNEG : WorkUnit
    {
        private UtileriaNeg utileriasNEG = new UtileriaNeg();

        #region Constructor
        public TrabajadorNEG()
        {
        }

        public TrabajadorNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        /// <summary>
        /// Método para crear un trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <param name="normasTrabajador">Normas del trabajador</param>
        /// <returns></returns>
        public bool Crear(Trabajadores trabajadores)
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                datos.Crear(trabajadores);
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
        /// Método para editar un trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <param name="normasTrabajador">Normas del trabajador</param>
        /// <returns></returns>
        public bool Editar(Trabajadores trabajadores)
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                datos.Editar(trabajadores);
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
        /// Método para eliminar un trabajador
        /// </summary>
        /// <param name="trabajadorID">Clave del trabajador</param>
        /// <returns></returns>
        public bool EliminarTrabajador(Trabajadores trabajadores)
        {
            bool resultado = false;
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                resultado = datos.Eliminar(trabajadores);
                Save();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener un trabajador
        /// </summary>
        /// <param name="trabajadores"></param>
        /// <returns></returns>
        public Trabajadores ConsultarTrabajador(int trabajadorID)
        {
            try
            {
                Trabajadores result = new TrabajadorDAT(contextoSIRCE).ConsultarTrabajador(trabajadorID);
                return result;
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
        }

        /// <summary>
        /// Método para obtener todos los trabajadores.(Nombre-CURP)
        /// </summary>
        /// <param name="centroTrabajadorSIRCEID"></param>
        /// <returns></returns>
        public List<CatalogoCompuestoPOCO> ConsultarTrabajadoresCatalogoPOCO(int EmpresaSIRCEID, int centroTrabajadorSIRCEID)
        {
            List<CatalogoCompuestoPOCO> trabajadores = new List<CatalogoCompuestoPOCO>();
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            try
            {
                trabajadores = datos.ConsultarTrabajadoresCatalogoPOCO(EmpresaSIRCEID, centroTrabajadorSIRCEID);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return trabajadores;
        }

        /// <summary>
        /// Método para obtener todos trabajadores de una empresa, la clave es compuesta.
        /// </summary>
        /// <param name="centroTrabajadorSIRCEID"></param>
        /// <returns></returns>
        public List<CatalogoCompuestoPOCO> ConsultarTrabajadoresCatalogoCompuestoPOCO(int EmpresaSIRCEID, int centroTrabajadorSIRCEID)
        {
            List<CatalogoCompuestoPOCO> trabajadores = new List<CatalogoCompuestoPOCO>();
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            try
            {
                trabajadores = datos.ConsultarTrabajadoresCatalogoCompuestoPOCO(EmpresaSIRCEID, centroTrabajadorSIRCEID);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return trabajadores;
        }

        /// <summary>
        /// Método que obtiene las ocupaciones especificas
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarOcupaciones()
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();

            try
            {
                resultado = datos.ConsultarOcupaciones();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener las normas o estandares del trabajador
        /// </summary>
        /// <param name="trabajadores"></param>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarNormaCompetencia()
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();
            try
            {
                resultado = datos.ConsultarNormaCompetencia();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener los todos trabajadores de un centro de trabajo
        /// </summary>
        /// <param name="empresaID">Clave de la empresa</param>
        /// <returns></returns>
        public List<TrabajadorPOCO> ConsultarTrabajadoresPOCO(int CentroTrabajoSIRCEID)
        {
            List<TrabajadorPOCO> resultado = new List<TrabajadorPOCO>();
            List<CatalogoPOCO> institucionesEducativas = new List<CatalogoPOCO>();
            List<CatalogoPOCO> genero = new List<CatalogoPOCO>();
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                resultado = datos.ConsultarTrabajadoresPOCO(CentroTrabajoSIRCEID);
                if (resultado.Count > 0)
                {
                    institucionesEducativas = utileriasNEG.EnumeradorALista<Enumeradores.InstitucionEducativa>();
                    genero = utileriasNEG.EnumeradorALista<Enumeradores.Genero>();
                    foreach (TrabajadorPOCO item in resultado)
                    {
                        item.InstitucionesEducativasDescripcion = (item.InstitucionesEducativasID.HasValue ? institucionesEducativas.First(x => x.catalogoID == item.InstitucionesEducativasID).catalogoDescripcion : string.Empty);
                        item.GeneroDescripcion = (item.Genero > 0 ? genero.First(x => x.catalogoID == item.Genero).catalogoDescripcion : string.Empty);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            } return resultado;
        }

        /// <summary>
        /// Método para obtener las normas del trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <returns></returns>
        public bool ConsultarNormasTrabajador(Trabajadores trabajadores)
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                datos.ConsultarNormasTrabajador(trabajadores);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return true;
        }

        /// <summary>
        /// Método para obtener las instituciones educativas
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarInstitucionesEducativa()
        {
            List<CatalogoPOCO> datos = new List<CatalogoPOCO>();
            try
            {
                datos = utileriasNEG.EnumeradorALista<Enumeradores.InstitucionEducativa>();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return datos;
        }

        /// <summary>
        /// Método que obtiene los documentos probatorios del trabajador
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarDocumentosProbatorios()
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();

            try
            {
                resultado = datos.ConsultarDocumentosProbatorios();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener las entidades federativas
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarEntidades()
        {
            List<CatalogoPOCO> datos = new List<CatalogoPOCO>();

            try
            {
                datos = utileriasNEG.ConsultarEntidades();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return datos;
        }

        /// <summary>
        /// Método para obtener las municipios
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarMunicipios(int entidadID)
        {
            List<CatalogoPOCO> datos = new List<CatalogoPOCO>();

            try
            {
                datos = utileriasNEG.ConsultarMunicipios(entidadID);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return datos;
        }

        /// <summary>
        /// Método que obtiene el nivel máximo de estudios terminados
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarEscolaridades()
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();

            try
            {
                resultado = datos.ConsultarEscolaridades();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método que obtiene el genero
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarGeneros()
        {
            List<CatalogoPOCO> datos = new List<CatalogoPOCO>();
            try
            {
                datos = utileriasNEG.EnumeradorALista<Enumeradores.Genero>();
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return datos;
        }

        /// <summary>
        /// Método para validar si un RFC ya se encuentra registrado
        /// </summary>
        /// <param name="trabajadores"></param>
        /// <returns></returns>
        public bool ValidaCURP(Trabajadores trabajadores)
        {
            Boolean resultado = true;
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);

            try
            {
                resultado = datos.ValidaCURP(trabajadores);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para validar si un RFC es correcto
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public bool ValidarFormatoCURP(string curp)
        {
            var esValido = true;

            if (!Regex.IsMatch(curp, "^[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$"))
                esValido = false;

            return esValido;
        }

        public bool PostTrabajador(Trabajadores trabajador)
        {
            TrabajadorDAT objTrabajadorDAT = new TrabajadorDAT(contextoSIRCE);
            bool success = objTrabajadorDAT.PostTrabajador(trabajador);
            Save();
            Dispose();
            return success;
        }

        /// <summary>
        /// Método para consultar una ocupación
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarOcupacion(Ocupaciones entidad)
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            var resultado = true;

            try
            {
                resultado = datos.ConsultarOcupacion(entidad);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return resultado;
        }

        /// <summary>
        /// Método para consultar una institución educativa
        /// </summary>
        /// <returns></returns>
        public bool ConsultarInstitucionEducativa(CatalogoPOCO entidad)
        {
            var resultado = false;
            
            try
            {
                var instituciones = ConsultarInstitucionesEducativa();
                
                    var catalogo = instituciones.Where(x => x.catalogoID == entidad.catalogoID).FirstOrDefault();
                    if (catalogo != null)
                    {
                        entidad.catalogoID = catalogo.catalogoID;
                        entidad.catalogoDescripcion = catalogo.catalogoDescripcion;
                        resultado = true;
                    }
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }
            return resultado;
        }

        /// <summary>
        /// Método para consultar una escolaridad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool ConsultarEscolaridad(Escolaridades entidad)
        {
            TrabajadorDAT datos = new TrabajadorDAT(contextoSIRCE);
            var resultado = true;

            try
            {
                resultado = datos.ConsultarEscolaridad(entidad);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return resultado;
        }

    }
}
