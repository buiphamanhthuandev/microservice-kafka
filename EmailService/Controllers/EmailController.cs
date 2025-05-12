using EmailService.DTOs;
using EmailService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers
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
        [HttpPost("send-email-welcome")]
        public async Task<ActionResult> SendEmail([FromBody]EmailRequest emailRequest){
            await _emailService.SendEmailService(emailRequest.Email, emailRequest.UserName);
            return Ok();
        }
    }
}
