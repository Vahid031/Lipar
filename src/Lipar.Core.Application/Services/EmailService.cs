using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Services;

public class EmailService : IEmailService
{
    public MailOptions _mailOptions { get; }

    public EmailService(LiparOptions liparOptions)
    {
        _mailOptions = liparOptions.Mail;
    }

    public async Task SendAsync(EmailRequest request)
    {

        var email = new MimeMessage();
        email.Sender = new MailboxAddress(_mailOptions.DisplayName, request.From ?? _mailOptions.EmailFrom);
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = request.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailOptions.SmtpHost, _mailOptions.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailOptions.SmtpUser, _mailOptions.SmtpPass);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}


