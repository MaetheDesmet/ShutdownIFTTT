using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;

namespace ShutdownIFTTT
{
    public partial class ShutdownIFTTT : ServiceBase
    {
        private string filePath = "";
        private Timer timer = new Timer();

        public ShutdownIFTTT()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.Sleep(60000);

            //fill in path in App.config before deploying
            filePath = ConfigurationSettings.AppSettings.Get("filePath");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000;  
            timer.Enabled = true;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                var psi = new ProcessStartInfo("shutdown", "/s /t 0")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
