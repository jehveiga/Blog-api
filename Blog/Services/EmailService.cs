using System.Net;
using System.Net.Mail;

namespace Blog.Services;
public class EmailService
{
    /// <summary>
    /// Tem que ter um serviço cadastrado em alguma empresa que disponha do serviço SMTP
    /// tipo: Sengrid, SendinBlue, SendPulse, etc...
    /// </summary>
    /// <param name="toName"></param>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="fromName"></param>
    /// <param name="fromEmail"></param>
    /// <returns></returns>
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Equipe pog.io",
        string fromEmail = "email@balta.io")
    {
        // Configuração de serviço de envio de email por SMTP
        var smtpCliente = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

        // Configurando as credenciais de rede
        smtpCliente.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);

        // Configuraçãod do método de entrega
        smtpCliente.DeliveryMethod = SmtpDeliveryMethod.Network;

        // Habilitando para uso de porta segura
        smtpCliente.EnableSsl = true;

        // Montando a mensagem que será enviada por e-mail
        var mail = new MailMessage();

        // Configuração do remetente e nome
        mail.From = new MailAddress(fromEmail, fromName);

        // Configuração do destinatário e nome ('To' é uma lista e pode ser enviado para mais de uma pessoa)
        mail.To.Add(new MailAddress(toEmail, toName));

        // Configuração do assunto do email
        mail.Subject = subject;

        // Configuração do corpo do email
        mail.Body = body;

        // Configuração se pode obter html no corpo do email
        mail.IsBodyHtml = true;

        // Tentará enviar o email
        try
        {
            smtpCliente.Send(mail);
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }
}

