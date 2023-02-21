using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore;

namespace ClassManagement_ModelLibrary.Class_Model
{
    
    public class UserRegistration
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage ="User Name Is Required")]
        [DataType(DataType.EmailAddress)]
        public String UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public long? MobileNo { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "Address is Required")]
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string OTP { get; set; }
        public string CaptchCode { get; set; }
   

        //public string
        //{ get; set; }
        //public IList<AuthenticationScheme> ExternalLogin { get; set; }


        //public UserRegistration() { }
        //public UserRegistration(ApplicationUser appUser)
        //{
        //    UserName = appUser.UserName;
        //    Email = appUser.Email;
        //    Password = appUser.PasswordHash;
        //}




    }

   public class CaptchCode
    {
       // [Key]
        public int id { get; set; }
        public string Tttle { get; set; }
        public string Comment  { get; set; }
    }

    public class EmailValidate
    {
        [EmailAddress]
        public string Email { get; set; }

    }
}
