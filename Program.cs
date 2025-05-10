using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeteccionPersonasWindowsForms
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Ruta donde se descargaron las DLLs de OpenCV
            string opencvPath = @"C:\Users\yasmi\.nuget\packages\emgu.cv.runtime.windows\4.5.5\runtimes\win-x64\native";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + opencvPath);

           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
