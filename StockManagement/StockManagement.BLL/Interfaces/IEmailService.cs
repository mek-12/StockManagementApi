﻿namespace StockManagement.API.Core.Interfaces {
    public interface IEmailService {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
