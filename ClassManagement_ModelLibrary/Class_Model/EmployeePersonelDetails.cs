using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement_ModelLibrary.Class_Model
{
 public class EmployeePersonelDetails
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage ="FirstName is Mandatory")]
        [StringLength(50)]
        [Column(TypeName ="varchar(50)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "MiddleName is Mandatory")]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string MiddleName { get; set; }
        
        [Required(ErrorMessage ="SurName is Mandatory")]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string? SurName  { get; set; }

        [Required(ErrorMessage ="Gender Is Mandatory")]
       
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "DateofBirth Is Mandatory")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage ="DepartMent is Mandatory")]
        public DepartMent DepartMent { get; set; }

        [Required(ErrorMessage = "Email Is Mandatory"),DataType(DataType.EmailAddress)]
        public string Email  { get; set; }

        [Required(ErrorMessage = "PhoneNumber Is Mandatory")]
        [StringLength(10)]
        public string PhoneNumeber  { get; set; }
     
        [Required(ErrorMessage = "LandMark Is Mandatory")]
        public String Landmark { get; set; }
        public bool? isActive { get; set;}
        [Required]
        [ForeignKey("Counties")]

        public int Countryid { get; set; }
        public virtual Country Counties { get; set; }
        [Required]
        [ForeignKey("CountryState")]
        public int StateId { get; set; }
        public virtual CountryState CountryState  { get; set; }
    }

    public enum Gender
    {
        Unknow,
        Male ,
        Female,
        Transgender,
    }
    public enum Relation
    {    Relation,
        Father,
        Mother,
        Sister,
        Wife,
        Brother,
        Son,
        Daughter,
    }

    public enum DepartMent
    {
        Designation,
        HR ,
        Admin,
        BusinessAnalyst,
        SoftwareDeveloper,
        ProjectManager,

    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancel 
        
    }
    public enum Payment
    {
        Online,
        Case,

    }

}
