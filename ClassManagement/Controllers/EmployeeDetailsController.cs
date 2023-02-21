using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement.DAL;
using ClassManagement.BAL;
using ClassManagement_ModelLibrary.Class_Model;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using System.Text;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.Diagnostics;
using DinkToPdf;
using DinkToPdf.Contracts;
using ClassManagement.Models;
//using Document = iTextSharp.text.Document;

namespace ClassManagement.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly InfrastructureDBContext _context;
        private IConverter _converter;



        public EmployeeDetailsController(InfrastructureDBContext context, IEmployeeRepository employeeRepository, IWebHostEnvironment webHost, IConverter converter)
        {
            this._context = context;
            this._employeeRepository = employeeRepository;
            this.webHostEnvironment = webHost;
            this._converter = converter;

        }
        [Authorize(Roles = "User, Admin")]
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {


            var Table = new MultipleTable_Bind
            {
                GetEmployeeDetailsList = await _employeeRepository.GetEmployees(),
                NomineeDetailsList = await _employeeRepository.GetNomineeDetailsList(),
                GetCountryListView = await _employeeRepository.GetCountryList(),
                GetCountryStateViewList = await _employeeRepository.GetCountryStateList(),

            };
            ViewData["IndexPage"] = Table;
           // ViewBag.Designation = GetDesignationList();

            return View(Table);
        }

        [HttpGet]
        public async Task<EmployeePersonelDetails> GetEmployeeDetailsById(int Id)
        {
            return await _employeeRepository.GetEmployeeById(Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetailsList(string Filter_Value)
        {

            var ModelSearch = _context.employeePersonelDetails.ToList();
            if (!string.IsNullOrEmpty(Filter_Value))
            {

                ModelSearch = await _context.employeePersonelDetails.Where(e => e.Email.Contains(Filter_Value) || e.FirstName.Contains(Filter_Value)).ToListAsync();
                ViewBag.searchText = Filter_Value;
                return View(ModelSearch);
            }
            else
            {
                ModelSearch = await _context.employeePersonelDetails.ToListAsync();
                return View(ModelSearch);
            }
           
            //int pageSize = 3;
           // return View( await PaginatedList<EmployeePersonelDetails>.CreateAsync(ModelSearch.AsQueryable(), pageNumber ?? 1, pageSize));
        }


        /// <summary>
        /// Create  EmployeePersonel Details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEditEmployeeDetails(int? Id)
        {
            var enumData = from Gender P in Enum.GetValues(typeof(Gender))
                           select new
                           { ID = P, Name = P.ToString() };


            var enumDepartment = from DepartMent p in Enum.GetValues(typeof(DepartMent))
                                 select new
                                 { ID = p, Name = p.ToString() };

            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");
            ViewBag.DepartEnumlist = new SelectList(enumDepartment, "ID", "Name");
            ViewBag.CountryList = GetCountry();
            List<CountryState> StateList = _context.countryState.ToList();
            ViewBag.CountryStateList = new SelectList(StateList, "StateId", "StateName");
            //ViewBag.CountryStateList= GetStateList();


            if (Id == null)
            {

                return View();
            }

            var DataModel = await _context.employeePersonelDetails.FindAsync(Id);
            return View(DataModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditEmployeeDetails(int? Id, [Bind("EmployeeId", "FirstName", "MiddleName", "SurName", "Gender", "DateOfBirth", "DepartMent", "Email", "PhoneNumeber", "Countryid", "StateId", "Landmark", "isActive")] EmployeePersonelDetails ModelObj)
        {
            bool IsExistEmployeeDetails = false;

            EmployeePersonelDetails EmployeeList = await _context.employeePersonelDetails.FindAsync(Id);

            if (EmployeeList != null)
            {
                IsExistEmployeeDetails = true;
            }
            else
            {
                EmployeeList = new EmployeePersonelDetails();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (IsExistEmployeeDetails)
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        EmployeeList.FirstName = ModelObj.FirstName;
                        EmployeeList.MiddleName = ModelObj.MiddleName;
                        EmployeeList.SurName = ModelObj.SurName;
                        EmployeeList.Gender = ModelObj.Gender;
                        EmployeeList.DateOfBirth = ModelObj.DateOfBirth;
                        EmployeeList.DepartMent = ModelObj.DepartMent;
                        EmployeeList.Email = ModelObj.Email;
                        EmployeeList.PhoneNumeber = ModelObj.PhoneNumeber;
                        EmployeeList.Countryid = ModelObj.Countryid;
                        EmployeeList.StateId = ModelObj.StateId;
                        //EmployeeList.StateId = ModelObj.StateId;
                        //EmployeeList.StateName = ModelObj.StateName;
                        //EmployeeList.CityId = ModelObj.CityId;
                        //EmployeeList.CityName = ModelObj.CityName;
                        //EmployeeList.DistrictId = ModelObj.DistrictId;
                        //EmployeeList.DistrictName = ModelObj.DistrictName;
                        EmployeeList.Landmark = ModelObj.Landmark;
                        EmployeeList.isActive = true;

                        _context.Update(EmployeeList);

                    }
                    else
                    {
                        //employee.EmployeeID = employeeData.EmployeeID;
                        EmployeeList.FirstName = ModelObj.FirstName;
                        EmployeeList.MiddleName = ModelObj.MiddleName;
                        EmployeeList.SurName = ModelObj.SurName;
                        EmployeeList.Gender = ModelObj.Gender;
                        EmployeeList.DateOfBirth = ModelObj.DateOfBirth;
                        EmployeeList.DepartMent = ModelObj.DepartMent;
                        EmployeeList.Email = ModelObj.Email;
                        EmployeeList.PhoneNumeber = ModelObj.PhoneNumeber;
                        EmployeeList.Countryid = ModelObj.Countryid;
                        EmployeeList.StateId = ModelObj.StateId;
                        // EmployeeList.CountryName = ModelObj.CountryName;
                        //EmployeeList.StateId = ModelObj.StateId;
                        //EmployeeList.StateName = ModelObj.StateName;
                        //EmployeeList.CityId = ModelObj.CityId;
                        //EmployeeList.CityName = ModelObj.CityName;
                        //EmployeeList.DistrictId = ModelObj.DistrictId;
                        //EmployeeList.DistrictName = ModelObj.DistrictName;
                        EmployeeList.Landmark = ModelObj.Landmark;
                        EmployeeList.isActive = true;
                        _context.Add(EmployeeList);

                    }

                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "EmployeeDetails");
            }
            return View(EmployeeList);
        }




        /// <summary>
        /// Create Nomineedetails List 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateNomineeDetails(int? Id)
        {
            if (Id == null)
            {
                var enumData = from Gender P in Enum.GetValues(typeof(Gender))
                               select new
                               { ID = P, Name = P.ToString() };
                ViewBag.EnumList = new SelectList(enumData, "ID", "Name");

                return View();
            }
            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNomineeDetails(int? Id, NomineeDetails ModelObj)
        {
           if(ModelObj.NomineeName!=null && ModelObj.NomineeImage!=null && ModelObj.NomineeWork != null)
            { 
                string UniqueFiles = UploadedFile(ModelObj);
                ModelObj.NomineeImage = UniqueFiles;
                ModelObj.isAcitveded = true;
                _context.Add(ModelObj);
                //_context.Entry(ModelObj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Validation Error");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNomineeDetails(int? id)
        {
            var result = await _context.nomineDetails.FindAsync(id);
            ViewBag.UpdateEnumValue = result.Gender;
            if (result != null)
            {
                return View(result);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateNomineeDetails(int? Id, [Bind("EmployeeId", "NomineeName", "NomineeImage", "NomineeWork", "Gender", "DateofBirth", "AdhareCardNo", "Email", "MobileNo", "isAcitveded")] NomineeDetails ModelObj)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.nomineDetails.Where(x => x.EmployeeId == Id).FirstOrDefault();
                    if (result != null)
                    {
                        //result.EmployeeId = ModelObj.EmployeeId;
                        result.NomineeName = ModelObj.NomineeName;
                        result.NomineeImage = ModelObj.NomineeImage;
                        result.NomineeWork = ModelObj.NomineeWork;
                        result.Gender = ModelObj.Gender;
                        result.DateofBirth = ModelObj.DateofBirth;
                        result.AdhareCardNo = ModelObj.AdhareCardNo;
                        result.Email = ModelObj.Email;
                        result.isAcitveded = ModelObj.isAcitveded;
                        _context.Update(result);
                        _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return NotFound();



        }



        /// <summary>
        /// Nominee Details View Modal
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NomineeDetialsView(int Id)
        {

            if (Id == null)
            {
                return View("Error_view");
            }

            var result = _context.nomineDetails.FirstOrDefault(x => x.EmployeeId == Id);
            return View(result);


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNomineeDetails(int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.nomineDetails.FindAsync(Id);
                _context.nomineDetails.Remove(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }


        /// <summary>
        /// Employee Details View Modal
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EmployeeDetialsView(int Id)
        {
            if (Id == null)
            {
                return View("Error_view");
            }

            var result = _context.employeePersonelDetails.FirstOrDefault(x => x.EmployeeId == Id);
            return View(result);

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeDetails(int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.employeePersonelDetails.FindAsync(Id);
                _context.employeePersonelDetails.Remove(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return NotFound();
        }



        /// <summary>
        /// Get Employee Details list  by Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>result</returns>
        [HttpGet]
        public async Task<IActionResult> EmployeeDetailsView(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {

                ViewBag.CountryList = GetCountry();
                var result =  await _context.employeePersonelDetails.FirstOrDefaultAsync(x => x.EmployeeId == Id);
                return View(result);
            }
        }

        /// <summary>
        /// Create Uploaded Image Path And Folder in Project
        /// </summary>
        /// <param name="modelObj"></param>
        /// <returns>UniqueFileName</returns>
        private String UploadedFile(NomineeDetails modelObj)
        {
            string UniqueFileName = null;
            if (modelObj.NomineePic != null)
            {
                string uploadedsFolder = System.IO.Path.Combine(webHostEnvironment.WebRootPath, "image");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + modelObj.NomineePic.FileName;
                string filePath = System.IO.Path.Combine(uploadedsFolder, UniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    modelObj.NomineePic.CopyTo(fileStream);
                }

            }
            return UniqueFileName;

        }


        /// <summary>
        /// Get listing Country List Loading in Add&Edit model
        /// </summary>
        /// <returns></returns>

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
        private List<SelectListItem> GetStateList()
        {
            var CountryState = new List<SelectListItem>();
            List<CountryState> countrystateList = _context.countryState.ToList();
            CountryState = countrystateList.Select(u => new SelectListItem()
            {

                Value = u.StateId.ToString(),
                Text = u.StateName


            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "------Select CountryState------"
            };
            CountryState.Insert(0, defItem);
            return CountryState;
        }


        public IActionResult GetEmployeeDetailsList()
        {
            var ModelSearch = _context.employeePersonelDetails.ToList();
            return View();
        }

        [HttpPost]
        public FileResult ExportToPdf(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(stream, pdfDocument);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", "EmployeeDetailsReport.pdf");
            }

        }
        [HttpPost]
        public FileResult ExportExcelTO(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

       




        //[HttpGet]
        //public IActionResult CreatePDF()
        //{
        //    var globalSetting = new GlobalSettings
        //    {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4,
        //        Margins = new MarginSettings { Top = 10 },
        //        DocumentTitle = "Pdf Report",
        //        Out = @"C:\Users\itemad.hyder\Desktop\PdfCreateor"

        //    };
        //    var objectSetting = new ObjectSettings
        //    {
        //        PagesCount = true,
        //        HtmlContent = GetHTMLString(),
        //        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
        //        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        //    };
        //    var pdf = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = globalSetting,
        //        Objects = { objectSetting }
        //    };
        //   var file= _converter.Convert(pdf);
        //    return File(file,"application/pdf");
        //}
        /// <summary>
        /// retriving The Html formate for Pdf Creating
        /// </summary>
        /// <returns></returns>
        //public string GetHTMLString()
        //    {
        //        var employees = _context.employeePersonelDetails.ToList();
        //        var sb = new StringBuilder();
        //        sb.Append(@"
        //                <html>
        //                    <head>
        //                    </head>
        //                    <body>
        //                        <div class='header'><h1>This is the generated PDF report!!!</h1></div>
        //                        <table align='center'>
        //                            <tr>
        //                                <th>FirstName</th>
        //                                <th>MiddleName</th>
        //                                <th>DateOfBirth</th>
        //                                <th>Gender</th>
        //                                <th>Countryid</th>
        //                                <th>Email</th>
        //                            </tr>");
        //        foreach (var emp in employees)
        //        {
        //            sb.AppendFormat(@"<tr>
        //                            <td>{0}</td>
        //                            <td>{1}</td>
        //                            <td>{2}</td>
        //                            <td>{3}</td>
        //                          </tr>", emp.FirstName, emp.MiddleName, emp.DateOfBirth, emp.Gender,emp.Countryid,emp.Email);
        //        }
        //        sb.Append(@"
        //                        </table>
        //                    </body>
        //                </html>");
        //        return sb.ToString();
        //    }

        //[HttpPost]
        //public FileResult Export()
        //{

        //    List<object> customers = _context.employeePersonelDetails.ToList<object>();
        //      StringBuilder sb = new StringBuilder();

        //        sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");

        //    //Building the Header row.
        //    sb.Append("<tr>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>FirstName</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>MiddleName</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>CountryState</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Country</th>");
        //    sb.Append("</tr>");

        //    for (int i = 0; i < customers.Count; i++)
        //    {
        //        string[] customer = (string[])customers[i];
        //        sb.Append("<tr>");
        //        for (int j = 0; j < customer.Length; j++)
        //        {
        //            //Append data.
        //            sb.Append("<td style='border: 1px solid #ccc'>");
        //            sb.Append(customer[j]);
        //            sb.Append("</td>");
        //        }
        //        sb.Append("</tr>");
        //    }

        //    //Table end.
        //    sb.Append("</table>");
        //    using (MemoryStream stream = new MemoryStream())
        //    {

        //        StringReader reader = new StringReader(customers);
        //        Microsoft.CodeAnalysis.Document PdfFile = new Microsoft.CodeAnalysis.Document(PageSize.A4);
        //        PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //        PdfFile.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
        //        PdfFile.Close();
        //        return File(stream.ToArray(), "application/pdf", "ExportData.pdf");


        //    }
        //    //using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
        //    //{
        //    //    ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        //    //    // PdfWriter writer =  PdfWriter.GetInstance(byteArrayOutputStream);
        //    //    // PdfDocument pdfDocument = new PdfDocument(writer)
        //    //    System.Reflection.Metadata.Document PdfFile = new System.Reflection.Metadata.Document(PageSize.A4);
        //    //    iText.Kernel.Pdf.PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //    //    PdfFile.Open();
        //    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
        //    //    PdfFile.Close();
        //    //    return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
        //    //}
        //}
    }

}
