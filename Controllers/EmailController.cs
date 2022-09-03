//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//using MimeKit;
//using MimeKit.Text;

namespace SimpleEmailApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

       public EmailController(IEmailService emailService)
       {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmails()
        {
            var result = await _emailService.GetEmails();
            return Ok(result);
        }


        [HttpPost(Name =  "Post_Email")]
        public async Task<IActionResult> PostEmail(DataEmail request)
        {
            //  _emailService.SendEmail(request);
            var new_email = new DataEmail()
            {
                IdEmail = request.IdEmail,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body,
            };


            var result =  await _emailService.SendEmail(new_email);

            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update( int id,  DataEmail request)
        {
           
            var result = await _emailService.UpdateEmailContent(id, request);
            if (result != null)
            {
                return Ok(result);
            }
            else return NotFound();
      
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( int id)
        {
           
            var result = await _emailService.DeleteEmail(id);
            if (result != null)
            {
                return Ok(result);
            }
            else return NotFound();
            
        }


    }
}


