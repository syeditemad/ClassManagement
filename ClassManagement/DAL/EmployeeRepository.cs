using ClassManagement_ModelLibrary.Class_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement.DAL;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ClassManagement.DAL
{

    
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly InfrastructureDBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public EmployeeRepository(InfrastructureDBContext context, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            webHostEnvironment = hostEnvironment;
        }
        public async Task<EmployeePersonelDetails> AddEmployee(EmployeePersonelDetails employeeModel)
        {
            var result = await _context.employeePersonelDetails.AddAsync(employeeModel);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<EmployeePersonelDetails> DeleteEmployee(int Id)
        {
            var result = await _context.employeePersonelDetails
               .FirstOrDefaultAsync(x => x.EmployeeId == Id);
            if (result != null)
            {
                _context.employeePersonelDetails.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public Task<EmployeePersonelDetails> GetEmployeeByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeePersonelDetails> GetEmployeeById(int Id)
        {
            return await _context.employeePersonelDetails.FirstOrDefaultAsync(x => x.EmployeeId == Id);
        }

        public async Task<IEnumerable<EmployeePersonelDetails>> GetEmployees()
        {
            return await _context.employeePersonelDetails.ToListAsync();
        }

        //public async Task<EmployeePersonelDetails> UpdateEmployee(EmployeePersonelDetails employeeModel)
        //{
        //    var result = await _context.employeePersonelDetails
        //       .FirstOrDefaultAsync(x => x.EmployeeId == employeeModel.EmployeeId);

        //    // var result = await _context.employee
        //    //  .FirstOrDefaultAsync(e => e.EmployeeId == employeeModel.EmployeeId);

        //    if (result != null)
        //    {
        //        result.FirstName = employeeModel.FirstName;
        //        result.MiddleName = employeeModel.MiddleName;
        //        result.SurName = employeeModel.SurName;
        //        result.Email = employeeModel.Email;
        //        result.Gender = employeeModel.Gender;
        //        result.DateOfBirth = employeeModel.DateOfBirth;
        //        result.DepartMent = employeeModel.DepartMent;
        //        result.PhoneNumeber = employeeModel.PhoneNumeber;
        //        result.Countryid = employeeModel.Countryid;
        //        result.Country = employeeModel.Country;
        //        result.Landmark = employeeModel.Landmark;
        //        result.isActive = employeeModel.isActive;

        //        await _context.SaveChangesAsync();

        //        return result;
        //    }

        //    return null;
        //}

        /// <summary>
        /// Get Nominee List of all Employee
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NomineeDetails>> GetNomineeDetailsList()
        {
            return await _context.nomineDetails.ToListAsync();
        }


        /// <summary>
        /// Get Country List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Country>> GetCountryList()
        {

            return await _context.country.ToListAsync();
        }


        /// <summary>
        /// Get Country State List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CountryState>> GetCountryStateList()
        {

            return await _context.countryState.ToListAsync();
        }

        public async Task<NomineeDetails> GetNomineeDetailsById(int Id)
        {
            return await _context.nomineDetails.FirstOrDefaultAsync(x => x.EmployeeId == Id);
        }

       
        public async Task<NomineeDetails> DeleteNomineeDetails(int Id)
        {
            var result = await _context.nomineDetails
               .FirstOrDefaultAsync(x => x.EmployeeId == Id);
            if (result != null)
            {
                _context.nomineDetails.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public Task<IEnumerable<DesignationList>> GetDesignationLists()
        {
            throw new NotImplementedException();
        }

        //public async Task<NomineeDetails> SoftDeleteNomineeDetail(int Id)
        //{
        //    var result = await _context.nomineDetails.FirstOrDefaultAsync(x => x.isAcitveded == true);
        //    if (result != null)
        //    {
        //        result.isAcitveded == false;
        //    }
        //}
    }
}
