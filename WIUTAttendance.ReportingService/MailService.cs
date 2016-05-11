using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WIUTAttendance.ReportingServices;


namespace WIUTAttendance.ReportingService
{
    public partial class MailService : ServiceBase
    {
        private Timer timer1;
        private int getCallType = Convert.ToInt32(ConfigurationManager.AppSettings["CallType"]);
        public MailService()
        {
            InitializeComponent();
            int strTime = Convert.ToInt32(ConfigurationManager.AppSettings["callDuration"]);

            if (getCallType == 1)
            {
                timer1 = new System.Timers.Timer();
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
            else
            {
                timer1 = new System.Timers.Timer();
                timer1.Interval = strTime * 1000;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
        }

        protected override void OnStart(string[] args)
        {
            timer1.AutoReset = true;
            timer1.Enabled = true;
            ServiceLog.WriteErrorLog("Daily Reporting service started");
        }

        protected override void OnStop()
        {
            timer1.AutoReset = false;
            timer1.Enabled = false;
            ServiceLog.WriteErrorLog("Daily Reporting service stopped");
        }


        private double GetNextInterval()
        {
            String timeString = ConfigurationManager.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();

            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                //Here you can increase the timer interval based on your requirments.   
                ts = t.AddDays(30) - System.DateTime.Now;
            }
            //return ts.TotalMilliseconds;
            return 16000;
        }
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string message = "Hi ! This is Absenteeism report.";//whatever msg u want to send write here.  
                var students = Helper.GetStudentsWithAttendanceOffence().ToString();
                string Msg = message + students;
                ServiceLog.WriteProgressLog("Here is the formed message: " + Msg);

                // Here you can write the   
                ServiceLog.SendEmail("mahmudovanilufar@gmail.com", "nmakhmudova@students.wiut.uz", "00002059@mail.ru", "Monthly Report of Absenteeism on " + DateTime.Now.ToString("dd-MMM-yyyy"), Msg);

                if (getCallType == 1)
                {
                    timer1.Stop();
                    System.Threading.Thread.Sleep(1000000);
                    SetTimer();
                }
            }

            catch (Exception)
            {

                throw;
            }
        }
    }
}
