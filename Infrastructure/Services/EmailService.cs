using Infrastructure.Services.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task<List<MimeMessage>> GetTicketMessagesFromTicketAccount()
        {
            using (var client = new ImapClient(new ProtocolLogger("imap.log")))
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate("a.biljettportalen@gmail.com", "bqwa avyb vjry ruwe");

                client.Inbox.Open(FolderAccess.ReadOnly);

                var uids = client.Inbox.Search(SearchQuery.All);
                var ticketMessages = new List<MimeMessage>();

                foreach (var uid in uids)
                {
                    var message = await client.Inbox.GetMessageAsync(uid);

                    if (message.From.Mailboxes.FirstOrDefault().Address == "aikfotboll@ebiljett.nu")
                    {
                        var textPart = message.Body as TextPart;
                        textPart.Text = CreateNewTicketMessageBody(textPart.Text);
                        message.Body = textPart;
                        message.Subject = "Biljettöverlåtelse från årskortsinnehavare via Allmänna Biljett Portalen";

                        ticketMessages.Add(message);
                    }

                }

                client.Disconnect(true);

                return ticketMessages;
            }
        }

        public string CreateNewTicketMessageBody(string originalText)
        {

            string pattern = @"<p>.*?</p>";
            Regex rgx = new Regex(pattern);

            string newText = rgx.Replace(originalText,"<p>Här är din efterskänkta årskortsbiljett via Allmänna Biljett Portalen.</p>", 1);

            return newText;
        }




    }
}
