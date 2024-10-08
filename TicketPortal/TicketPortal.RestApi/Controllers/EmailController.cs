using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;


namespace TicketPortal.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetMessages()
        {
            using (var client = new ImapClient(new ProtocolLogger("imap.log")))
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate("a.biljettportalen@gmail.com", "bqwa avyb vjry ruwe");

                client.Inbox.Open(FolderAccess.ReadOnly);

                var uids = client.Inbox.Search(SearchQuery.All);

                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                }

                client.Disconnect(true);

                return Ok();
            }
        }



    }
}
