namespace MedUA.Service
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailService : IIdentityMessageService
    {
        private SendGridClient client;

        public EmailService()
        {
            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            client = new SendGridClient(apiKey);
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(), new EmailAddress(message.Destination), message.Subject, message.Body, message.Body);
            await client.SendEmailAsync(msg);
        }
    }
}
