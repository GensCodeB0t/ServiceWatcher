using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Management;
using System.Windows.Forms;

namespace ServiceMonitor
{
    public partial class GDSincProcessMonitor : ServiceBase
    {
        private ManagementEventWatcher processWatch;
        public GDSincProcessMonitor()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            processWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            processWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            processWatch.Start();                        
        }

        protected override void OnStop()
        {
            processWatch.Stop();
        }
        public static void ProcessClosed(Object sender, EventArgs e, Process p)
        {
            MessageBox.Show($"The {p.ProcessName} process has closed");
            Email.SendMail(p.ProcessName);
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Process[] p = Process.GetProcessesByName("NOTEPAD++");
            Array.ForEach<Process>(p.ToArray<Process>(), x => {

                Console.WriteLine(x.ProcessName);
                x.EnableRaisingEvents = true;
                x.Exited += delegate (Object processSender, EventArgs processEvent) { ProcessClosed(processSender, processEvent, x); };
            });
        }
    }
}
