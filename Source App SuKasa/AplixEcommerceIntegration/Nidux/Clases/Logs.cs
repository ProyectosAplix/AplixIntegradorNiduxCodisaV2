using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplixEcommerceIntegration.Nidux.Clases
{
    class LogsFile
    {
        public void WriteLogs(string mensaje)
        {
            string basePath = ConfigurationSettings.AppSettings["BitacoraArticulos"].ToString() + "\\Logs";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string filepath = basePath + "\\AppLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";

            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensaje}");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensaje}");
                }
            }
        }
    }
}
