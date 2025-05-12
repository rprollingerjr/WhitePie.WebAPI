using Resend;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace WhitePie.WebAPI.Services;

public class ResendEmailService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public ResendEmailService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config["Resend:ApiKey"]);
    }

    public async Task<bool> SendEmailAsync(string subject, string html)
    {
        IResend resend = ResendClient.Create(_config["Resend:ApiKey"]);

        var resp = await resend.EmailSendAsync(new EmailMessage()
        {
            From = "EdibleMami <management@ediblemami.com>",
            To = _config["Resend:SendToEmail"],
            Subject = subject,
            HtmlBody = html,
        });
        Console.WriteLine("Email Id={0}", resp.Content);

        return resp.Success;
    }
}


