using System.Net;
using System.Net.Mail;
using LOMs.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace LOMs.Infrastructure.Services.EmailSender;

public class SmtpEmailSender(IOptions<SmtpSettings> options) : IEmailSender
{
    private readonly SmtpSettings _settings = options.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port);
        client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        client.EnableSsl = _settings.EnableSsl;

        var mail = new MailMessage(_settings.From, to, subject, body);
        await client.SendMailAsync(mail);
    }
}