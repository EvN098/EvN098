//archivo Helper del programa excel de Hospitalizados
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaExcelEmail
{
    public class Helper
    {


        public void ExportDataSet(DataSet ds, string destination)
        {
            var workbook = new XLWorkbook();
            //agregando hojas por cada tabla en el dataset
            workbook.Worksheets.Add(ds);

            
            string[] SheetsArray = {     "LEYENDA_HOJAS",
                                         "HOSP_CARDIO",
                                         "HOSP_CIRU_PED",
                                         "HOSP_DONAC",
                                         "HOSP_ONCOL",
                                         "HOSP_ESP_PED",
                                         "HOSP_ESP_QUI",
                                         "HOSP_HEMA",
                                         "HOSP_NEURO",
                                         "HOSP_QUEM",
                                         "HOSP_TPH",
                                         "UCI_CARDIO",
                                         "UCI_CARDV",
                                         "UCI_NEONAT",
                                         "UCI_NEURO",
                                         "UCI_PEDIAT",
                                         "UCI_QUEM",
                                         "UCIN"     };

            //iterando hojas del excel
            for (int i = 1; i <= workbook.Worksheets.Count(); i++)
            {
                //Quitando formato por defecto del closedxml
                workbook.Worksheet(i).Tables.FirstOrDefault().Theme = XLTableTheme.None;
         
                /*
                if (i > 1)
                {
                    //Eliminando las dos primeras columnas 
                    //workbook.Worksheet(i).Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(180, 198, 231);
                    workbook.Worksheet(i).Range(1, 1, 1, k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(180, 198, 231));
                    workbook.Worksheet(i).Column(1).Delete();
                    workbook.Worksheet(i).Column(1).Delete();
                    workbook.Worksheet(i).Columns().AdjustToContents();
                }
                */
                //Renombrando hojas de excel
                workbook.Worksheet(i).Name = SheetsArray[i-1];

                //Añadiendo color a las cabeceras de las columnas
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("FechaHoraEvolucion").Fill.SetBackgroundColor(XLColor.FromArgb(191, 191, 191));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("PreAlta_Alta").Fill.SetBackgroundColor(XLColor.FromArgb(191, 191, 191));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("Dificultades").Fill.SetBackgroundColor(XLColor.FromArgb(191, 191, 191));

                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("FechaProbableAlta").Fill.SetBackgroundColor(XLColor.FromArgb(250, 250, 250));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("NudoCritico").Fill.SetBackgroundColor(XLColor.FromArgb(250, 250, 250));

                //workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("NudoCritico").Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("NudoCritico").Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //workbook.Worksheet(i).Column(k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left; //Alineamos horizontalmente
                //workbook.Worksheet(i).Column(k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente

                //Agregando todos los bordes a las celdas
                workbook.Worksheet(i).RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                workbook.Worksheet(i).RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                workbook.Worksheet(i).RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                workbook.Worksheet(i).RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                workbook.Worksheet(i).RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;
                workbook.Worksheet(i).RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;

                
                //Buscando rango de celdas usadas
                var ws = workbook.Worksheet(i);
                var range = ws.RangeUsed();

                //Buscando valor de columna "FechaProbableAlta" que es la ultima columna de las hojas de excel
                var cell2 = range.FindColumn(c => c.FirstCell().Value.ToString().Contains("FechaProbableAlta"));

                if (cell2 != null)
                {
                    //Obtenemos el numero de la columna
                    var columnaNumero = cell2.WorksheetColumn().ColumnNumber();

                    if (i > 1)
                    {
                        //Eliminando las dos primeras columnas 
                        //workbook.Worksheet(i).Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(180, 198, 231);
                        workbook.Worksheet(i).Range(1, 1, 1, columnaNumero).Style.Fill.SetBackgroundColor(XLColor.FromArgb(180, 198, 231));
                        workbook.Worksheet(i).Column(1).Delete();
                        workbook.Worksheet(i).Column(1).Delete();
                        workbook.Worksheet(i).Columns().AdjustToContents();
                    }


                    workbook.Worksheet(i).Row(1).Style.Alignment.WrapText = true;
                    workbook.Worksheet(i).Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; //Alineamos horizontalmente
                    workbook.Worksheet(i).Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                    workbook.Worksheet(i).Row(1).Style.Font.Bold = true;

                    /*
                    workbook.Worksheet(i).Row(1).Style.Alignment.WrapText = true;
                    workbook.Worksheet(i).Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; //Alineamos horizontalmente
                    workbook.Worksheet(i).Row(1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                    workbook.Worksheet(i).Row(1).Style.Font.Bold = true;
                    */

                    //Iteramos para cada columna con celdas

                    for (int k = 1; k <= columnaNumero; k++)
                    {
                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("HOSPITALIZACION")) 
                        {
                            
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            //Obtenemos el rango de filas 
                            var NumFilas = Rango1.RowCount();

                            //Con el rango de filas sacamos el rango para pintar la columna
                            //workbook.Worksheet(i).Range(2,k,NumFilas,k ).Style.Fill.SetBackgroundColor(XLColor.FromArgb(239, 251, 241));

                            workbook.Worksheet(i).Column(k).CellsUsed();
                            workbook.Worksheet(i).Column(k).Width = 20;
                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("UCI "))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            //workbook.Worksheet(i).Range(2, k, NumFilas , k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(239, 251, 241));

                            workbook.Worksheet(i).Column(k).CellsUsed();
                            workbook.Worksheet(i).Column(k).Width = 20;
                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains(" UNIDAD DE CUIDADOS INTERMEDIOS NEONATAL"))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            //workbook.Worksheet(i).Range(2, k, NumFilas, k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(239, 251, 241));

                            workbook.Worksheet(i).Column(k).CellsUsed();
                            workbook.Worksheet(i).Column(k).Width = 20;
                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("Dx"))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            workbook.Worksheet(i).Column(k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left; //Alineamos horizontalmente
                            workbook.Worksheet(i).Column(k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                            //workbook.Worksheet(i).Range(2, k, NumFilas , k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 252, 243));

                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("FechaHoraEvolucion"))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            //workbook.Worksheet(i).Range(2, k, NumFilas , k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(242, 242, 242));

                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("PreAlta_Alta"))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            //workbook.Worksheet(i).Range(2, k, NumFilas , k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(242, 242, 242));

                        }

                        if (workbook.Worksheet(i).Column(k).FirstCell().Value.ToString().Contains("Dificultades en la evolucion"))
                        {
                            var Rango1 = workbook.Worksheet(i).RangeUsed();
                            var NumFilas = Rango1.RowCount();
                            //workbook.Worksheet(i).Range(2, k, NumFilas , k).Style.Fill.SetBackgroundColor(XLColor.FromArgb(242, 242, 242));

                        }

                    }
                }
                
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("HOSPITALIZACION").Fill.SetBackgroundColor(XLColor.FromArgb(198, 239, 206));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("UCI ").Fill.SetBackgroundColor(XLColor.FromArgb(198, 239, 206));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("Dx").Fill.SetBackgroundColor(XLColor.FromArgb(255, 242, 204));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains(" UNIDAD DE CUIDADOS INTERMEDIOS NEONATAL").Fill.SetBackgroundColor(XLColor.FromArgb(198, 239, 206));
                workbook.Worksheet(i).Row(1).AddConditionalFormat().WhenContains("Cama").Fill.SetBackgroundColor(XLColor.FromArgb(191, 191, 191));
                //workbook.Worksheet(i).Columns().Width = 25;

                if (i == 1)
                {
                    workbook.Worksheet(i).Tables.FirstOrDefault().Theme = XLTableTheme.None;
                    workbook.Worksheet(i).Range(1,1,1,2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 192, 0));
                    workbook.Worksheet(i).Range(1, 1, 1, 2).Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 192, 0));
                    workbook.Worksheet(i).Column(1).AdjustToContents();
                    workbook.Worksheet(i).Column(2).AdjustToContents();
                }

                
                //colorear columna 
                //workbook.Worksheet(i).Column("A").Style.Fill.BackgroundColor = XLColor.Red;


                //workbook.Worksheet(i).Columns().AdjustToContents();

                var Rango = workbook.Worksheet(i).RangeUsed();
                var NumColumnas = Rango.ColumnCount();
                
            }


            workbook.SaveAs(destination);
            workbook.Dispose();
        }


        public DataTable ReadExcelSheet(string fname, bool firstRowIsHeader = true)
        {
            List<string> Headers = new List<string>();
            DataTable dt = new DataTable();
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fname, false))
            {
                //Read the first Sheets 
                Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                int counter = 0;
                foreach (Row row in rows)
                {
                    counter = counter + 1;
                    //Read the first row as header
                    if (counter == 1)
                    {
                        var j = 1;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            var colunmName = firstRowIsHeader ? GetCellValue(doc, cell) : "Field" + j++;
                            Console.WriteLine(colunmName);
                            Headers.Add(colunmName);
                            dt.Columns.Add(colunmName);
                        }
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = GetCellValue(doc, cell);
                            i++;
                        }
                    }
                }

            }
            return dt;
        }

        public void CreateExcelFile(DataTable table, string destination)
        {
            var ds = new DataSet();
            ds.Tables.Add(table);
            ExportDSToExcel(ds, destination);
        }

        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        public void ExportDSToExcel(DataSet ds, string destination)
        {
            
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 1;

                foreach (DataTable table in ds.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);

                    }

                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                }
            }
        }
    }
}