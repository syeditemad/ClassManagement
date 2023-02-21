using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement.DAL;

namespace ClassManagement.DAL
{
    public interface IEmailService
    {
        Task CoursePurchaseEmailConfirmation(string userEmail, String link,string otpobj);
        //string GenerateOtp();
        
        bool SendEmailConfirmation(string userEmail, string Link, string Content);
        string GenerateCaptchaCode();
        string GenerateOtp();
    }
}
