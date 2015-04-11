using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using STPS.Framework;
using STPS.Framework.Excel;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Entidades.POCOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Negocio
{
    public class CargaMasivaNEG : WorkUnit
    {
        #region Constructor
        public CargaMasivaNEG()
        {
        }

        public CargaMasivaNEG(SIRCEEntities context)
            : base(context)
        {
        }
        #endregion Métodos

        /// <summary>
        /// Metodo para leer archivo excel
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <returns></returns>
        public ExcelData ProcesarArchivo(string rutaArchivo)
        {
            var excelData = new ExcelData();
            var data = new ExcelWorkBook();
            var workBook = (new ExcelReader()).ReadExcel(rutaArchivo);

            if (!workBook.Equals(null))
            {
                var sheet = workBook.Sheets.FirstOrDefault();

                if (!sheet.DataRows.Equals(null))
                {
                    excelData = sheet;
                }
            }
            return excelData;
        }

        /// <summary>
        /// Metodo para crear una entidad Constancia
        /// </summary>
        /// <param name="trabajadorID">Id trabajador</param>
        /// <param name="cursoID">ID Curso</param>
        /// <param name="listaCentroTrabajoID">ID Lista centro trabajo</param>
        /// <returns></returns>
        public Constancias CrearEntidadConstancia(string trabajadorID, string cursoID, int listaCentroTrabajoID)
        {
            Constancias constancia = new Constancias();
            constancia.TrabajadorID = Convert.ToInt32(trabajadorID);
            constancia.CursoID = Convert.ToInt32(cursoID);
            constancia.ListaCentroTrabajoID = Convert.ToInt32(listaCentroTrabajoID);
            constancia.Eliminado = false;

            return constancia;
        }

        /// <summary>
        /// Método para agregar una constancia
        /// </summary>
        /// <returns></returns>
        public bool CrearConstancia(Constancias constancia)
        {
            using (ConstanciaNEG negocio = new ConstanciaNEG())
            {
                negocio.CrearConstancia(constancia);
            }
            return true;
        }

        /// <summary>
        /// Método para crear una entidad Cursos
        /// </summary>
        /// <param name="row">Registro excel</param>
        /// <param name="empresaId">id empresa</param>
        /// <returns>Entidad Cursos</returns>
        private Cursos CrearEntidadCurso(ConstanciaCMPOCO datos, int empresaId)
        {
            Cursos curso = new Cursos();
            curso.EmpresaSIRCEID = empresaId;
            curso.Nombre = datos.NombreCurso;
            curso.AreaTematicaID = Convert.ToInt32(datos.AreaTematicaId);
            curso.Duracion = Convert.ToByte(datos.Duracion);

            curso.FechaInicio = Utilidades.ConvertirFecha(datos.FechaInicio);
            curso.FechaTermino = Utilidades.ConvertirFecha(datos.FechaTermino);

            curso.TipoAgenteCapacitadorID = Convert.ToByte(datos.TipoAgenteCapacitadorID);
            curso.RegistroAgenteExterno = datos.RegistroAgenteExterno;
            curso.ModalidadCapacitacionID = Convert.ToByte(datos.ModalidadCapacitacionID);
            curso.ObjetivoCapacitacionID = Convert.ToInt32(datos.ObjetivoCapacitacionID);

            return curso;
        }

        /// <summary>
        /// Método para crear una entidad Trabajadores
        /// </summary>
        /// <param name="row">registro excel</param>
        /// <param name="centroTrabajoID">ID centro trabajo</param>
        /// <returns></returns>
        public Trabajadores CrearEntidadTrabajadores(ConstanciaCMPOCO datos, int centroTrabajoID)
        {
            Trabajadores trabajador = new Trabajadores();

            trabajador.CentroTrabajoSIRCEID = centroTrabajoID;

            trabajador.CURP = datos.CURP;
            trabajador.Genero = Convert.ToByte(new TrabajadorNEG().ConsultarGeneros().First(x => x.catalogoDescripcion.Substring(0, 1).ToUpper() == trabajador.CURP.Substring(10, 1).ToString().ToUpper()).catalogoID);
            trabajador.FechaNacimiento = Utilidades.ConvertirFecha(string.Format("{2}/{1}/{0}", trabajador.CURP.Substring(4, 2), trabajador.CURP.Substring(6, 2), trabajador.CURP.Substring(8, 2)));
            trabajador.Nombre = datos.NombreTrabajador;

            trabajador.ApellidoPaterno = datos.ApellidoPaterno;
            trabajador.ApellidoMaterno = datos.ApellidoMaterno;
            trabajador.EntidadFederativaID = Convert.ToInt32(datos.EntidadFederativaID);
            trabajador.MunicipioID = Convert.ToInt32(datos.MunicipioID);
            trabajador.OcupacionID = Convert.ToInt32(datos.OcupacionID);
            //Normas
            trabajador.EscolaridadID = Convert.ToInt32(datos.EscolaridadID);
            trabajador.InstitucionesEducativasID = Convert.ToInt32(datos.InstitucionesEducativasID);

            trabajador.VerificaCURP = false;

            return trabajador;
        }

        public ConstanciaCMPOCO CrearEntidadConstanciaCMPOCO(List<string> fila, int centroTrabajoID, int empresaID)
        {
            ConstanciaCMPOCO preconstancia = new ConstanciaCMPOCO();
            preconstancia.CursoDatosValidos = true;
            preconstancia.TrabajadorDatosValidos = true;

            var existeTrabajador = ValidarTrabajadorID(preconstancia, fila, 0);
            var existeCurso = ValidarCursoID(preconstancia, fila, 12);

            ValidarCURP(preconstancia, fila, 1);
            ValidarNombreTrabajador(preconstancia, fila);
            ValidarEntidadFederativa(preconstancia, fila, 5);
            ValidarMunicipio(preconstancia, fila, 6);
            ValidarOcupacion(preconstancia, fila, 7);
            ValidarEscolaridad(preconstancia, fila, 10);
            ValidarInstitucion(preconstancia, fila, 11);

            if (!existeTrabajador && preconstancia.TrabajadorDatosValidos)
            {
                Trabajadores trabajador = CrearEntidadTrabajadores(preconstancia, centroTrabajoID);
                existeTrabajador = new TrabajadorNEG().Crear(trabajador);
                preconstancia.TrabajadorID = trabajador.TrabajadorID.ToString();
            }

            ValidarNombreCurso(preconstancia, fila, 13);
            ValidarAreaTematica(preconstancia, fila, 14);
            ValidaDuracion(preconstancia, fila, 15);
            ValidaFechaInicioFin(preconstancia, fila);
            ValidarTipoAgente(preconstancia, fila, 18);
            ValidarAgenteExterno(preconstancia, fila, 19);
            ValidarModalidad(preconstancia, fila, 20);
            ValidarObjetivoCapacitacion(preconstancia, fila, 21);

            if (!existeCurso && preconstancia.CursoDatosValidos)
            {
                Cursos curso = CrearEntidadCurso(preconstancia, empresaID);
                existeCurso = new CursoNEG().Crear(curso);
                preconstancia.CursoId = curso.CursoID.ToString();
            }

            return preconstancia;
        }

        /// <summary>
        /// Método para validar si un trabajador se encuentra registrado
        /// </summary>
        /// <param name="trabajadorID"></param>
        /// <param name="cursoID"></param>
        /// <returns></returns>
        public bool ValidarTrabajadorID(ConstanciaCMPOCO datos, List<string> fila, int indice)
        {
            var resultado = false;
            var negocio = new TrabajadorNEG(contextoSIRCE);

            if (indice < fila.Count)
            {
                datos.TrabajadorID = fila[indice].Trim();

                if (ValidarTipoDato(new Trabajadores().TrabajadorID, datos.TrabajadorID))
                {
                    //validamos si el trabajador existe
                    var trabajador = negocio.ConsultarTrabajador(Convert.ToInt32(datos.TrabajadorID));
                    if (!trabajador.CURP.Equals(null) || !trabajador.CURP.Equals(string.Empty))
                        resultado = true;
                    else
                        datos.TrabajadorIDValido = "Clave trabajador inexistente";
                }
                else
                {
                    datos.TrabajadorIDValido = "Clave trabajador inválida";
                }
            }
            else
            {
                datos.TrabajadorID = string.Empty; ;
                datos.TrabajadorIDValido = "Clave trabajador obligatorio";
            }

            return resultado;
        }

        /// <summary>
        /// Método para validar si un curso existe
        /// </summary>
        /// <param name="trabajadorID"></param>
        /// <param name="cursoID"></param>
        /// <returns></returns>
        public bool ValidarCursoID(ConstanciaCMPOCO datos, List<string> fila, int indice)
        {
            var cursoNeg = new CursoNEG(contextoSIRCE);
            var curso = new Cursos();
            var resultado = false;

            if (indice < fila.Count)
            {
                datos.CursoId = fila[indice].Trim();

                if (ValidarTipoDato(new Cursos().CursoID, datos.CursoId))
                {
                    // validamos si el curso ya existe
                    curso.CursoID = Convert.ToInt32(datos.CursoId);
                    resultado = cursoNeg.Consultar(curso);

                    if (!resultado)
                        datos.CursoIdValido = "Clave curso inexistente";
                }
                else
                {
                    datos.CursoIdValido = "Clave curso inválida";
                }
            }
            else
            {
                datos.CursoId = string.Empty;
                datos.TrabajadorIDValido = "Clave curso obligatorio";
            }

            return resultado;
        }

        /// <summary>
        /// Método para validar si existe la relación listaCentroTrabajoID
        /// </summary>
        /// <returns></returns>
        //public bool ValidarCentroTrabajoID(ConstanciaCMPOCO datosListaCtroTrabajo)
        //{
   
        //}

        /// <summary>
        /// Método que valida si el curp es válido y si ya se encuentra registrado
        /// </summary>
        /// <param name="datosTrabajador"></param>
        public void ValidarCURP(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            TrabajadorNEG negocio = new TrabajadorNEG();

            if (indice < fila.Count)
            {
                datosTrabajador.CURP = fila[indice].Trim();

                if (datosTrabajador.CURP != string.Empty && negocio.ValidarFormatoCURP(datosTrabajador.CURP))
                {
                    //agregar centro trabajo, no es posible tener dos trabajadores en un mismo centro
                    if (!negocio.ValidaCURP(new Trabajadores() { CURP = datosTrabajador.CURP }))
                    {
                        datosTrabajador.CURPValido = "CURP ya se encuentra registrado";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosTrabajador.CURPValido = "CURP inválido";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.CURP = string.Empty; ;
                datosTrabajador.CURPValido = "CURP inválido";
                datosTrabajador.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método que valida si el nombre es válido
        /// </summary>
        /// <param name="trabajador"></param>
        public void ValidarNombreTrabajador(ConstanciaCMPOCO datosTrabajador, List<string> fila)
        {
            //VALIDA NOMBRE
            if (2 < fila.Count && fila[2].Trim() != string.Empty)
            {
                datosTrabajador.NombreTrabajador = fila[2].Trim();

                if (datosTrabajador.NombreTrabajador.Length > 50)
                {
                    datosTrabajador.NombreTrabajadorValido = "Nombre trabajador excede longitud máxima";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.NombreTrabajador = string.Empty; ;
                datosTrabajador.NombreTrabajadorValido = "Nombre trabajador obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }

            //VALIDA APELLIDO PATERNO
            if (3 < fila.Count && fila[3].Trim() != string.Empty)
            {
                datosTrabajador.ApellidoPaterno = fila[3].Trim();

                if (datosTrabajador.ApellidoPaterno.Length > 50)
                {
                    datosTrabajador.ApellidoPaternoValido = "Apellido paterno excede longitud máxima";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.ApellidoPaterno = string.Empty; ;
                datosTrabajador.ApellidoPaternoValido = "Apellido paterno obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }

            //VALIDA APELLIDO MATERNO 
            if (4 < fila.Count && fila[4].Trim() != string.Empty)
            {
                datosTrabajador.ApellidoMaterno = fila[4].Trim();

                if (datosTrabajador.ApellidoMaterno.Length > 50)
                {
                    datosTrabajador.ApellidoMaternoValido = "Apellido paterno excede longitud máxima";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.ApellidoMaterno = string.Empty; ;
                datosTrabajador.ApellidoMaternoValido = "Apellido paterno obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para validar id entidad federativa
        /// </summary>
        /// <param name="datosTrabajador"></param>
        /// <param name="dato"></param>
        public void ValidarEntidadFederativa(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            UtileriaNeg utilNegocio = new UtileriaNeg();
            CatalogoPOCO entidad = new CatalogoPOCO();
            //datosTrabajador.TrabajadorDatosValidos = false;

            if (indice < fila.Count)
            {
                datosTrabajador.EntidadFederativaID = fila[indice].Trim();

                if (ValidarTipoDato(new Trabajadores().EntidadFederativaID, datosTrabajador.EntidadFederativaID))
                {
                    entidad.catalogoID = Convert.ToInt32(datosTrabajador.EntidadFederativaID);

                    if (!utilNegocio.ConsultarEntidad(entidad))
                    {
                        datosTrabajador.EntidadFederativaIdValido = "Entidad federativa inexistente";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosTrabajador.EntidadFederativaIdValido = "Entidad federativa obligatorio";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.EntidadFederativaID = string.Empty;
                datosTrabajador.EntidadFederativaIdValido = "Entidad federativa obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }

        }

        /// <summary>
        /// Método para validar id Municipio
        /// </summary>
        /// <param name="datosTrabajador"></param>
        /// <param name="fila"></param>
        public void ValidarMunicipio(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            UtileriaNeg utilNegocio = new UtileriaNeg();
            CatalogoPOCO entidad = new CatalogoPOCO();

            //datosTrabajador.TrabajadorDatosValidos = false;

            if (indice < fila.Count)
            {
                datosTrabajador.MunicipioID = fila[indice].Trim();

                if (ValidarTipoDato(new Trabajadores().MunicipioID, datosTrabajador.MunicipioID) && datosTrabajador.EntidadFederativaIdValido == null)
                {
                    entidad.catalogoID = Convert.ToInt32(datosTrabajador.MunicipioID);

                    if (!utilNegocio.ConsultarMunicipio(entidad, Convert.ToInt32(datosTrabajador.EntidadFederativaID)))
                    {
                        datosTrabajador.MunicipioIdValido = "Municipio inexistente";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosTrabajador.MunicipioIdValido = "Municipio inválido";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.MunicipioID = string.Empty;
                datosTrabajador.MunicipioIdValido = "Municipio obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para validar id ocupación
        /// </summary>
        /// <param name="datosTrabajador"></param>
        /// <param name="fila"></param>
        public void ValidarOcupacion(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            TrabajadorNEG negocio = new TrabajadorNEG();
            Ocupaciones entidad = new Ocupaciones();

            //datosTrabajador.TrabajadorDatosValidos = false;

            if (indice < fila.Count)
            {
                datosTrabajador.OcupacionID = fila[indice].Trim();

                if (ValidarTipoDato(new Trabajadores().OcupacionID, datosTrabajador.OcupacionID))
                {
                    entidad.OcupacionID = Convert.ToInt32(datosTrabajador.OcupacionID);

                    if (!negocio.ConsultarOcupacion(entidad))
                    {
                        datosTrabajador.OcupacionIdValido = "Ocupación inexistente";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosTrabajador.OcupacionIdValido = "Ocupación inválido";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.OcupacionID = string.Empty;
                datosTrabajador.OcupacionIdValido = "Ocupación obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para vaidar id escolaridad
        /// </summary>
        /// <param name="datosTrabajador"></param>
        /// <param name="fila"></param>
        public void ValidarEscolaridad(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            TrabajadorNEG negocio = new TrabajadorNEG();
            Escolaridades entidad = new Escolaridades();

            //datosTrabajador.TrabajadorDatosValidos = false;

            if (indice < fila.Count)
            {
                datosTrabajador.EscolaridadID = fila[indice].Trim();

                if (ValidarTipoDato(new Trabajadores().OcupacionID, datosTrabajador.EscolaridadID))
                {
                    entidad.EscolaridadID = Convert.ToInt32(datosTrabajador.EscolaridadID);

                    if (!negocio.ConsultarEscolaridad(entidad))
                    {
                        datosTrabajador.EscolaridadIdValido = "Ocupacion inexistente";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }

                }
                else
                {
                    datosTrabajador.EscolaridadIdValido = "Escolaridad inválido";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.EscolaridadID = string.Empty;
                datosTrabajador.EscolaridadIdValido = "Ocupación obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }

        }

        /// <summary>
        /// Método para validar id Institucion
        /// </summary>
        /// <param name="datosTrabajador"></param>
        /// <param name="fila"></param>
        public void ValidarInstitucion(ConstanciaCMPOCO datosTrabajador, List<string> fila, int indice)
        {
            TrabajadorNEG negocio = new TrabajadorNEG();
            CatalogoPOCO entidad = new CatalogoPOCO();
            Trabajadores trabajador = new Trabajadores();
            trabajador.InstitucionesEducativasID = 0;

            //datosTrabajador.TrabajadorDatosValidos = false;

            if (indice < fila.Count)
            {
                datosTrabajador.InstitucionesEducativasID = fila[indice].Trim();

                if (ValidarTipoDato(trabajador.InstitucionesEducativasID, datosTrabajador.InstitucionesEducativasID))
                {
                    entidad.catalogoID = Convert.ToInt32(datosTrabajador.InstitucionesEducativasID);

                    if (!negocio.ConsultarInstitucionEducativa(entidad))
                    {
                        datosTrabajador.InstitucionesEducativasIdValido = "Institución inexistente";
                        datosTrabajador.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosTrabajador.InstitucionesEducativasIdValido = "institución inválida";
                    datosTrabajador.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosTrabajador.InstitucionesEducativasID = string.Empty;
                datosTrabajador.InstitucionesEducativasIdValido = "Institución educativa obligatorio";
                datosTrabajador.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para validar el campo nombre del curso.
        /// </summary>
        /// <param name="datosCurso"></param>
        /// <param name="fila"></param>
        /// <param name="indice"></param>
        public void ValidarNombreCurso(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count && fila[indice].ToString().Trim() != string.Empty)
            {
                datosCurso.NombreCurso = fila[indice].ToString().Trim();

                if (datosCurso.NombreCurso.Length > 50)
                {
                    datosCurso.NombreCursoValido = "Nombre curso excede longitud";
                    datosCurso.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosCurso.NombreCurso = string.Empty;
                datosCurso.NombreCursoValido = "Nombre curso obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para validar un id área temática
        /// </summary>
        /// <param name="datosCurso"></param>
        /// <param name="fila"></param>
        public void ValidarAreaTematica(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            CursoNEG negocio = new CursoNEG();
            AreasTematicas entidad = new AreasTematicas();

            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count)
            {
                datosCurso.AreaTematicaId = fila[indice].Trim();

                if (ValidarTipoDato(new Cursos().AreaTematicaID, datosCurso.AreaTematicaId))
                {
                    entidad.AreaTematicaID = Convert.ToInt32(datosCurso.AreaTematicaId);

                    if (!negocio.ConsultarAreaTematica(entidad))
                    {
                        datosCurso.AreaTematicaIdValido = "Area temática inexistente";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.AreaTematicaIdValido = "Area temática inválido";
                    datosCurso.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosCurso.AreaTematica = string.Empty;
                datosCurso.AreaTematicaIdValido = "Area temática obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Método para validar un id tipo agente
        /// </summary>
        public void ValidarTipoAgente(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            CursoNEG negocio = new CursoNEG();
            CatalogoPOCO entidad = new CatalogoPOCO();

            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count)
            {
                datosCurso.TipoAgenteCapacitadorID = fila[indice].Trim();

                if (ValidarTipoDato(new Cursos().TipoAgenteCapacitadorID, datosCurso.TipoAgenteCapacitadorID))
                {
                    entidad.catalogoID = Convert.ToInt32(datosCurso.TipoAgenteCapacitadorID);

                    if (!negocio.ConsultarTipoAgente(entidad))
                    {
                        datosCurso.TipoAgenteCapacitadorValido = "Tipo gente inexistente";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.TipoAgenteCapacitadorValido = "Tipo agente inválido";
                    datosCurso.CursoDatosValidos = false;
                }
            }
            else
            {
                datosCurso.TipoAgenteCapacitadorID = string.Empty;
                datosCurso.TipoAgenteCapacitadorValido = "Tipo agente capacitador obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }
        }

        public void ValidarAgenteExterno(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            if (indice < fila.Count)
                datosCurso.RegistroAgenteExterno = fila[indice].Trim();
            else
                datosCurso.RegistroAgenteExterno = string.Empty;
        }

        /// <summary>
        /// Mpetodo para valida un id modalidad
        /// </summary>
        /// <param name="datosCurso"></param>
        /// <param name="fila"></param>
        public void ValidarModalidad(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            CursoNEG negocio = new CursoNEG();
            CatalogoPOCO entidad = new CatalogoPOCO();

            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count)
            {
                datosCurso.ModalidadCapacitacionID = fila[indice].Trim();

                if (ValidarTipoDato(new Cursos().ModalidadCapacitacionID, datosCurso.ModalidadCapacitacionID))
                {
                    entidad.catalogoID = Convert.ToInt32(datosCurso.ModalidadCapacitacionID);

                    if (!negocio.ConsultarModalidad(entidad))
                    {
                        datosCurso.ModalidadCapacitacionValido = "Modalidad inexistente";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.ModalidadCapacitacionValido = "Modalidad inválido";
                    datosCurso.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosCurso.ModalidadCapacitacionID = string.Empty;
                datosCurso.ModalidadCapacitacionValido = "Modalidad obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }
        }

        /// <summary>
        /// Mpetodo para valida un id objetivo capacitación
        /// </summary>
        /// <param name="datosCurso"></param>
        /// <param name="fila"></param>
        public void ValidarObjetivoCapacitacion(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            CursoNEG negocio = new CursoNEG();
            ObjetivosCapacitacion entidad = new ObjetivosCapacitacion();

            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count)
            {
                datosCurso.ObjetivoCapacitacionID = fila[indice].Trim();

                if (ValidarTipoDato(new Cursos().ObjetivoCapacitacionID, datosCurso.ObjetivoCapacitacionID))
                {
                    entidad.ObjetivoCapacitacionID = Convert.ToInt32(datosCurso.ObjetivoCapacitacionID);

                    if (!negocio.ConsultarObjetivoCapacitacion(entidad))
                    {
                        datosCurso.ObjetivoCapacitacionValido = "Objetivo capacitación inexistente";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.ObjetivoCapacitacionValido = "Objetivo capacitación inválido";
                    datosCurso.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosCurso.ObjetivoCapacitacionID = string.Empty;
                datosCurso.ObjetivoCapacitacionValido = "Objetivo capacitación obligatorio";
                datosCurso.TrabajadorDatosValidos = false;

            }
        }

        /// <summary>
        /// Método para validar el campo duración del curos
        /// </summary>
        /// <param name="datosCurso"></param>
        /// <param name="fila"></param>
        /// <param name="indice"></param>
        public void ValidaDuracion(ConstanciaCMPOCO datosCurso, List<string> fila, int indice)
        {
            //datosCurso.CursoDatosValidos = false;

            if (indice < fila.Count)
            {
                datosCurso.Duracion = fila[indice].Trim();

                if (!ValidarTipoDato(new Cursos().Duracion, datosCurso.Duracion))
                {
                    datosCurso.DuracionValida = "Duración inválida";
                    datosCurso.TrabajadorDatosValidos = false;
                }
                //no es campo obligatorio
            }
        }

        public void ValidaFechaInicioFin(ConstanciaCMPOCO datosCurso, List<string> fila)
        {
            var fechaInicio = new DateTime();

            //datosCurso.CursoDatosValidos = false;

            if (16 < fila.Count)
            {
                datosCurso.FechaInicio = fila[16].Trim();

                if (ValidarTipoDato(new Cursos().FechaInicio, datosCurso.FechaInicio))
                {
                    fechaInicio = Convert.ToDateTime(fila[16]);

                    if (fechaInicio < DateTime.Now)
                    {
                        datosCurso.FechaInicioValida = "Fecha inicio no puede ser menor a la fecha actual";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.FechaInicioValida = "Fecha inicio inválida";
                    datosCurso.TrabajadorDatosValidos = false;
                }
            }
            else
            {
                datosCurso.FechaInicio = string.Empty;
                datosCurso.FechaInicioValida = "Fecha inicio obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }

            //datosCurso.CursoDatosValidos = false;

            if (17 < fila.Count)
            {
                datosCurso.FechaTermino = fila[17].Trim();

                if (ValidarTipoDato(new Cursos().FechaTermino, datosCurso.FechaTermino))
                {
                    var fechaTermino = Convert.ToDateTime(fila[17]);

                    if (fechaTermino < DateTime.Now && fechaTermino < fechaInicio)
                    {
                        datosCurso.FechaTerminoValida = "Fecha termino no puede ser menor a la fecha inicio";
                        datosCurso.TrabajadorDatosValidos = false;
                    }
                }
                else
                {
                    datosCurso.FechaTerminoValida = "Fecha inicio inválida";
                    datosCurso.CursoDatosValidos = false;
                }
            }
            else
            {
                datosCurso.FechaTermino = string.Empty;
                datosCurso.FechaTerminoValida = "Fecha inicio obligatorio";
                datosCurso.TrabajadorDatosValidos = false;
            }

        }

        /// <summary>
        /// Méto para validar el tipo y valor de un dato
        /// </summary>
        /// <param name="referencia"></param>
        /// <param name="dato"></param>
        /// <returns></returns>
        public bool ValidarTipoDato(object referencia, string dato)
        {
            var resultado = false;
            int valorInt32;
            byte valorByte;
            DateTime valorDate;

            var tipoReferencia = referencia.GetType();

            if (dato.Equals(string.Empty))
                resultado = false;
            if (tipoReferencia.FullName.Contains("Int32"))
                resultado = Int32.TryParse(dato, out valorInt32);
            if (tipoReferencia.FullName.Contains("Byte"))
                resultado = Byte.TryParse(dato, out valorByte);
            if (tipoReferencia.FullName.Contains("DateTime"))
                resultado = DateTime.TryParse(dato, out valorDate);
            if (tipoReferencia.FullName.Contains("String") && dato != string.Empty)
                resultado = true;

            return resultado;
        }

    }
}
