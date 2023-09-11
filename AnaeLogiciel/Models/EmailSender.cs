using System.Net;
using System.Net.Mail;
using AnaeLogiciel.Models.Interface;

namespace AnaeLogiciel.Models;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        Console.WriteLine("mitsofoka ato anatinle mandefa mail");
        var client = new SmtpClient("razafimahefasteven130102@gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("razafimahefasteven130102@gmail.com", "mypassword")
        };
 
        Console.WriteLine("mivoaka tao anatinle mandefa mail");
        
        return client.SendMailAsync(
            new MailMessage(from: "razafimahefasteven130102@gmail.com",
                to: email,
                subject,
                message
            ));
    }
}