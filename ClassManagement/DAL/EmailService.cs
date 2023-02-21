using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;
using ClassManagement_ModelLibrary.Class_Model;
//using Microsoft.IdentityModel.Protocols;


namespace ClassManagement.DAL
{
    public class EmailService : IEmailService
    {
         private readonly MailRequestModel _mailSetting;

        public EmailService(IOptions<MailRequestModel> mailSettings)
        {
            _mailSetting = mailSettings.Value;
        }


        public async Task CoursePurchaseEmailConfirmation(string userEmail, string link, string otpobj)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress
                                       ("My Mail",
                                        "itemadhyder.service@gmail.com"
                                        ));
            mailMessage.To.Add(new MailboxAddress
                                        ("Receiver Name",
                                        userEmail
                                        ));
            mailMessage.Subject = "Purchase new ItemList";
            var builder = new BodyBuilder();
            //mailMessage.IsBodyHtml = true;
            //if (mailMessage.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in _mailSetting.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //               fileBytes = ms.ToArray();
            //           }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}

            builder.HtmlBody = $" Hello {userEmail} , \n  Thank You For Enrollment New Course \n  Your ProductId ={link} ,\n CourseName ={otpobj}";
            mailMessage.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(host: "smtp.gmail.com", port: 465, useSsl: true);
                client.Authenticate("itemadhyder.service@gmail.com", "febbwfkyvnrscmbc");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }


        public bool SendEmailConfirmation(string userEmail, string link,string Content)
        {
            bool Status = false;
            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress
                                           ("My Mail",
                                            "itemadhyder.service@gmail.com"
                                            ));
                mailMessage.Subject = "Email Confirmation ?";
                // mailMessage.Body = Link;
                mailMessage.To.Add(new MailboxAddress
                                            ("Verified Email",
                                            userEmail
                                            ));
                mailMessage.Subject = Content;
                var builder = new BodyBuilder();
                builder.HtmlBody = link;
                mailMessage.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(host: "smtp.gmail.com", port: 465, useSsl: true);
                    client.Authenticate("itemadhyder.service@gmail.com", "febbwfkyvnrscmbc");
                    client.Send(mailMessage);
                    Status = true;
                }
                return Status;
            }
            catch (Exception)
            {
                throw;
            }
            return Status;
        }


      

        public string GenerateOtp()
        {
            string[] Num = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            int len = Num.Length;
            string Otp = string.Empty;
            int OtpDigit = 6;
            string FinalDigit;
            int GetIndex;

            for (int i = 0; i < OtpDigit; i++)
            {


                Random rand = new Random();
                GetIndex = rand.Next(0, len);
                FinalDigit = Num[rand.Next(0, len)];
                Otp += FinalDigit;

            };

            return Otp;

        }

        public string GenerateCaptchaCode()
           {
            int Size = 6;
            //bool lowerCase = false;
          StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for(int i=1; i<=Size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble()+ 65)));
                builder.Append(ch);
            }
            return builder.ToString().ToUpper();
    }



}
}
