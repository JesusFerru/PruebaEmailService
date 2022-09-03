using System;

namespace SimpleEmailApp.Services
{
    public interface IEmailService
    {
        Task<List<DataEmail>> GetEmails();                 //Get
        Task<DataEmail> SendEmail(DataEmail request);      //Post
        Task<DataEmail> UpdateEmailContent(int id, DataEmail body);       //Put
        Task<DataEmail> DeleteEmail(int id);               //Delete
    }
}
