using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;

namespace STPS.Framework.Excel
{
    public class ExcelStatus
    {
        /// <summary>
        /// Mensaje
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indica si fue exitoso o no
        /// </summary>
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    /// <summary>
    /// Clase auxiliar de datos al exportar a excel
    /// </summary>
    public class ExcelData
    {
        /// <summary>
        /// Estatus
        /// </summary>
        public ExcelStatus Status { get; set; }

        /// <summary>
        /// Columnas
        /// </summary>
        public Columns ColumnConfigurations { get; set; }

        /// <summary>
        /// Encabezados
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// Información necesaria
        /// </summary>
        public List<List<string>> DataRows { get; set; }

        /// <summary>
        /// Nombre de la hoja
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ExcelData()
        {
            Status = new ExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }
}
