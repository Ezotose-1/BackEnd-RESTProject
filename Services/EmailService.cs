using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using BackEnd_RESTProject.Data;
using BackEnd_RESTProject.Models;
using BackEnd_RESTProject.Helpers;
using System;

namespace BackEnd_RESTProject.Services
{
    public interface IEmailService
    {
        Task<Response> SendEmailAsync(string FromEmail, List<string> emails, string subject, string message);
    }
    public class EmailService : IEmailService
    {
        public IConfiguration Configuration { get; }
        private Context _context;

        public EmailService(IConfiguration configuration,Context context)
        {
            _context = context;
            Configuration = configuration;
        }
        public async Task<Response> SendEmailAsync(string FromEmail, List<string> ToEmails, string subject, string message)
        {
            return await ExecuteEmail(Configuration["SendgridKey"], subject, message, ToEmails, FromEmail);
        }

        public async Task<Response> ExecuteEmail(string apiKey, string subject, string message, List<string> emails, string FromEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("juliencalisto@gmail.com", FromEmail),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            
            
            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Response response = await client.SendEmailAsync(msg);
            return response;
        }

    }
}