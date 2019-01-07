using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace ShutdownIFTTT
{
    public partial class Service1 : ServiceBase
    {
        private string filePath = "C:\\Users\\desmetma\\Documents\\junk\\test.txt";
        private Timer timer = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.Sleep(60000);

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
