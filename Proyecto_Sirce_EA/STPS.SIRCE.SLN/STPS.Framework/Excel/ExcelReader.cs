using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace STPS.Framework.Excel
{
    public class ExcelReader
    {
        /// <summary>
        /// Método para obtener el nombre de una columna
        /// </summary>
        /// <param name="cellReference">Referencia de celda</param>
        /// <returns>Nombre de la columna</returns>
        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Método para convertir nombre de columna en valor numérico
        /// </summary>
        /// <param name="columnName">Nombre de la columna</param>
        /// <returns>Valor numérico</returns>
        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64;
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        /// <summary>
        /// Obtener enumerador de celdas de un registro
        /// </summary>
        /// <param name="row">Registro</param>
        /// <returns>Colección de celdas</returns>
        public IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        /// <summary>
        /// Método para leer una celda
        /// </summary>
        /// <param name="cell">Cedla</param>
        /// <param name="workbookPart">Objeto tipo WorkbookPart</param>
        /// <returns>Valor de celda</returns>
        private string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        /// <summary>
        /// Método para leer un archivo Excel
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Objeto tipo SlExcelWorkBook</returns>
        public ExcelWorkBook ReadExcel(string nombreArchivo)
        {

            var data = new ExcelWorkBook();

            var sheetNumber = 0;
            using (var document = SpreadsheetDocument.Open(nombreArchivo, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                int totalSheets = workbookPart.WorksheetParts.Count() - 1;

                for (int sheetPosition = totalSheets; sheetPosition >= 0; sheetPosition--)
                {
                    var worksheetPart = workbookPart.WorksheetParts.ToList()[sheetPosition];
                    var sheet = new ExcelData();
                    ReadSheet(worksheetPart, sheet, workbookPart, sheetNumber);
                    
                    data.Sheets.Add(sheet);
                    sheetNumber++;
                }
               
                document.Close();
            }

            return data;

        }

        /// <summary>
        /// Método para leer hoja
        /// </summary>
        /// <param name="worksheetPart">Objeto tipo WorksheetPart</param>
        /// <param name="data">Objeto tipo SlExcelData</param>
        /// <param name="workbookPart">Objeto tipo WorkbookPart</param>
        /// <param name="sheetNumber">Número de hoja</param>
        private void ReadSheet(WorksheetPart worksheetPart, ExcelData data, WorkbookPart workbookPart, int sheetNumber)
        {

            try
            {
                data.SheetName = workbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetNumber).Name;

                var workSheet = worksheetPart.Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();

                List<Row> rows = sheetData.Elements<Row>().ToList();

                if (rows.Count > 0)
                {
                    var row = rows[0];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        data.Headers.Add(text);
                    }
                }

                if (rows.Count > 1)
                {
                    for (var i = 1; i < rows.Count; i++)
                    {
                        var dataRow = new List<string>();
                        data.DataRows.Add(dataRow);
                        var row = rows[i];
                        var cellEnumerator = GetExcelCellEnumerator(row);
                        while (cellEnumerator.MoveNext())
                        {
                            var cell = cellEnumerator.Current;
                            var text = ReadExcelCell(cell, workbookPart).Trim();
                            dataRow.Add(text);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                data.Status.Message = "No se puede abrir el documento";
            }
        }
    }
}
