using ClassManagement.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ClassManagement_ModelLibrary.Class_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static iText.StyledXmlParser.Jsoup.Select.NodeFilter;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.IO.Source;
using System.Text;
using System.IO;
using System.Xml;
using System.Web;
namespace ClassManagement.Controllers
{
    public class CountryStateController : Controller
    {


        private readonly InfrastructureDBContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        public CountryStateController(InfrastructureDBContext context, IEmployeeRepository employeeRepository)
        {
            this._context = context;
            this._employeeRepository = employeeRepository;

        }
        [Authorize(Roles = "User, Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEditview(int? Id)
        {
            ViewBag.CountryList = GetCountry();
            if (Id == null)
            {
                return View();
            }
           
            var DataModel = await _context.countryState.FindAsync(Id);
            return View(DataModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditview(int? Id, [Bind("StateId", "StateCode", "StateName", "CreatedOn", "EditedOn","CountryId")] CountryState ModelObj)
        {
            bool IsCountrExist = false;
            CountryState StateData = await _context.countryState.FindAsync(Id);
            if (StateData != null)
            {
                IsCountrExist = true;
            }
            else
            {
                StateData = new CountryState();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (IsCountrExist)
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        StateData.StateCode = ModelObj.StateCode;
                        StateData.StateName = ModelObj.StateName;
                        StateData.CountryId = ModelObj.CountryId;
                        StateData.CreatedOn = DateTime.Now;
                        StateData.EditedOn = DateTime.Now;
                        _context.Update(StateData);

                    }
                    else
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        StateData.StateCode = ModelObj.StateCode;
                        StateData.StateName = ModelObj.StateName;
                        StateData.CountryId = ModelObj.CountryId;
                        StateData.CreatedOn = DateTime.Now;
                        StateData.EditedOn = DateTime.Now;
                        _context.Add(StateData);

                    }

                    await _context.SaveChangesAsync();
                    TempData["Data"] = $"{StateData.StateName} Added Successfully!";

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "EmployeeDetails");
            }
            return View(StateData);

        }


        /// <summary>
        /// Get CountryState Details list  by Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>result</returns>
        [HttpGet]
        public async Task<IActionResult> CountryStateDetails(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                var result = _context.countryState.FirstOrDefault(x => x.CountryId == Id);
                return View(result);
            }
        }

        private List<SelectListItem> GetCountry()
        {
            var Country = new List<SelectListItem>();
            List<Country> CountryList = _context.country.ToList();
            Country = CountryList.Select(u => new SelectListItem()
            {

                Value = u.CountryId.ToString(),
                Text = u.CountryName


            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "------Select Country------"
            };
            Country.Insert(0, defItem);
            return Country;
        }

       // [HttpPost]
        //public async Task<ActionResult> ExportToPdf(HtmlTextWriter HtmlTextWriter)
        //{
            
        //    List<CountryState> pdfCountryState= await _context.countryState.ToListAsync() as List<CountryState>;

        //    using(StringWriter  sb= new StringWriter())
        //    {
        //        using(HtmlTextWriter hw = new HtmlTextWriter(sb))
        //        {
        //            GridView gridview = new GridView();
        //            gridview.DataSource = pdfCountryState;
        //            gridview.DataBind();

        //        }
        //    }
        //}



    }
}
