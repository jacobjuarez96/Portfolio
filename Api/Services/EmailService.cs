using BlazorApp.Shared;
using BlazorApp.Shared.Interfaces;
using BlazorApp.Shared.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;

namespace Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = Environment.GetEnvironmentVariable("SMTP_Server");
        private readonly int _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_Port"));
        private readonly string _smtpUsername = Environment.GetEnvironmentVariable("SMTP_User");
        private readonly string _smtpPassword = Environment.GetEnvironmentVariable("SMTP_Pass");


        public async Task<bool> SendEmail(ContactFormModel model)
        {
            try
            {
                // Create the email message
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(model.Name, model.Email));
                emailMessage.To.Add(new MailboxAddress("Your Name or Company", "jacobjuarez1996@gmail.com"));
                emailMessage.Subject = model.Subject;

                // Set the body of the email (you can use HTML here)
                emailMessage.Body = new TextPart("plain") { Text = model.Message };

                // Connect to the SMTP server and send the email
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, true); // Don't use SSL (use SSL on port 465)
                    //await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return true; // Return true if email is sent successfully
            }
            catch (Exception ex)
            {
                // Log or handle the error, and return false
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}