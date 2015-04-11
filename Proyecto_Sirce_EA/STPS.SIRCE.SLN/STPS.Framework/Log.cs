using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STPS.Framework
{
    public static class Log
    {
        public static void SetLog(Exception ex, string url = "")
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "Sistema SIRCE";
            sLog = "Application";
            sEvent = "Exception name: " + ex.Message
                + Environment.NewLine
                + "URL: " + url
                + Environment.NewLine
                + "Exception detail: ";



            var logError = sSource + "\n" + sLog + "\n" + sEvent;


            LogErrores.WriteError(ex, logError);

        }

        public static void SetLogSirce(Exception ex, string url = "")
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "Sistema SIRCE";
            sLog = "Application";
            sEvent = "Exception name: " + ex.Message
                + Environment.NewLine
                + "URL: " + url
                + Environment.NewLine
                + "Exception detail: ";

            var logError = sSource + "\n" + sLog + "\n" + sEvent;

            LogErrores.WriteError(ex, logError);

        }

    }
}
