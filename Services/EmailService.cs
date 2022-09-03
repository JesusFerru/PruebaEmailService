using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using SimpleEmailApp.Data;
using MailKit.Security;
using System;
using Microsoft.EntityFrameworkCore;
using SimpleEmailApp.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;

/// <summary>
/// Se vuelve privada la configuracion del usuario de destino para el Email Service
///Nadie tendrá acceso a la información privada en el appsettings.json
/// </summary>

namespace SimpleEmailApp.Services;




public class EmailService : IEmailService
{
    private readonly AppDbContext _db;
   /* public EmailService(AppDbContext db)
    {
        _db = db;
    }*/
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db = db;
    }

    public async Task<List<DataEmail>> GetEmails()
    {
        return await _db.DataEmails.ToListAsync();
    }


    public async  Task<DataEmail> SendEmail(DataEmail request)
//public void SendEmail(DataEmail request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smtp = new SmtpClient();
        smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
        smtp.AuthenticationMechanisms.Remove("XOAUTH2");
        smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
        
        smtp.Send(email);
        smtp.Disconnect(true);

        await _db.AddAsync(request);
        await _db.SaveChangesAsync();
        return request;
    }

    public async Task<DataEmail> UpdateEmailContent(int id, DataEmail request)
    {
        var EmailSaved = _db.DataEmails.Find(id);
        if (EmailSaved != null)
        {
            EmailSaved.To = request.To;
            EmailSaved.Subject = request.Subject;
            EmailSaved.Body = request.Body;


            _db.Update(EmailSaved);
            await _db.SaveChangesAsync();

            return EmailSaved;
        }
        else return null;
    }

    public async Task<DataEmail> DeleteEmail(int id)
    {
        var EmailSaved = _db.DataEmails.Find(id);
        if (EmailSaved != null)
        {
            _db.Remove(EmailSaved);
            await _db.SaveChangesAsync();
            return EmailSaved;
        }
        else return null;
    }

}



