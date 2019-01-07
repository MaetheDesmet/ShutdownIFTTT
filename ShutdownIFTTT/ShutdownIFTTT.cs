using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;

namespace ShutdownIFTTT
{
    public partial class ShutdownIFTTT : ServiceBase
    {
        private string filePath;
        private Timer timer = new Timer();

        public ShutdownIFTTT()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //fill in path in App.config before deploying
#pragma warning disable CS0618 // Type or member is obsolete
            filePath = ConfigurationSettings.AppSettings.Get("filePath");
            int sleep = int.Parse(ConfigurationSettings.AppSettings.Get("sleep")) * 1000;
            int interval = int.Parse(ConfigurationSettings.AppSettings.Get("interval")) * 1000;
#pragma warning restore CS0618 // Type or member is obsolete
            System.Threading.Thread.Sleep(sleep);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = interval;  
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
