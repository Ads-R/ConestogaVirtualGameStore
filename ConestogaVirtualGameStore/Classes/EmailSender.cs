using System.Net.Mail;
using System;
using Microsoft.Extensions.Configuration;

namespace ConestogaVirtualGameStore.Classes
{
    public class EmailSender
    {
        string email;
        string emailPassword;
        public EmailSender(IConfiguration configuration)
        {
            email = configuration["OwnerEmail"];
            emailPassword = configuration["EmailPassword"];
        }
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your account";
            mailMessage.IsBodyHtml = true;
            string bodyText = "<h2>Please click the link to confirm your email</h2><br><br>";
            mailMessage.Body = bodyText + confirmationLink;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(email, emailPassword);
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error in EmailSender " + ex.Message);
            }
            return false;
        }

        public bool SendEmailPassword(string userEmail, string password)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Password has been reset";
            mailMessage.IsBodyHtml = true;
            string bodyText = "<h2>Your password has been reset. Below is your new password</h2><br><br>";
            mailMessage.Body = bodyText + password;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(email, emailPassword);
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error in EmailSender " + ex.Message);
            }
            return false;
        }
    }
}
