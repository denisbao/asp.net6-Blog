using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
   public bool Send(
     string toName,
     string toEmail,
     string subject,
     string body,
     string fromName = "Email tester",
     string fromEmail = "email@tester.com "
   )
   {
     // DEFININDO OS DADOS DO HOST:
     var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

     // DEFININDO AS CREDENDIAIS DE ENVIO (EMAIL E SENHA) E MÉTODO DE ENVIO:
     smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
     smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
     smtpClient.EnableSsl = true;

     // CRIANDO O EMAIL:
     var mail = new MailMessage();

     mail.From = new MailAddress(fromEmail, fromName);
     mail.To.Add(new MailAddress(toEmail, toName));
     mail.Subject = subject;
     mail.Body = body;
     mail.IsBodyHtml = true;

     // FAZENDO O ENVIO DO EMAIL:
     try 
     {
       smtpClient.Send(mail);
       return true;
     }
     catch (Exception ex)
     {
       return false;
     }

   }
}