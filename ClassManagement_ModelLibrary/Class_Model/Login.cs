using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ClassManagement_ModelLibrary.Class_Model
{


    [Keyless]
    public class ForgotPassWordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

   
    public class ResetPasswordModel
    {
        [Keyless]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = " Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "New Password and ConfirmPassword don't Match")]
        public string confirmPassword { get; set; }
        public String Email { get; set; }
        public String ToKen { get; set; }
    }

    [Keyless]
    public class MailRequestModel
    {
        public String To { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHtml { get; set; }
    }
     
    [Keyless]
    public class MailSettingModel
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHtml { get; set; }
    }
       
    public class ExternalLoginConfirmModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
