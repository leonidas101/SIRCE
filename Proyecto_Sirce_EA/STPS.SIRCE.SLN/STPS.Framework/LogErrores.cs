using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace STPS.Framework
{
    public class LogErrores
    {

        public static void Log(string logMessage, TextWriter w)
        {
            try
            {
                w.Write(logMessage);
            }catch(Exception){
            
            }
        }

        public void WriteError(string error) {
           
            try
            {
                string mydocpath = Environment.CurrentDirectory;// Application.StartupPath Environment.GetFolderPath(Environment.SpecialFolder.Windows);

                string path = string.Concat(ConfigurationManager.ConnectionStrings["rutalog"].ConnectionString, @"\LogSirce.txt");
                using (StreamWriter w = File.AppendText(path))
                {
                    Log(error, w);
                 
                }

              
            }catch(Exception ex){
            
            }finally{
              //log.Close();
            }
        }

        public static void WriteError(Exception ex)
        {
            string mydocpath = Environment.CurrentDirectory;// Application.StartupPath Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            string path = string.Concat(ConfigurationManager.ConnectionStrings["rutalog"].ConnectionString, @"\LogException.txt");
            try
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    String Data = FlattenException(ex);
                    w.WriteLine("----------------------------------------------------------------------------------------------------");
                    w.WriteLine(String.Empty);
                    w.Write(Data);
                }
            }
            catch { }
        }
        public static void WriteError(Exception ex, String info)
        {
            string mydocpath = Environment.CurrentDirectory;// Application.StartupPath Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            string path = string.Concat(ConfigurationManager.ConnectionStrings["rutalog"].ConnectionString, @"\LogSirce.txt");

            try
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    String Data = FlattenException(ex);
                    w.WriteLine("----------------------------------------------------------------------------------------------------");
                    w.WriteLine(DateTime.Now.ToString());
                    w.Write(info);
                    w.WriteLine(String.Empty);
                    w.Write(Data);
                }
            }
            catch
            {
            }
        }

        public static String FlattenException(Exception exception)
        {
            try
            {
                var stringBuilder = new StringBuilder();

                while (exception != null)
                {
                    stringBuilder.AppendLine(exception.Message);
                    stringBuilder.AppendLine(exception.StackTrace);

                    exception = exception.InnerException;
                }

                return stringBuilder.ToString();
            }catch(Exception ex){
                return string.Empty;
            }
        }
    }
}