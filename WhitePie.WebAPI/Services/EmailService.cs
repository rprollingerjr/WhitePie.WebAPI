using System.Net;
using System.Net.Mail;
using WhitePie.WebAPI.DTOs;

namespace WhitePie.WebAPI.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendContactEmailAsync(ContactRequestDto contact)
    {
        var smtpHost = _config["SMTP_HOST"];
        var smtpPort = int.Parse(_config["SMTP_PORT"]);
        var smtpUser = _config["SMTP_USER"];
        var smtpPass = _config["SMTP_PASS"];
        var toEmail = "management@ediblemami.com";

        var body = $@"
            <p><strong>From:</strong> {contact.Name} ({contact.Email})</p>
            <p><strong>Message:</strong></p>
            <p>{contact.Message}</p>";

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpUser, smtpPass)
        };

        var mail = new MailMessage
        {
            From = new MailAddress(smtpUser, "EdibleMami Contact Form"),
            Subject = "New Contact Request",
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(toEmail);
        await client.SendMailAsync(mail);
    }
}

public class EmailRequest
{
    public string To { get; set; }
    public string From { get; set; } = "your@email.com";
    public string Subject { get; set; }
    public string Html { get; set; }
}
