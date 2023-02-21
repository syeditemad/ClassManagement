using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.Models;

namespace ClassManagement.DAL
{
  public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeePersonelDetails>> GetEmployees();
        Task<IEnumerable<NomineeDetails>> GetNomineeDetailsList();
        Task<IEnumerable<Country>> GetCountryList();
        Task<IEnumerable<CountryState>> GetCountryStateList();
        Task<IEnumerable<DesignationList>> GetDesignationLists();
        Task<EmployeePersonelDetails> GetEmployeeById(int Id);
        Task<EmployeePersonelDetails> GetEmployeeByEmail(string email);
        Task<EmployeePersonelDetails> AddEmployee(EmployeePersonelDetails employeeModel);
        //Task<EmployeePersonelDetails> UpdateEmployee(EmployeePersonelDetails employeeModel);
        Task<EmployeePersonelDetails> DeleteEmployee(int Id);
        Task<NomineeDetails> GetNomineeDetailsById(int Id);
        Task<NomineeDetails> DeleteNomineeDetails(int Id);
        //Task<AddTOCartList> GetUserName()
        
    }
}
