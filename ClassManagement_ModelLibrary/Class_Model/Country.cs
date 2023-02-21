using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement_ModelLibrary.Class_Model
{
   public  class Country
    {
        [Key]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set;  }
        [MaxLength(3)]
        [Required(ErrorMessage ="CountryCode Is Mandatory")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage ="CountryName is Mandatory")]
        [MaxLength(75)]
        public String CountryName { get; set; }
             
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
       [DataType(DataType.DateTime)]
       public DateTime? EditedOn { get; set; }
    }


    public class CountryState
    {

        [Key]
        public int StateId { get; set; }
       
        [MaxLength(20)]
        [Required(ErrorMessage =" StateCode is Mandatory")]
        public String StateCode { get; set; }
        [Required(ErrorMessage = "StatName is Mandatory")]
        public string StateName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EditedOn { get; set; }
       // [ForeignKey("Country")]
        public int CountryId { get; set; }
      //  public virtual Country Country { get; set; }


    }

    internal class KeylessAttribute : Attribute
    {
    }
    //public class City
    //{ 
    //    [Key]
    //    [MaxLength(3)]
    //    public string CityCode{ get; set; }

    //    [Required(ErrorMessage ="CityName is Mandatory")]
    //    public string CityName { get; set; }

    //    public DateTime CreatedOn { get; set; }
    //    public DateTime? EditedOn { get; set; }

    //    [ForeignKey("Country")]
    //    public string CountryCode { get; set; }

    //    public virtual Country Country { get; set; }


    //}
    //public class District
    //{
    //    [Key]
    //    public int DistrictId { get; set; }

    //    [Required(ErrorMessage ="DistrictName is Mandatory")]
    //    [StringLength(100)]
    //    public string DistrictName { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public DateTime? EditedOn { get; set; }

    //    [ForeignKey("City")]
    //    public int CityId { get; set; }
    //    public virtual City city { get; set; }


    //}

}
