//Program principal del envio del excel de hospitalizados
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

            string datetime = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string ExcelFileName = "HOSPITALIZADOS";
            string FolderPath = @"C:\Reportes_PacientesHospitalizados\";
            
            //string ExcelPath, StoredProcedureName, queryString;
            //string connstring2;
            try
            {
                
                // If directory does not exist, create it
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                ExcelFileName = ExcelFileName + "_" + datetime + ".XLSX";

                string ExcelPath = Path.Combine(FolderPath, ExcelFileName);
                string StoredProcedureName = "dbo.USP_TRAMA_SEGUIMIENTO_HOSPITALIZADOS_V1";

                string connstring2 = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
                
                //Eliminar excel si existe
                File.Delete(FolderPath + ExcelFileName + ".xlsx");
                File.Delete(ExcelPath);

                //bd Conexion 
                SqlConnection myADONETConnection = new SqlConnection(connstring2);
               
                //Ejecutar sp y cargar data en el dataset
                string queryString = "EXEC " + StoredProcedureName;
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, myADONETConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                //Exportando dataset para crear el excel 
                Helper help = new Helper();
                help.ExportDataSet(ds, ExcelPath);
                //help.ExportDSToExcel(ds, ExcelPath);

                string fromEmail = ConfigurationManager.AppSettings.Get("email_from").ToString();
                string ToEmail = ConfigurationManager.AppSettings.Get("email_to").ToString();
                string ToEmail2 = ConfigurationManager.AppSettings.Get("email_to2").ToString();
                string ToEmail3 = ConfigurationManager.AppSettings.Get("email_to3").ToString();
                string ToEmail4 = ConfigurationManager.AppSettings.Get("email_to4").ToString();
                string ToEmail5 = ConfigurationManager.AppSettings.Get("email_to5").ToString();
                string ToEmail6 = ConfigurationManager.AppSettings.Get("email_to6").ToString();
                string ToEmail7 = ConfigurationManager.AppSettings.Get("email_to7").ToString();
                string ToEmail8 = ConfigurationManager.AppSettings.Get("email_to8").ToString();
                string ToEmail9 = ConfigurationManager.AppSettings.Get("email_to9").ToString();
                string ToEmail10 = ConfigurationManager.AppSettings.Get("email_to10").ToString();
                string ToEmail11 = ConfigurationManager.AppSettings.Get("email_to11").ToString();
                string ToEmail12 = ConfigurationManager.AppSettings.Get("email_to12").ToString();
                string ToEmail13 = ConfigurationManager.AppSettings.Get("email_to13").ToString();
                string ToEmail14 = ConfigurationManager.AppSettings.Get("email_to14").ToString();
                string ToEmail15 = ConfigurationManager.AppSettings.Get("email_to15").ToString();
                string ToEmail16 = ConfigurationManager.AppSettings.Get("email_to16").ToString();
                string ToEmail17 = ConfigurationManager.AppSettings.Get("email_to17").ToString();
                string ToEmail18 = ConfigurationManager.AppSettings.Get("email_to18").ToString();
                string ToEmail19 = ConfigurationManager.AppSettings.Get("email_to19").ToString();
                string ToEmail20 = ConfigurationManager.AppSettings.Get("email_to20").ToString();
                string ToEmail21 = ConfigurationManager.AppSettings.Get("email_to21").ToString();
                string ToEmail22 = ConfigurationManager.AppSettings.Get("email_to22").ToString();
                string ToEmail23 = ConfigurationManager.AppSettings.Get("email_to23").ToString();
                string ToEmail24 = ConfigurationManager.AppSettings.Get("email_to24").ToString();
                string ToEmail25 = ConfigurationManager.AppSettings.Get("email_to25").ToString();
                string ToEmail26 = ConfigurationManager.AppSettings.Get("email_to26").ToString();
                string ToEmail27 = ConfigurationManager.AppSettings.Get("email_to27").ToString();
                string ToEmail28 = ConfigurationManager.AppSettings.Get("email_to28").ToString();
                string ToEmail29 = ConfigurationManager.AppSettings.Get("email_to29").ToString();
                


                //creando array de contactos
                string[] Destinos = { ToEmail , ToEmail2};
                
                /*
                string[] Destinos = {   ToEmail,
                                        //ToEmail2,
                                        ToEmail3,
                                        ToEmail4,
                                        ToEmail5,
                                        ToEmail6,
                                        ToEmail7,
                                        ToEmail8,
                                        ToEmail9,
                                        ToEmail10,
                                        ToEmail11,
                                        ToEmail12,
                                        ToEmail13,
                                        ToEmail14,
                                        ToEmail15,
                                        ToEmail16,
                                        ToEmail17,
                                        ToEmail18,
                                        ToEmail19,
                                        ToEmail20,
                                        ToEmail21,
                                        ToEmail22,
                                        ToEmail23,
                                        ToEmail24,
                                        ToEmail25,
                                        ToEmail26,
                                        ToEmail27,
                                        ToEmail28,
                                        ToEmail29
                                     };
                */
                Email clemail = new Email();
                clemail.enviarEmail("", FolderPath, ExcelFileName, Destinos );              
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



