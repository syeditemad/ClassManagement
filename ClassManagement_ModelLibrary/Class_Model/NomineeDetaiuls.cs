using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement_ModelLibrary.Class_Model
{
     public class NomineeDetails
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage ="Nomineename is Mandatory")]
        [StringLength(50)]
        public string NomineeName { get; set; }

        [Required(ErrorMessage = "NomineeImage is Mandatory")]
        [Display(Name ="Nominee")]
        public string? NomineeImage { get; set; }
        
        [NotMapped]
         public IFormFile NomineePic { get; set; }

        [Required(ErrorMessage = "NomineeWork is Mandatory")]
        public string NomineeWork { get; set; }

        [Required(ErrorMessage ="Gender is Mandatory")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Adhare Card is Mandatory")]
        public long AdhareCardNo  { get; set; }

        [Required(ErrorMessage ="Email is Mandatory"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="MobileNo is Mandatory")]
        public long MobileNo { get; set;}
        public bool isAcitveded { get; set; }

    }
}
