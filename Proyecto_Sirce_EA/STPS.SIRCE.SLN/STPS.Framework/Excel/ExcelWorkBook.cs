using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.Framework.Excel
{
    public class ExcelWorkBook
    {
         /// <summary>
        /// Estatus de la hoja de cálculo
        /// </summary>
        public ExcelStatus Status { get; set; }
              
        /// <summary>
        /// Hojas que componen el archivo
        /// </summary>
        public List<ExcelData> Sheets { get; set; } 


        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ExcelWorkBook()
        {
            Status = new ExcelStatus();
            this.Sheets = new List<ExcelData>();
        }
    }
}
