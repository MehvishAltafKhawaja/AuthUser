using System.Net;
using System.Net.Mail;

namespace UserAuth.helper
{
    public class SandMail
    {
        public bool SendMail(string to, string subject, string msg)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            message.From = new MailAddress("khawajamehvish13@gmail.com");
            message.To.Add(to);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = msg;

            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("khawajamehvish13@gmail.com", "avkg ivnk egim anai");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
