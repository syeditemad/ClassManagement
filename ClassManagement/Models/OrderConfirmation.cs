using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClassManagement.Models;
using Microsoft.AspNetCore.Identity;
using ClassManagement_ModelLibrary.Class_Model;

namespace ClassManagement.Models
{
    public class OrderConfirmation
    {
        [Key]

        public Guid TransactionId { get; set;}
        public string ProductId { get; set;}
        public virtual AddTOCartList product { get; set; }
        public string UserId { get; set;}
        public virtual IdentityUser IdentityUser { get; set;}

        [Required(ErrorMessage ="PaymentMode is Mandatory")]
        public Payment PaymentMode { get; set;}

        [Required(ErrorMessage ="ContactNo is Mandatory")]
        public long ContactNo { get; set; }

        [Required(ErrorMessage ="Address is Mandatory"),MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage ="Email is Mandatory"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }

    }
}
