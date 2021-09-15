
using System.Net.Mail;


namespace ServiceRequestApplication.Helpers
{
    public class Email
    {
       
        public  void SendEmail(string subject, string message)
        {
            var mailFrom = "x@gmail.com";
            var mailTo = "x@gmail.com";
            string password = "password";


            MailMessage mailMessage = new MailMessage(mailFrom, mailTo, subject, message);
            mailMessage.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(mailFrom, password);

            client.Send(mailMessage);

            client.Dispose();
        }
     

    }
}
