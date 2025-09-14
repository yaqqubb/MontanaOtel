﻿using MailKit.Net.Smtp; // MailKit SmtpClient
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Mailing.MailKitImplementations;

public class MailKitMailService : IMailService
{
    private readonly MailSettings? _mailSettings;

    public MailKitMailService(IConfiguration configuration)
    {
        _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
    }

    public void SendMail(Mail mail)
    {
        if (mail == null) throw new ArgumentNullException("Email not defined.");

        if (_mailSettings == null) throw new ArgumentNullException("Mail setting not defined.");

        MimeMessage email = new();

        email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));

        email.To.Add(new MailboxAddress(mail.ToFullName, mail.ToEmail));

        email.Subject = mail.Subject;

        BodyBuilder bodyBuilder = new()
        {
            TextBody = mail.TextBody,
            HtmlBody = mail.HtmlBody
        };

        if (mail.Attachments != null)
            foreach (MimeEntity? attachment in mail.Attachments)
                bodyBuilder.Attachments.Add(attachment);

        email.Body = bodyBuilder.ToMessageBody();

        // Sadece MailKit.Net.Smtp.SmtpClient kullanıyoruz
        using MailKit.Net.Smtp.SmtpClient smtp = new();
        smtp.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}