using ClassManagement_ModelLibrary.Class_Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Models
{
    public class AddTOCartList
    {
        [Key]

        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "AuthorName is Mandatory"), MaxLength(70)]
        public String AuthorName { get; set; }
        [Required(ErrorMessage = "CourseName is Mandatory"), MaxLength(70)]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "CourseContent is Mandatory"), MaxLength(150)]
        public string CourseContent { get; set; }

        //[DataType(DataType.Currency)]

        [Required(ErrorMessage = "Amount is Mandatory")]
        public int Amount { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? EditedOn { get; set; }
       

        [ForeignKey("Courses")]
        public int CourseId { get; set; }
        public  CourseList Courses { get; set; }

        [Required(ErrorMessage = "DesignationId is Mandatory")]
        public int DesignationId { get; set; }
       
        [ForeignKey("identityUser")]

        public string UserId { get; set; }
        public virtual IdentityUser identityUser { get; set; }

    }

    public class CheckOutList
    {
        [Key]
        public Guid Checkid { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser identityUser { get; set; }
        public int CourseId { get; set; }
        public virtual CourseList CourseList { get; set; }

        [Required(ErrorMessage = "TotalAmount is Mandatory")]
        public int TotalAmount { get; set; }
        [Required(ErrorMessage = "PaymentStatus is Mandatory")]
        public PaymentStatus PaymentStatus { get; set; }
        [Required(ErrorMessage = "CreatOn is Mandatory"), DataType(DataType.DateTime)]
        public DateTime CreatOn { get; set; }

        public bool IsConfirm { get; set; }

    }

    public class PaymentMode
    {
        [Key]
        public Guid PaymentId { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual AddTOCartList Product { get; set; }
        [Required(ErrorMessage ="CourseName is Mandatory"), MaxLength(100)]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Status is Mandatory")]
        public Payment Status { get; set; }
        public String? UpiId { get; set; }
        public int TotalAmount { get; set; }
        [Required(ErrorMessage = "Address is Mandatory"), MaxLength(200)]
        public String Address { get; set; }
        [Required(ErrorMessage ="ContactNo is Mandatory")]
        public long ContactNo { get; set; }

        [Required(ErrorMessage ="Email is Mandatory"), DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public DateTime CreatedOn { get; set; }

       
    }

   
}
