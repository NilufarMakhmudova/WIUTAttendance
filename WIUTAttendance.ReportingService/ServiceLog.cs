using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WIUTAttendance.ReportingService
{
   public class ServiceLog
    {
        /// <summary>  
        /// This function write log to LogFile.text when some error occurs.  
        /// </summary>  
        /// <param name="ex"></param>  
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        /// <summary>  
        /// this function write Message to log file.  
        /// </summary>  
        /// <param name="Message"></param>  
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteProgressLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ProgressFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        #region Send Email Code Function
        /// <summary>  
        /// Send Email with cc bcc with given subject and message.  
        /// </summary>  
        /// <param name="ToEmail"></param>  
        /// <param name="cc"></param>  
        /// <param name="bcc"></param>  
        /// <param name="Subj"></param>  
        /// <param name="Message"></param>  
        public static void SendEmail(String ToEmail, string cc, string bcc, String Subj, string Message)
        {
            //Reading sender Email credential from web.config file  

            string HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            int PortAdd = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
            string FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();

            //creating the object of MailMessage  
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
            ServiceLog.WriteProgressLog("Here is the from address: " + mailMessage.From.ToString());
            mailMessage.Subject = Subj; //Subject of Email  
            mailMessage.Body = Message; //body or message of Email  
            mailMessage.IsBodyHtml = true;

            string[] ToMuliId = ToEmail.Split(',');
            foreach (string ToEMailId in ToMuliId)
            {
                mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
            }


            string[] CCId = cc.Split(',');

            foreach (string CCEmail in CCId)
            {
                mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
            }

            string[] bccid = bcc.Split(',');

            foreach (string bccEmailId in bccid)
            {
                mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
            }
            SmtpClient smtp = new SmtpClient(HostAdd, PortAdd);  // creating object of smptpclient  
          
            ServiceLog.WriteProgressLog("Here is the host and port: " + smtp.Host.ToString() + smtp.Port.ToString());
            //network and security related credentials  

      
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = "00002059";
            NetworkCred.Password = Pass;
            smtp.EnableSsl = true;

            ServiceLog.WriteProgressLog("Here is the email and password: " + NetworkCred.UserName.ToString() + NetworkCred.Password.ToString());
            
            
            smtp.Credentials = NetworkCred;
            ServiceLog.WriteProgressLog("Credentials set, trying to sent the message");
           
            try
            {
                smtp.Send(mailMessage); //sending Email
                ServiceLog.WriteProgressLog("Message sent");
           
            }
            catch (Exception ex)
            {

                ServiceLog.WriteProgressLog(ex.Message);
            }  
        }

        #endregion  
    }
}
