using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using bellyful_proj_v._0._3.Models;
using System.Linq;

namespace bellyful_proj_v._0._3
{
    public interface IMyEmailSender
    {
        Task SendEmailAsync(string orderId, string[] userEmails);
    }

    public class GoogleEmailSender : IMyEmailSender
    {
        readonly SmtpClient _sender;
        string displayName;

        public GoogleEmailSender()
        {
            _sender = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("bellyful.inv@gmail.com", "z543z609z"),
                EnableSsl = true,
            };
        }


        public Task SendEmailAsync(string orderId , string [] userEmails)
        {
            using (var message = new MailMessage())
            {  //收件人
                for (int i = 0; i < userEmails.Length; i++)
                {
                    displayName= userEmails[i].Split('@')[0];
                    message.To.Add(new MailAddress(userEmails[i], displayName));
                }
                //发件人
                message.From = new MailAddress("bellyful.inv@gmail.com", "Bellyful Invercargill");

                //抄送
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                //密件抄送
                //  message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));

                message.Subject = string.Format("New mission <OrderId:{0}> is comming", orderId);
                message.Body = string.Format("<h2>New Order delivery Mission <OrderId:{0}></h2><br />" +
                    "<h3>Please Login your<a href='http://d5614d32.ngrok.io/OrdersForVolunteer/PushedOrdersIndex'>Bellyful App</a> see the order details.</h3> " +
                    "<br /> <h3>Hopefully you will take this order </h3><br />" +
                    " <h4>Best Regards</h4><h4>Bellyful Invercargill</h4>", orderId);
                message.IsBodyHtml = true;
                //使用using，因为MailMessage实现了IDisposable接口。
                _sender.Send(message);

            }
            

            return Task.CompletedTask;
        }
    }
}