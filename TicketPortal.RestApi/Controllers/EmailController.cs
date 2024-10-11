using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Infrastructure.Services.Interfaces;
using Application.Factories;


namespace TicketPortal.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> GetAllMessages()
        {
            var mimeMessages = await _emailService.GetTicketMessagesFromTicketAccount();

            var messages = mimeMessages.Select(x => MessageFactory.CreateMessage(x));

            return Ok(messages);
        }


    }
}
