using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Infrastructure.Services.Interfaces;
using Application.Factories;
using Application.Enums;


namespace TicketPortal.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<List<MessageDto>>> GetAllTicketMessages()
        {
            var mimeMessages = await _emailService.GetTicketMessagesFromTicketAccount();

            var messages = mimeMessages.Select(x => MessageFactory.CreateMessage(x));

            return Ok(messages);
        }

        [HttpPost]
        [Route("Send")]
        public ActionResult PostTicketMessageToUser(MessageDto message, string toEmailAddress)
        {
            var status = _emailService.SendTicketMessage(message, toEmailAddress);

            if (status == StatusMessage.Success)
                return Ok(message);

            else
                return BadRequest(status);

        }


    }
}
