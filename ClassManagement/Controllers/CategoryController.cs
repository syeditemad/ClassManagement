using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClassManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ClassManagement.DAL;

namespace ClassManagement.Controllers
{

    [Authorize(Roles = "User, Admin")]
    public class CategoryController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly InfrastructureDBContext _context;
        private readonly IEmailService _emailService;
        public CategoryController(InfrastructureDBContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,IEmailService emailService)
        {
            this._context = context;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._emailService = emailService;
        }
        [HttpGet]
        public IActionResult CoursesList()
        {
            return View("CoursesList");
        }

        /// <summary>
        /// Total CoursesList
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(_context.CourseLists.ToList());
        }

        [HttpGet]
        public IActionResult GetDesignationList()
        {
            return View(_context.DesignationList.ToList());
        }

        [HttpGet]
        public IActionResult GetDevelopementCourseList(int id)                  //string Course
        {


            var developementList = _context.CourseLists.Where(a => a.DesignationId == id).ToList();
            //return View(developementList);
            return View(developementList);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCourseList(int? Id)
        {

            List<DesignationList> StateList = _context.DesignationList.ToList();
            ViewBag.DesignationList = new SelectList(StateList, "DesignationId", "DesignationName");
            if (Id == null)
            {
                return View();
            }
            else
            {
                var model = await _context.CourseLists.FindAsync(Id);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddEditDesignationList(int? Id)
        {

            if (Id == null)
            {
                return View();
            }
            else
            {
                var model = await _context.DesignationList.FindAsync(Id);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditCourseList(int? Id, CourseList ModelObj)
        {
            bool IsCourseExist = false;
            CourseList CourseDataList = await _context.CourseLists.FindAsync(Id);
            if (CourseDataList != null)
            {
                IsCourseExist = true;
            }
            else
            {
                CourseDataList = new CourseList();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (IsCourseExist)
                    {
                        CourseDataList.DesignationId = ModelObj.DesignationId;
                        CourseDataList.AuthorName = ModelObj.AuthorName;
                        CourseDataList.CourseName = ModelObj.CourseName;
                        CourseDataList.CourseContent = ModelObj.CourseContent;
                        CourseDataList.Amount = ModelObj.Amount;
                        CourseDataList.CreatedOn = DateTime.Now;
                        CourseDataList.EditedOn = DateTime.Now;
                        _context.Update(CourseDataList);
                    }
                    else
                    {
                        CourseDataList.DesignationId = ModelObj.DesignationId;
                        CourseDataList.AuthorName = ModelObj.AuthorName;
                        CourseDataList.CourseName = ModelObj.CourseName;
                        CourseDataList.CourseContent = ModelObj.CourseContent;
                        CourseDataList.Amount = ModelObj.Amount;
                        CourseDataList.CreatedOn = DateTime.Now;
                        CourseDataList.EditedOn = DateTime.Now;
                        _context.Add(CourseDataList);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "Category");
            }
            return View(CourseDataList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEditDesignationList(int? Id, [Bind("DesignationId", "DesignationName", "CreatedOn", "EditedOn")] DesignationList ModelObj)
        {
            bool IsExist = false;
            DesignationList DesignationDataList = await _context.DesignationList.FindAsync(Id);
            if (DesignationDataList != null)
            {
                IsExist = true;
            }
            else
            {
                DesignationDataList = new DesignationList();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (IsExist)
                    {

                        DesignationDataList.DesignationName = ModelObj.DesignationName;
                        DesignationDataList.CreatedOn = DateTime.Now;
                        DesignationDataList.EditedOn = DateTime.Now;
                        _context.Update(DesignationDataList);
                    }
                    else
                    {

                        DesignationDataList.DesignationName = ModelObj.DesignationName;

                        DesignationDataList.CreatedOn = DateTime.Now;
                        DesignationDataList.EditedOn = DateTime.Now;
                        _context.Add(DesignationDataList);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("GetDesignationList", "Category");
            }
            return View(DesignationDataList);
        }


        [HttpGet]
        public async Task<IActionResult> AddtoCart()
        {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // var CardData = new List<AddTOCartList>();

            var CartList = await _context.addCart.Where(a => a.UserId == Userid).ToListAsync();

            return View(CartList);

        }


        [HttpGet]
        public IActionResult PurchaseCourseList(int Id)
        {
            var addToList = new List<AddTOCartList>();
            foreach (var cartList in _context.CourseLists.Where(a => a.CourseId == Id).ToList())
            {
                var car = new AddTOCartList
                {
                    //ProductId = Guid.NewGuid(),
                    CourseId = cartList.CourseId,
                    DesignationId = cartList.DesignationId,
                    AuthorName = cartList.AuthorName,
                    CourseName = cartList.CourseName,
                    CourseContent = cartList.CourseContent,
                    Amount = cartList.Amount,
                    CreatedOn = cartList.CreatedOn,
                    EditedOn = cartList.EditedOn,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                };
                addToList.Add(car);
                _context.Add(car);
                //_context.Add(addToList);
                _context.SaveChanges();
                ViewData["AddtoCart"] = car;

            }


            return RedirectToAction("Index", "Category");
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
                var Remove =   _context.addCart.Where(a=>a.ProductId==id).FirstOrDefault();
                if (Remove == null)
                {
                    return NotFound();
                }
                _context.addCart.Remove(Remove);
                  _context.SaveChanges();
                return RedirectToAction("AddtoCart", "Category");
            
           
        }

        [HttpGet]
        [ActionName("PaymentModeList")]
        public async Task<IActionResult> PaymentModeList(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    var Payment_Data = new List<OrderConfirmation>();
                    var paymentDataMode = _context.addCart.Find(Id);
                    var StatusEnum= from Payment P in Enum.GetValues(typeof(Payment))
                                    select new
                                    { ID = P, Name = P.ToString() };
                    ViewBag.StatusList = new SelectList(StatusEnum, "ID", "Name");
                    ViewBag.ProdductId = paymentDataMode.ProductId.ToString();
                    ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    ViewBag.EmailList = User.FindFirstValue(ClaimTypes.Name);
                    ViewBag.CoursenameList = paymentDataMode.CourseName;
                    
                       
                    return View();
                }
                
            }catch(Exception e)
            {
                throw;
            }
            return View();
       }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> PaymentModeList(Guid Id, OrderConfirmation orderConfirmation)
        {
                 try
                {
                  var paymentDataMode = _context.addCart.Find(Id);
                ViewBag.CoursenameList = paymentDataMode.CourseName;
                orderConfirmation.CreatedOn = DateTime.Now;
                        orderConfirmation.EditedOn = DateTime.Now;
                        _context.Add(orderConfirmation);
                        await _context.SaveChangesAsync();
                 await  _emailService.CoursePurchaseEmailConfirmation(orderConfirmation.Email, orderConfirmation.ProductId, ViewBag.CoursenameList);
                        Delete(Id);
                      return RedirectToAction("AddtoCart", "Category");   
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
              
            
        }

      [HttpGet]
      public async Task<IActionResult> EmployeePurchaseList()
        {
            try
            {
                //var id = "f59ce2ea-3934-4353-b74c-149c2dcbad70";
                var purchaselist =  await _context.orderConfirmations.ToListAsync();
                return View(purchaselist);

            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
