using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace STPS.Framework
{
    public static class Utilidades
    {
        /// <summary>
        /// Método que convierte una fecha en DateTime de acuerdo a la cultura.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime ConvertirFecha(string fecha)
        {
            DateTime dtFecha = new DateTime();
            dtFecha = Convert.ToDateTime(fecha, new CultureInfo("es-MX"));
            return dtFecha;
        }

        /// <summary>
        /// Método que convierte una fecha en DateTime de acuerdo a la cultura.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime ConvertirFechaHora(string fecha)
        {
            DateTime fechaH = Convert.ToDateTime(fecha, new CultureInfo("es-MX"));
            fechaH = new DateTime(fechaH.Year,fechaH.Month,fechaH.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
            DateTime dtFecha = new DateTime();
            dtFecha = Convert.ToDateTime(fechaH, new CultureInfo("es-MX"));
            return dtFecha;
        }

        /// <summary>
        /// Método que convierte una fecha en DateTime de acuerdo a la cultura.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static String ConvertirTexto(DateTime fecha)
        {
            String fechaString = String.Empty;
            fechaString = fecha.ToString("d", new CultureInfo("es-MX"));
            return fechaString;
        }

        /// <summary>
        /// Método que convierte una fecha con hora en DateTime de acuerdo a la cultura.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static String ConvertirTextoFechaHora(DateTime fecha)
        {
            String fechaString = String.Empty;
            fechaString = fecha.ToString(new CultureInfo("es-MX"));
            return fechaString;
        }
    }
}
