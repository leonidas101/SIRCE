using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Datos;
using STPS.Framework;

namespace STPS.SIRCE.Negocio
{
    public class UtileriaNeg
    {
        private UtileriaDAT objUtileriaDAT = new UtileriaDAT();
        #region Conexiones a Catalogos Institucionale
        public List<CatalogoPOCO> ConsultarEntidades()
        {
            return objUtileriaDAT.ConsultarEntidades();
        }

        public List<CatalogoPOCO> ConsultarMunicipios(int? municipioID)
        {
            return objUtileriaDAT.ConsultarMunicipios(municipioID);
        }
        #endregion

        #region Conexiones a Directorio Nacional de Empresas
        public List<EmpresaPOCO> ConsultarEmpresas(FiltrosEmpresaPOCO filtrosBusquedaEmpresa)
        {
            return objUtileriaDAT.ConsultarEmpresas(filtrosBusquedaEmpresa);
        }

        public List<CentroTrabajoPOCO> ConsultarCentrosTrabajo(CentroTrabajoPOCO centroTrabajo)
        {
            return objUtileriaDAT.ConsultarCentrosTrabajo(centroTrabajo);
        }

        public List<CentroTrabajoPOCO> ConsultarCentrosTrabajoPorEstablecimiento(int EmpresaID, List<int> CentroTrabajoID)
        {
            return objUtileriaDAT.ConsultarCentrosTrabajoPorEstablecimiento(EmpresaID, CentroTrabajoID);
        }

        public List<CentroTrabajoPOCO> ConsultarSCIANCentrosTrabajo(List<CentroTrabajoPOCO> centrosTrabajo)
        {
            return objUtileriaDAT.ConsultarSCIANCentrosTrabajo(centrosTrabajo);
        }
        #endregion

        public List<CatalogoPOCO> EnumeradorALista<T>() where T : struct
        {
            Type enumType = typeof(T);
            List<CatalogoPOCO> catalogo = new List<CatalogoPOCO>();

            // Validamos que sea de tipo Enum
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            // Obtenemos los elementos del enumerador
            var elementos = Enum.GetValues(enumType);

            // Para cada elemento del enumerador, creamos su CatalogoPOCO
            foreach (var enumerador in elementos)
            {
                object obj = Enum.ToObject(enumType, (int)enumerador);
                // Obtenemos la descripcion del elemento
                var descripcion = EnumHelper.ToDescription((Enum)obj);
                // Agregamos a la lista el nuevo POCO
                catalogo.Add(new CatalogoPOCO { catalogoID = (int)enumerador, catalogoDescripcion = descripcion });
            }

            return catalogo;
        }

        public bool ConsultarEntidad(CatalogoPOCO entidad)
        {
            UtileriaDAT datos = new UtileriaDAT();
            var resultado = true;

            try
            {
                resultado = datos.ConsultarEntidad(entidad);
            }
            catch (Exception ex)
            {
                Log.SetLog(ex);
                throw;
            }

            return resultado;
        }

        public bool ConsultarMunicipio(CatalogoPOCO entidad, int idEntidad)
        {
            UtileriaDAT datos = new UtileriaDAT();
            var resultado = true;

            try
            {
                resultado = datos.ConsultarMunicipo(entidad, idEntidad);
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
