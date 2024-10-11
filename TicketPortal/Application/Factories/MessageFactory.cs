using Application.DTOs;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public static class MessageFactory
    {
        public static MessageDto CreateMessage(MimeMessage message)
        {
            return new MessageDto()
            {
                MessageId = message.MessageId,
                Subject = message.Subject,
                Body = message.Body.ToString(),
            };
        }



    }
}
