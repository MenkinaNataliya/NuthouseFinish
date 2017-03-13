using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Net.Mail;
using DataBaseService;

namespace Server
{
    class Program
    {
        //В планировщике задач windows настроить ежемесячную задачу которая запускает эту программу раз в месяц
        static void Main()
        {
            SendingStoriesInThePastMonth();
        }

        public static void SendingStoriesInThePastMonth()
        {
            var messageBody = MessageBodyFormation(Get.History());
            var text = File.ReadAllLines("connect.txt");
            var login = text[0];
            var password = text[1];
            SendMail("smtp.gmail.com", login, password, login, "Передвижение оборудования за месяц", messageBody);
        }

        public static string MessageBodyFormation(List<ChangeHistory> history )
        {
            var text = "Отчет по передвижению техники за "+ DateTime.Now+"\n";
            return history.Aggregate(text, (current, item) => current + ("Устройство " + item.InventNumber + 
                                    " перешло из состояния " + item.OldStatus.Naming.ToUpper() + " в состояние " + item.NewStatus.Naming.ToUpper() + "\n"));
        }
     
        public static void SendMail(string smtpServer, string from, string password,
            string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage {From = new MailAddress(@from)};
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient
                {
                    Host = smtpServer,
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(@from.Split('@')[0], password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

 
    }
}
