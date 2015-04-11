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
    public class TrabajadorDAT : AccesableContext
    {
        #region Constructor
        public TrabajadorDAT(SIRCEEntities contexto)
            : base(contexto)
        {
        }
        #endregion

        /// <summary>
        /// Método para dar de alta un trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <returns></returns>
        public bool Crear(Trabajadores trabajadores)
        {
            //Guardamos el trabajador
            contexto.Trabajadores.Add(trabajadores);
            return true;
        }

        /// <summary>
        /// Método para editar un trabajador
        /// </summary>
        /// <param name="entidad">Entidad del trabajador</param>
        /// <returns></returns>
        public bool Editar(Trabajadores entidad)
        {
            // Buscamos el registro que vamos a actualizar
            var trabajador = contexto.Trabajadores.FirstOrDefault(x => x.TrabajadorID == entidad.TrabajadorID);

            if (trabajador != null)
            {
                // Actualizamos los atributos del trabajador
                trabajador.CURP = entidad.CURP;
                trabajador.Nombre = entidad.Nombre;
                trabajador.ApellidoPaterno = entidad.ApellidoPaterno;
                trabajador.ApellidoMaterno = entidad.ApellidoMaterno;
                trabajador.OcupacionID = entidad.OcupacionID;
                trabajador.EntidadFederativaID = entidad.EntidadFederativaID;
                trabajador.MunicipioID = entidad.MunicipioID;

                trabajador.CentroTrabajoSIRCEID = entidad.CentroTrabajoSIRCEID;

                trabajador.ComprobanteEstudioID = entidad.ComprobanteEstudioID;
                trabajador.InstitucionesEducativasID = entidad.InstitucionesEducativasID;
                trabajador.DocumentosProbatorios = entidad.DocumentosProbatorios;
                trabajador.EscolaridadID = entidad.EscolaridadID;
                trabajador.FechaNacimiento = entidad.FechaNacimiento;

                EditarNormasTrabajador(trabajador.NormaTrabajador, entidad);

                // Cambiamos el estado de la entidad y salvamos los cambios
                contexto.Entry(trabajador).State = EntityState.Modified;
            }

            return true;
        }

        /// <summary>
        /// Método para eliminar un trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <returns></returns>
        public bool Eliminar(Trabajadores trabajadores)
        {
            bool resultado = false;

            // Buscamos el registro que vamos a actualizar
            var trabajador = contexto.Trabajadores.FirstOrDefault(x => x.TrabajadorID == trabajadores.TrabajadorID);

            if (trabajador != null)
            {
                // Actualizamos los atributos del trabajador
                trabajador.Eliminado = true;
                // Cambiamos el estado de la entidad y salvamos los cambios
                contexto.Entry(trabajador).State = EntityState.Modified;
            }
            return resultado;
        }

        /// <summary>
        /// Método para obtener un trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <returns></returns>
        public Trabajadores ConsultarTrabajador(int trabajadorID)
        {
            var trabajador = (from t in contexto.Trabajadores
                              where t.TrabajadorID == trabajadorID
                              && t.Eliminado == false
                              select t
                            ).FirstOrDefault();

            trabajador.NormaTrabajador = trabajador.NormaTrabajador.Where(x => x.Eliminado.Equals(false)).ToList();
            return trabajador;
        }

        /// <summary>
        /// Método para obtener los todos trabajadores de una empresa
        /// </summary>
        /// <param name="empresaID">Clave de la centro del trabajo</param>
        /// <returns></returns>
        public List<TrabajadorPOCO> ConsultarTrabajadoresPOCO(int CentroTrabajoSIRCEID)
        {
            List<CatalogoPOCO> escolaridades = new List<CatalogoPOCO>();
            List<CatalogoPOCO> documentosProbatorios = new List<CatalogoPOCO>();

            escolaridades = ConsultarEscolaridades();
            documentosProbatorios = ConsultarDocumentosProbatorios();

            var trabajadorPoco = (from t in contexto.Trabajadores
                                  join o in contexto.Ocupaciones on t.OcupacionID equals o.OcupacionID
                                  join ct in contexto.CentrosTrabajoSIRCE on t.CentroTrabajoSIRCEID equals ct.CentroTrabajoSIRCEID
                                  where ct.CentroTrabajoSIRCEID == CentroTrabajoSIRCEID && t.Eliminado == false
                                  select new TrabajadorPOCO
                                  {
                                      TrabajadorID = t.TrabajadorID,
                                      CURP = t.CURP,
                                      Nombre = t.Nombre,
                                      ApellidoPaterno = t.ApellidoPaterno,
                                      ApellidoMaterno = t.ApellidoMaterno,
                                      OcupacionID = t.OcupacionID,
                                      OcupacionDescripcion = o.Descripcion,
                                      EntidadFederativaID = t.EntidadFederativaID,
                                      MunicipioID = t.MunicipioID,
                                      EscolaridadID = t.EscolaridadID.Value,
                                      InstitucionesEducativasID = t.InstitucionesEducativasID.Value,
                                      DocumentosProbatoriosID = t.ComprobanteEstudioID.Value,
                                      Genero = t.Genero,
                                      FechaNacimiento = (DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", t.FechaNacimiento))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", t.FechaNacimiento))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", t.FechaNacimiento)), 4)
                                                               ).Replace(" ", "0"),
                                      Eliminado = (bool)t.Eliminado,
                                      CentroTrabajoSIRCEID = ct.CentroTrabajoSIRCEID
                                  }).ToList();

            if (trabajadorPoco.Count > 0)
            {
                foreach (TrabajadorPOCO item in trabajadorPoco)
                {
                    item.EscolaridadDescripcion = (item.EscolaridadID.HasValue ? escolaridades.First(x => x.catalogoID == item.EscolaridadID.Value).catalogoDescripcion : string.Empty);
                    item.DocumentosProbatoriosDescripcion = (item.DocumentosProbatoriosID.HasValue ? documentosProbatorios.First(x => x.catalogoID == item.DocumentosProbatoriosID).catalogoDescripcion : string.Empty);
                }
            }

            return trabajadorPoco;
        }

        /// <summary>
        /// Método para obtener los todos trabajadores de una empresa, que no pertenecen al centro de trabajo seleccionado.
        /// </summary>
        /// <param name="CentroTrabajoSIRCEID">Clave del centro de trabajo</param>
        /// <returns></returns>
        public List<TrabajadorPOCO> ConsultarTrabajadoresIncorporarEstablecimientosPOCO(int EmpresaSIRCEID, int CentroTrabajoSIRCEID)
        {
            List<CatalogoPOCO> escolaridades = new List<CatalogoPOCO>();
            List<CatalogoPOCO> documentosProbatorios = new List<CatalogoPOCO>();

            escolaridades = ConsultarEscolaridades();
            documentosProbatorios = ConsultarDocumentosProbatorios();

            var trabajadorPoco = (from t in contexto.Trabajadores
                                  join o in contexto.Ocupaciones on t.OcupacionID equals o.OcupacionID
                                  join ct in contexto.CentrosTrabajoSIRCE on t.CentroTrabajoSIRCEID equals ct.CentroTrabajoSIRCEID
                                  join e in contexto.EmpresaCentrosTrabajoSIRCE on t.CentroTrabajoSIRCEID equals e.CentroTrabajoSIRCEID
                                  where e.EmpresaSIRCEID == EmpresaSIRCEID
                                  && e.CentroTrabajoSIRCEID != CentroTrabajoSIRCEID
                                  && t.CentroTrabajoSIRCEID == e.CentroTrabajoSIRCEID
                                  && t.Eliminado == false
                                  select new TrabajadorPOCO
                                  {
                                      TrabajadorID = t.TrabajadorID,
                                      CURP = t.CURP,
                                      Nombre = t.Nombre,
                                      ApellidoPaterno = t.ApellidoPaterno,
                                      ApellidoMaterno = t.ApellidoMaterno,
                                      OcupacionID = t.OcupacionID,
                                      OcupacionDescripcion = o.Descripcion,
                                      EntidadFederativaID = t.EntidadFederativaID,
                                      MunicipioID = t.MunicipioID,
                                      EscolaridadID = t.EscolaridadID.Value,
                                      InstitucionesEducativasID = t.InstitucionesEducativasID.Value,
                                      DocumentosProbatoriosID = t.ComprobanteEstudioID.Value,
                                      Genero = t.Genero,
                                      FechaNacimiento = (DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", t.FechaNacimiento))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", t.FechaNacimiento))), 2)
                                                                + "/"
                                                                + DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", t.FechaNacimiento)), 4)
                                                               ).Replace(" ", "0"),
                                      Eliminado = (bool)t.Eliminado,
                                      CentroTrabajoSIRCEID = e.CentroTrabajoSIRCEID
                                  }).ToList();

            if (trabajadorPoco.Count > 0)
            {
                foreach (TrabajadorPOCO item in trabajadorPoco)
                {
                    item.EscolaridadDescripcion = (item.EscolaridadID.HasValue ? escolaridades.First(x => x.catalogoID == item.EscolaridadID.Value).catalogoDescripcion : string.Empty);
                    item.DocumentosProbatoriosDescripcion = (item.DocumentosProbatoriosID.HasValue ? documentosProbatorios.First(x => x.catalogoID == item.DocumentosProbatoriosID).catalogoDescripcion : string.Empty);
                }
            }

            return trabajadorPoco;
        }

        /// <summary>
        /// Método para obtener todos trabajadores de una empresa.(Nombre - CURP)
        /// </summary>
        /// <param name="empresaID">Clave de la empresa</param>
        /// <returns></returns>
        public List<CatalogoCompuestoPOCO> ConsultarTrabajadoresCatalogoPOCO(int EmpresaSIRCEID, int CentroTrabajoSIRCEID)
        {
            List<CatalogoCompuestoPOCO> listCatalogoCompuestoPOCO = new List<CatalogoCompuestoPOCO>();
            List<TrabajadorPOCO> listTrabajadores = new List<TrabajadorPOCO>();

            //Obtengo todos los trabajadores y sus centros de trabajos
            listTrabajadores = (from t in ConsultarTrabajadoresPOCO(CentroTrabajoSIRCEID)
                                  select t).ToList();

            if (listTrabajadores.Count > 0)
            {
                listCatalogoCompuestoPOCO = GetTrabajadoresPOCO(EmpresaSIRCEID, CentroTrabajoSIRCEID, listTrabajadores);
            }

            return listCatalogoCompuestoPOCO;
        }

        /// <summary>
        /// Método para obtener todos trabajadores de una empresa, la clave es compuesta.
        /// </summary>
        /// <param name="empresaID">Clave de la empresa</param>
        /// <returns></returns>
        public List<CatalogoCompuestoPOCO> ConsultarTrabajadoresCatalogoCompuestoPOCO(int EmpresaSIRCEID, int CentroTrabajoSIRCEID)
        {
            List<CatalogoCompuestoPOCO> listCatalogoCompuestoPOCO = new List<CatalogoCompuestoPOCO>();
            List<TrabajadorPOCO> listTrabajadores = new List<TrabajadorPOCO>();

            //Obtengo todos los trabajadores y sus centros de trabajos
            listTrabajadores = (from t in ConsultarTrabajadoresIncorporarEstablecimientosPOCO(EmpresaSIRCEID, CentroTrabajoSIRCEID)
                                select t).ToList();

            if (listTrabajadores.Count > 0)
            {
                listCatalogoCompuestoPOCO = GetTrabajadoresPOCO(EmpresaSIRCEID, CentroTrabajoSIRCEID, listTrabajadores);
            }

            return listCatalogoCompuestoPOCO;
        }

        private List<CatalogoCompuestoPOCO> GetTrabajadoresPOCO(int EmpresaSIRCEID, int CentroTrabajoSIRCEID, List<TrabajadorPOCO> listTrabajadores)
        {
            List<CatalogoCompuestoPOCO> listCatalogoCompuestoPOCO = new List<CatalogoCompuestoPOCO>();

            //Obtengo la clave original de los centros de trabajo del DNE
            var centrosTrabajoSIRCE = (from lt in listTrabajadores
                                       join ect in contexto.CentrosTrabajoSIRCE on lt.CentroTrabajoSIRCEID equals ect.CentroTrabajoSIRCEID
                                       select new
                                       {
                                           CentroTrabajoID = ect.CentroTrabajoID,
                                           CentroTrabajoSIRCEID = lt.CentroTrabajoSIRCEID
                                       }).Distinct().ToList();

            List<int> listCentrosTrabajos = new List<int>();
            Dictionary<int, int> dCentrosTrabajo = new Dictionary<int, int>();
            foreach (var item in centrosTrabajoSIRCE)
            {
                listCentrosTrabajos.Add(item.CentroTrabajoID);
                dCentrosTrabajo.Add(item.CentroTrabajoSIRCEID, item.CentroTrabajoID);
            }
            //Obtengo la clave de la empresa del DNE.
            int empresaID = contexto.EmpresasSIRCE.FirstOrDefault(x => x.EmpresaSIRCEID == EmpresaSIRCEID).EmpresaID;

            //Recupero la descripción de los centros de trabajo del DNE
            List<CentroTrabajoPOCO> listCentroTrabajoPOCO = new List<CentroTrabajoPOCO>();
            listCentroTrabajoPOCO = new UtileriaDAT().ConsultarCentrosTrabajoPorEstablecimiento(empresaID, listCentrosTrabajos);

            foreach (var item in listTrabajadores)
            {
                CatalogoCompuestoPOCO catalogoCompuestoPOCO = new CatalogoCompuestoPOCO();
                string descripcionCentroTrabajo = string.Empty;

                //Recupero la descripción del Centro de Trabajo del DNE
                foreach (KeyValuePair<int, int> centroTrabajoSIRCEID in dCentrosTrabajo)
                {
                    if (item.CentroTrabajoSIRCEID == centroTrabajoSIRCEID.Key)
                    {
                        descripcionCentroTrabajo = listCentroTrabajoPOCO.FirstOrDefault(x => x.centroTrabajoID == centroTrabajoSIRCEID.Value).centroTrabajoNombre;
                        break;
                    }
                }

                catalogoCompuestoPOCO.catalogoID = item.TrabajadorID.ToString() + "|" + item.CentroTrabajoSIRCEID.ToString();
                catalogoCompuestoPOCO.catalogoDescripcion = string.Format("{0} - {1} {2} {3} / {4}", item.CURP, item.Nombre, item.ApellidoPaterno, item.ApellidoMaterno, descripcionCentroTrabajo);
                listCatalogoCompuestoPOCO.Add(catalogoCompuestoPOCO);
            }

            return listCatalogoCompuestoPOCO;
        }

        /// <summary>
        /// Método para eliminar las normas del trabajador
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public bool EditarNormasTrabajador(ICollection<NormaTrabajador> normas, Trabajadores entidad)
        {
            //Se obtienen todas las normas anteriores
            List<int> listaNormaTrabajador = entidad.NormaTrabajador.Select(x => x.NormaTrabajadorID).ToList();
            // Obtenemos las anteriores entidades.
            var anteriores = normas.Where(x => !listaNormaTrabajador.Contains(x.NormaTrabajadorID));
            foreach (var item in anteriores)
            {
                item.Eliminado = true;
            }

            foreach (var item in normas)
            {
                var registro = entidad.NormaTrabajador.FirstOrDefault(x => x.NormaTrabajadorID == item.NormaTrabajadorID);
                if (registro != null)
                {
                    item.NormaCompetenciaID = registro.NormaCompetenciaID;
                    item.FechaEmision = registro.FechaEmision;
                    item.Eliminado = registro.Eliminado;
                }
            }

            // Obtenemos las nuevas entidades.
            var nuevos = entidad.NormaTrabajador.Where(x => x.NormaTrabajadorID < 1);

            foreach (var item in nuevos)
            {
                normas.Add(item);
            }

            return true;
        }

        /// <summary>
        /// Método para crear las normas del trabajador
        /// </summary>
        /// <param name="normasTrabajador">Entidad de las normas del trabajador</param>
        /// <returns></returns>
        public bool CrearNormasTrabajador(NormaTrabajador normaTrabajador)
        {
            bool resultado = false;
            //Guardar la norma del trabajador
            contexto.NormaTrabajador.Add(normaTrabajador);
            return resultado;
        }

        /// <summary>
        /// Método para obtener las normas del trabajador
        /// </summary>
        /// <param name="trabajadores">Entidad del trabajador</param>
        /// <returns></returns>
        public bool ConsultarNormasTrabajador(Trabajadores trabajadores)
        {
            //obtener todas las normas del trabajador
            var lista = (from nt in contexto.NormaTrabajador
                         where nt.TrabajadorID == trabajadores.TrabajadorID && nt.Eliminado == false
                         select nt).ToList();

            trabajadores.NormaTrabajador = lista;
            return true;
        }

        /// <summary>
        /// Método que obtiene las ocupaciones especificas
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarOcupaciones()
        {
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();
            resultado = (from o in contexto.Ocupaciones
                         select new CatalogoPOCO
                         {
                             catalogoID = o.OcupacionID,
                             catalogoDescripcion = (o.Clave + " - " + o.Descripcion)
                         }).ToList();

            return resultado;
        }

        /// <summary>
        /// Método que obtiene el nivel máximo de estudios terminados
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarEscolaridades()
        {
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();
            resultado = (from e in contexto.Escolaridades
                         select new CatalogoPOCO
                                      {
                                          catalogoID = e.EscolaridadID,
                                          catalogoDescripcion = e.Descripcion
                                      }).ToList();

            return resultado;
        }

        /// <summary>
        /// Método que obtiene los documentos probatorios del trabajador
        /// </summary>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarDocumentosProbatorios()
        {
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();
            resultado = (from dp in contexto.DocumentosProbatorios
                         select new CatalogoPOCO
                         {
                             catalogoID = dp.DocumentoProbatorioID,
                             catalogoDescripcion = dp.Descripcion
                         }).ToList();
            return resultado;
        }

        /// <summary>
        /// Método para obtener las normas o estandares del trabajador
        /// </summary>
        /// <param name="trabajadores"></param>
        /// <returns></returns>
        public List<CatalogoPOCO> ConsultarNormaCompetencia()
        {
            List<CatalogoPOCO> resultado = new List<CatalogoPOCO>();
            resultado = (from nc in contexto.NormasCompetencia
                         select new CatalogoPOCO
                         {
                             catalogoID = nc.NormaCompetenciaID,
                             catalogoDescripcion = (nc.Clave + " - " + nc.Descripcion)
                         }).ToList();
            return resultado;
        }

        public bool ValidaCURP(Trabajadores trabajadores)
        {
            trabajadores.VerificaCURP = true;

            int existe = (from t in contexto.Trabajadores
                          where t.CentroTrabajoSIRCEID == trabajadores.CentroTrabajoSIRCEID &&
                          t.CURP == trabajadores.CURP &&
                          t.Eliminado == false
                          select t).Count();

            if (existe <= 0)
            {
                trabajadores.VerificaCURP = false;
            }

            return trabajadores.VerificaCURP;
        }

        public bool PostTrabajador(Trabajadores trabajador)
        {
            if (trabajador.TrabajadorID == 0)
                contexto.Trabajadores.Add(trabajador);
            else
                contexto.Entry(trabajador).State = EntityState.Modified;
            return true;
        }

        public bool ConsultarOcupacion(Ocupaciones entidad)
        {
            var resultado = contexto.Ocupaciones.Where(x => x.OcupacionID == entidad.OcupacionID).FirstOrDefault();
            if (resultado != null)
            {
                entidad.OcupacionID = resultado.OcupacionID;
                entidad.Descripcion = resultado.Descripcion;
                return true;
            }
            return false;
        }

        public bool ConsultarEscolaridad(Escolaridades entidad)
        {
            var resultado = contexto.Escolaridades.Where(x => x.EscolaridadID == entidad.EscolaridadID).FirstOrDefault();
            if (resultado != null)
            {
                entidad.EscolaridadID = resultado.EscolaridadID;
                entidad.Descripcion = resultado.Descripcion;
                return true;
            }
            return false;
        }
    }
}
