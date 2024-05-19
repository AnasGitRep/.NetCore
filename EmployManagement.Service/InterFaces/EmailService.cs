

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EmployManagementDataBase.Dto.Base;

namespace EmployManagement.Service.InterFaces
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ResponseModel<string>> SendMailAsync(string recipientEmail, string recipientName, string subject,string body)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                // Read email template
                string emailTemplate = File.ReadAllText("E:\\Interview\\VisualStudio\\EmployManagement\\EmployManagement.Service\\Html\\EmailBody.html");

                // Replace placeholders with actual values
                emailTemplate = emailTemplate.Replace("{recipientName}", recipientName);
                emailTemplate = emailTemplate.Replace("{body}", body);

                string fromMail = "ksmohammedanas50@gmail.com";
                string fromPassword = "nvlbhhsvvfgwybez";

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = subject,
                    Body = emailTemplate,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(recipientEmail);

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                response.IsOk = true;
                response.Message = "Email sent successfully";
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Message = ex.Message;
            }

            return response;
        }

    

    }
    
}
