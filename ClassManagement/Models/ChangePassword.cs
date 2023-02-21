using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
    
    public class ChangePassword
    {
       [Display(Name = "CurrentPassword")]
       [DataType(DataType.Password), Required(ErrorMessage ="CurrentPassword is mandatory")]
        public string CurrentPassword { get; set; }

        [Display(Name = "NewPassword")]
        [DataType(DataType.Password), Required(ErrorMessage = "NewPassword is mandatory")]
        public String NewPassword { get; set; }
        [Display(Name = "ConfirmPassword"), Compare("NewPassword",ErrorMessage ="The NeWPassword and ConfirmPassword do not Match")]
        [DataType(DataType.Password),Required(ErrorMessage = "NewPassword is mandatory")]
        public String ConfirmPassword { get; set; }
    }
}
