using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.DAL;
using ClassManagement.BAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ClassManagement.Controllers
{
    public class CountryController : Controller
    {
        private readonly InfrastructureDBContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        public CountryController(InfrastructureDBContext context,IEmployeeRepository employeeRepository)
        {
            this._context = context;
            this._employeeRepository = employeeRepository;
                
        }
        [Authorize(Roles = "User, Admin")]

        public  IActionResult Index()
        {
            //var table = new MultipleTable_Bind
            //{
            //    GetCountryList =  _context.country.ToList()
            //};
            return View();
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEditview(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            
                var DataModel = await _context.country.FindAsync(Id);
               return View(DataModel);
        }

     [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
     public async Task<IActionResult> AddEditview(int? Id,[Bind("CountryId","CountryName","CountryCode", "CreatedOn", "EditedOn")]Country ModelObj)
        {
                     bool IsCountrExist = false;
                     Country CourseDataList = await _context.country.FindAsync(Id);
            if (CourseDataList != null)
            {
                IsCountrExist = true;
            }
            else
            {
                CourseDataList = new Country();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (IsCountrExist)
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        CourseDataList.CountryName = ModelObj.CountryName;
                        CourseDataList.CountryCode = ModelObj.CountryCode;
                        CourseDataList.CreatedOn = DateTime.Now;
                        CourseDataList.EditedOn = DateTime.Now;
                           _context.Update(CourseDataList);

                    }
                    else
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        CourseDataList.CountryName = ModelObj.CountryName;
                        CourseDataList.CountryCode = ModelObj.CountryCode;
                        CourseDataList.CreatedOn = ModelObj.CreatedOn;
                        CourseDataList.EditedOn = DateTime.Now;
                        _context.Add(CourseDataList);

                    }

                    await _context.SaveChangesAsync();
                    TempData["Data"] = $"{CourseDataList.CountryName} Added Successfully!";


                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index","EmployeeDetails");
            }
            return View(CourseDataList);

        }



            /// <summary>
           /// Get Country Details list  by Id 
           /// </summary>
           /// <param name="Id"></param>
           /// <returns>result</returns>
     [HttpGet]
     public IActionResult CountryDetails(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                var result =   _context.country.FirstOrDefault(x => x.CountryId == Id);
                return View(result);
            }
        }




    }
}
