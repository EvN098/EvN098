//clase program modificado 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace AlertaFarmacia
{
    class Program
    {
        static void Main(string[] args)
        {

            string datetime = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string ExcelFileName = "AlertaFarmacia" + datetime + ".XLSX";
            string FolderPath = @"C:\Reportes_AlertaFarmacia\";
            string ExcelPath = Path.Combine(FolderPath, ExcelFileName);

            string StoredProcedureName = "dbo.Usp_FarmaciaMovimientoMensual";
            string connstring2 = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

            string CuerpoMensaje = "Buen día," + "<br/>" + "Adjunto el Excel de Seguimiento de pacientes hospitalizados a la fecha: " + DateTime.Now.ToString("F") + "." + "<br/>";

            
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddSeconds(-1);
            
            try
            {
                // Si no existe el directorio, crearlo
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                //Eliminar excel si existe
                File.Delete(ExcelPath);

                //bd Conexion 
                SqlConnection myADONETConnection = new SqlConnection(connstring2);

                //Ejecutar sp y cargar data en el dataset
                string queryString = "EXEC " + StoredProcedureName+ " @FechaInicio='"+ oPrimerDiaDelMes.ToString("yyyy-MM-dd HH:mm:ss") +"', " + "@FechaFin='"+ oUltimoDiaDelMes.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                //string queryString = "EXEC " + StoredProcedureName + " @FechaInicio='" + "2022-01-01 00:00:00" + "', " + "@FechaFin='" + oUltimoDiaDelMes.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                SqlDataAdapter adapter = new SqlDataAdapter(queryString, myADONETConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                string[] SheetsArray = { "AlertaFarmacia"};
                //Exportando dataset para crear el excel 
                Helper help = new Helper();
                help.ExportDataSet(ds, ExcelPath);
                //help.ExportDataSet(ds, ExcelPath, SheetsArray);

                //Obteniendo emails del app.config
                string fromEmail = ConfigurationManager.AppSettings.Get("email_from").ToString();
                string ToEmail = ConfigurationManager.AppSettings.Get("email_to").ToString();
                /*string ToEmail2 = ConfigurationManager.AppSettings.Get("email_to2").ToString();               
                */
                string email_cc1 = ConfigurationManager.AppSettings.Get("email_cc1").ToString();
                string email_cc2 = ConfigurationManager.AppSettings.Get("email_cc2").ToString();

                //creando array de destinatarios y destinatarios con copia
                string[] Destinos = { ToEmail 
                                      //,ToEmail2
                                    };
                //string[] CopiaA = { email_cc1 , email_cc2  };
              
                //Email clemail = new Email();
                //clemail.enviarEmail(CuerpoMensaje, FolderPath, ExcelFileName, Destinos, CopiaA);
                //clemail.enviarEmail(CuerpoMensaje, FolderPath, ExcelFileName, Destinos);

                //Elimina el excel creado
                //File.Delete(ExcelPath);
            }

            catch (Exception exception)
            {                
                using (StreamWriter sw = File.CreateText(FolderPath + "ErrorAlertaFarmacia_" + datetime + ".log"))
                {
                    sw.WriteLine(exception.ToString());
                }
            }
        }
    }
}
