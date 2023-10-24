
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using HotelCollection.Repository.EmailLogRepo;
using HotelCollection.Data.Entity;

namespace HotelCollection.Infrastructure.Services
{
    public class SMTPService : ISMTPService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IEmailSentLog _emailSentLogRepo;

       // private IConfiguration _config;
        public SMTPService(
           IOptions<EmailSettings> emailSettings, IEmailSentLog emailSentLogRepo)
        {
            _emailSettings = emailSettings.Value;
           // _config = config;
            _emailSentLogRepo = emailSentLogRepo;
        }




        public async Task SendEmailAsync(string email, string subject, string message, string attachedfiles)
        {
            try
            {
                //var mimeMessage = new MimeMessage();

                //mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                //mimeMessage.To.Add(new MailboxAddress(email));
                //mimeMessage.Subject = subject;

                //mimeMessage.Body = new TextPart("html")
                //{
                //    Text = message
                //};
                 //var _emailSettings =  _config.GetSection("EmailSettings").Value;

                    //IOptions<EmailSettings> _emailSettings = new IOptions<EmailSettings>(_config.GetSection("EmailSettings"));

               // string fileUrl = _config.GetSection("GoSmarticleEndPoint:filesUrl").Value;

                MailMessage mm = new MailMessage();

                foreach (string n in email.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(n)) mm.To.Add(new MailAddress(n.Trim()));
                }
                mm.Sender = new MailAddress(_emailSettings.SenderName);
                mm.From = new MailAddress(_emailSettings.SenderName);
                mm.Subject = subject;
                mm.Body = message;
              

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                //attachements
                if (!string.IsNullOrWhiteSpace(attachedfiles))
                {
                    foreach (string attachm in attachedfiles.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(attachm)) mm.Attachments.Add(new Attachment(attachm));
                    }
                }



                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.IsBodyHtml = true;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;




                EmailSentLog emailLog = new EmailSentLog
                {
                    EmailContent = message,
                    RecipientEmail = email
                };


                using (var client = new SmtpClient())
                {

                    client.Port = _emailSettings.MailPort;
                    client.Host = _emailSettings.MailServer;
                    client.EnableSsl = _emailSettings.SSl;   //true
                    client.Timeout = 20000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password);


                    // Note: only needed if the SMTP server requires authentication
                    //await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    try { 
                        await Task.Run(() =>
                           client.Send(mm)
                        );
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable)
                            {
                                //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                //System.Threading.Thread.Sleep(5000);
                                client.Send(mm);
                            }
                            else
                            {
                                //Console.WriteLine("Failed to deliver message to {0}",
                                //    ex.InnerExceptions[i].FailedRecipient);

                                emailLog.Status = "Failed";
                            }
                        }
                    }

                   await  _emailSentLogRepo.LogEmailAsync(emailLog);
                // await client.DisconnectAsync(true);
            }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task SendEmailAsync(string email,string CC, string subject, string message, string attachedfiles)
        {
            try
            {
          

                MailMessage mm = new MailMessage();

                foreach (string n in email.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(n)) mm.To.Add(new MailAddress(n.Trim()));
                }
                mm.Sender = new MailAddress(_emailSettings.SenderName);
                mm.From = new MailAddress(_emailSettings.SenderName);
                mm.Subject = subject;
                mm.Body = message;
                if (!string.IsNullOrWhiteSpace(CC))
                {
                    foreach (string c in CC.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(c)) mm.CC.Add(new MailAddress(c.Trim()));
                    }
                }

                    mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                //attachements
                if (!string.IsNullOrWhiteSpace(attachedfiles))
                {
                    foreach (string attachm in attachedfiles.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(attachm)) mm.Attachments.Add(new Attachment(attachm));
                    }
                }



                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.IsBodyHtml = true;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;




                EmailSentLog emailLog = new EmailSentLog
                {
                    EmailContent = message,
                    RecipientEmail = email
                };


                using (var client = new SmtpClient())
                {

                    client.Port = _emailSettings.MailPort;
                    client.Host = _emailSettings.MailServer;
                    client.EnableSsl = _emailSettings.SSl;   //true
                    client.Timeout = 20000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password);


                    // Note: only needed if the SMTP server requires authentication
                    //await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    try
                    {
                        await Task.Run(() =>
                           client.Send(mm)
                        );
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable)
                            {
                                //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                //System.Threading.Thread.Sleep(5000);
                                client.Send(mm);
                            }
                            else
                            {
                                //Console.WriteLine("Failed to deliver message to {0}",
                                //    ex.InnerExceptions[i].FailedRecipient);

                                emailLog.Status = "Failed";
                            }
                        }
                    }

                    await _emailSentLogRepo.LogEmailAsync(emailLog);
                    // await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task SendEmailTransactionAsync(string email, string subject, string message, string attachedfiles)
        {
            try
            {
                //var mimeMessage = new MimeMessage();

                //mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                //mimeMessage.To.Add(new MailboxAddress(email));
                //mimeMessage.Subject = subject;

                //mimeMessage.Body = new TextPart("html")
                //{
                //    Text = message
                //};
                //var _emailSettings =  _config.GetSection("EmailSettings").Value;

                //IOptions<EmailSettings> _emailSettings = new IOptions<EmailSettings>(_config.GetSection("EmailSettings"));

                // string fileUrl = _config.GetSection("GoSmarticleEndPoint:filesUrl").Value;

                MailMessage mm = new MailMessage();

                foreach (string n in email.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(n)) mm.To.Add(new MailAddress(n.Trim()));
                }
                mm.Sender = new MailAddress(_emailSettings.SenderName);
                mm.From = new MailAddress(_emailSettings.Sender);
                mm.Subject = subject;
                mm.Body = message;

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //attachements
                if (!string.IsNullOrWhiteSpace(attachedfiles))
                {
                    foreach (string attachm in attachedfiles.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(attachm)) mm.Attachments.Add(new Attachment(attachm));
                    }
                }

                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.IsBodyHtml = true;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                EmailSentLog emailLog = new EmailSentLog
                {
                    EmailContent = message,
                    RecipientEmail = email
                };


                using (var client = new SmtpClient())
                {

                    client.Port = _emailSettings.MailPort;
                    client.Host = _emailSettings.MailServer;
                    client.EnableSsl = _emailSettings.SSl;   //true
                    client.Timeout = 20000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password);


                    // Note: only needed if the SMTP server requires authentication
                    //await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    try
                    {
                        await Task.Run(() =>
                           client.Send(mm)
                        );
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable)
                            {
                                //Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                //System.Threading.Thread.Sleep(5000);
                                client.Send(mm);
                            }
                            else
                            {
                                //Console.WriteLine("Failed to deliver message to {0}",
                                //    ex.InnerExceptions[i].FailedRecipient);

                                emailLog.Status = "Failed";
                            }
                        }
                    }

                    await _emailSentLogRepo.LogEmailTransactionAsync(emailLog);
                    // await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}
