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
using System.Net;
using Application.DTOs;
using Application.Enums;

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

            string newText = rgx.Replace(originalText, "<p>Här är din efterskänkta årskortsbiljett via Allmänna Biljett Portalen.</p>", 1);

            return newText;
        }

        public StatusMessage SendTicketMessage(MessageDto message, string toEmailAddress)
        {
            var fromEmailAddress = "a.biljettportalen@gmail.com";

            MailMessage mailMessage = new MailMessage(fromEmailAddress, toEmailAddress);
            mailMessage.From = new MailAddress(fromEmailAddress);
            mailMessage.To.Add(toEmailAddress);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmailAddress, "bqwa avyb vjry ruwe");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                return StatusMessage.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return StatusMessage.Error;

        }






    }
}
