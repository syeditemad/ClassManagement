using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ClassManagement_ModelLibrary.Class_Model
{
     public class CourseList
    {
        [Key]
        public int CourseId { get; set; }
        [Required(ErrorMessage ="CourseName is Mandatory")]

        [ForeignKey("Designation")]
        public int DesignationId { get; set; }
        public virtual DesignationList Designation { get; set; }

        [Required(ErrorMessage = "AuthorName is Mandatory"),MaxLength(70)]
        public String AuthorName { get; set; }
        [Required(ErrorMessage = "CourseName is Mandatory"),MaxLength(70)]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "CourseName is Mandatory"), MaxLength(150)]
        public string CourseContent { get; set; }

        //[DataType(DataType.Currency)]

        [Required(ErrorMessage = "Amount is Mandatory")]
        public int Amount { get; set; }
        [Required,DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [ Required,DataType(DataType.Date)]
        public DateTime? EditedOn { get; set; }

        //Public bool? IsActive {get; set;}

    }


    public class DesignationList
    {
        [Key]
        public int DesignationId { get; set; }

        [Required(ErrorMessage ="DesignationName is Mandatory")]
        public string DesignationName { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime EditedOn { get; set; }

    }

    //public class PaymentModel
    //{
    //    [Key]
    //    public int PaymentId { get; set; }

    //    public int TotalAmount { get; set; }

    //    public Payment PaymentStatus { get; set; }

    //    public string? UpidNo { get; set; }
    //    //public bool IsUpiPayment { get; set; }
    //    //public bool IsCaseOnDelivery { get; set; }
        
    //}



    public class UpipaymentModel
    {
        public string UpiId { get; set; }
        public string UpiName  { get; set; }
        public String? PaymentImage { get; set; }
        [NotMapped]
        public IFormFile formFile { get; set; }

    }

    //public class DeliveryModel
    //{

    //    public Guid OrderId { get; set; }
    //    [ForeignKey("paymentModel")]
    //    public int PaymentId { get; set; }

    //    public PaymentModel paymentModel { get; set; }
    //    public String DeliveryPerson { get; set; }
    //    public PaymentStatus Status { get; set; }
    //    public string DeliveryName { get; set; }
    //    [Required(ErrorMessage =""),MaxLength(150)]
    //    public string DeliveryAddress { get; set; }
    //    public string ContactNo { get; set; }


    //}

    

}
