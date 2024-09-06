using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using StockManagement.CCC.Entities;
using StockManagement.BLL.Interfaces;

namespace StockManagement.API.Core.Services
{
    public class EmailService : IEmailService {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings) {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message) {
            // TO DO Can be added later
            return;
        }
    }
}
