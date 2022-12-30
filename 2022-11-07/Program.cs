using Proyecto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace PruebaExcelEmail
{
    class Program
    {
        static void Main(string[] args)
        {

            string datetime = DateTime.Now.ToString("yyyyMMdd");
            try
            {

                string ExcelFileName = "PruebaExcel";
                string FolderPath = "C:\\Apuntes\\";
                ExcelFileName = ExcelFileName + "_" + datetime+".XLSX";

                //string appPath = Environment.CurrentDirectory;
                //string ExcelFileName2 = ExcelFileName + ".XLSX";
                //string fullpath = Path.Combine(appPath, ExcelFileName2);


                string ExcelPath = Path.Combine(FolderPath, ExcelFileName);
                string StoredProcedureName = "dbo.USP_prueba_03112022";
                string SheetName = "1eraHoja";
                /***********************************************************************************************************/
                //declarando array con las hojas de excel
                string[] SheetsArray = { "HOSP_CARDIO",
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
                                         "UCI_CUI_INT",
                                        };
                
                /***********************************************************************************************************/
                

                OleDbConnection Excel_OLE_Con = new OleDbConnection();
                OleDbCommand Excel_OLE_Cmd = new OleDbCommand();

                //Construct ConnectionString for Excel
                string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + FolderPath + ExcelFileName
                    + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";

                string connstring2 = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
                //drop Excel file if exists
                File.Delete(FolderPath + ExcelFileName + ".xlsx");
                File.Delete(ExcelPath);

                //USE ADO.NET Connection from SSIS Package to get data from store procedure
                SqlConnection myADONETConnection = new SqlConnection(connstring2);
                //myADONETConnection = (SqlConnection)(Dts.Connections["DB_Conn_10.5.0.23.SIGH.eramoss"].AcquireConnection(Dts.Transaction) as SqlConnection);

                //Load Data into DataTable from SQL ServerTable
                string queryString = "EXEC " + StoredProcedureName;
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, myADONETConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                //Use OLE DB Connection and Create Excel Sheet
                Excel_OLE_Con.ConnectionString = connstring;
                Excel_OLE_Con.Open();
                Excel_OLE_Cmd.Connection = Excel_OLE_Con;

                //Get Header Columns
                string TableColumns = "";
                List<string> ListTableColumn = new List<string>();

                // Get the Column List from Data Table so can create Excel Sheet with Header

                int z = 0;
                foreach (System.Data.DataTable table in ds.Tables)
                {
                    TableColumns = "";

                    foreach (DataColumn column in table.Columns)
                    {
                        TableColumns += column + "],[";                        
                    }
                    
                    TableColumns = ("[" + TableColumns.Replace(",", " Text,").TrimEnd(','));
                    TableColumns = TableColumns.Remove(TableColumns.Length - 2);
                    
                    ListTableColumn.Add(TableColumns);

                    Excel_OLE_Cmd.CommandText = "Create table " + SheetsArray[z] + " (" + ListTableColumn[z] + ")";
                    Excel_OLE_Cmd.ExecuteNonQuery();

                    z = z +1;
                }



                int y = 0;
                /***********************************************************************************************************/
                //Write Data to Excel Sheet from DataTable dynamically
                foreach (System.Data.DataTable table in ds.Tables)
                {
                    String sqlCommandInsert = "";
                    String sqlCommandValue = "";
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        sqlCommandValue += dataColumn + "],[";
                    }

                    sqlCommandValue = "[" + sqlCommandValue.TrimEnd(',');
                    sqlCommandValue = sqlCommandValue.Remove(sqlCommandValue.Length - 2);

                    
                    sqlCommandInsert = "INSERT into " + SheetsArray[y] + "(" + sqlCommandValue + ") VALUES(";
                    y = y +1;
                    
                    int columnCount = table.Columns.Count;
                    foreach (DataRow row in table.Rows)
                    {
                        string columnvalues = "";
                        for (int i = 0; i < columnCount; i++)
                        {
                            int index = table.Rows.IndexOf(row);
                            columnvalues += "'" + table.Rows[index].ItemArray[i] + "',";

                        }
                        columnvalues = columnvalues.TrimEnd(',');
                        var command = sqlCommandInsert + columnvalues + ")";
                        Excel_OLE_Cmd.CommandText = command;
                        Excel_OLE_Cmd.ExecuteNonQuery();
                    }

                }             

                Excel_OLE_Con.Close();

                               
                Email clemail = new Email();
                clemail.enviarEmail("envio de prueba", FolderPath, ExcelFileName, "eramoss@insnsb.gob.pe");
                //Application excel = new Application();
                //Workbook wb = excel.Workbooks.Open(FolderPath + ExcelFileName + ".xlsx");

            }

            

            catch (Exception exception)
            {
                using (StreamWriter sw = File.CreateText("C:\\Apuntes\\PruebaExcel_" + datetime + ".log"))
                {
                    sw.WriteLine(exception.ToString());


                }

            }



        }
    }
}
