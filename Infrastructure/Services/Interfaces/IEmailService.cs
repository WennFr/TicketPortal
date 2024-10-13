using Application.DTOs;
using Application.Enums;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task<List<MimeMessage>> GetTicketMessagesFromTicketAccount();
        StatusMessage SendTicketMessage(MessageDto message, string toEmailAddress);
    }
}
