using System;
using System.Windows.Forms;
using Serilog;

namespace PuzzleExtractor.Forms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var logpath = System.Configuration.ConfigurationSettings.AppSettings["logPath"];
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logpath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
