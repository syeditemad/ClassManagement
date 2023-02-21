using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.Models;

namespace ClassManagement.DAL
{
    public class InfrastructureDBContext : IdentityDbContext
    {
        public InfrastructureDBContext(DbContextOptions<InfrastructureDBContext> options): base(options)
        {

        }
        public DbSet<EmployeePersonelDetails> employeePersonelDetails { get; set; } 
        public DbSet <Country> country { get; set; }
        public DbSet <CountryState> countryState { get; set; }
        public DbSet <NomineeDetails> nomineDetails { get; set; }
        public DbSet<ClassManagement_ModelLibrary.Class_Model.UserRegistration> UserRegistration { get; set; }
      
        public DbSet<CourseList> CourseLists { get; set; }
      
        public DbSet<ClassManagement_ModelLibrary.Class_Model.DesignationList> DesignationList { get; set; }
        
        // Extra Part 
        public DbSet<AddTOCartList> addCart { get; set; }

        public DbSet<CheckOutList> checkList { get; set; }

        public DbSet<PaymentMode> payment { get; set;  }

        public DbSet<OrderConfirmation> orderConfirmations { get; set;}

    }
}
