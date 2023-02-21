using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ClassManagement.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var User=_userManager.Users.ToList();
            return View(User);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]

        public async Task <IActionResult> CreateRole(CreateRoleViewModel modelObj)
        {
            if (ModelState.IsValid)
            {
                IdentityRole userRole = new IdentityRole
                {
                    Name = modelObj.RoleName 
                
                };
                IdentityResult result = await _roleManager.CreateAsync(userRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(modelObj);
        }

        /// <summary>
        /// Create list of Roles
        /// </summary>
        /// <returns>Role</returns>

        [HttpGet]
        public IActionResult ListRoles()
        {
            var Role = _roleManager.Roles;
            return View(Role);
        }

         [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be Founded ";
                return View("NotFound");
            }
            var model = new EditRoleViewmodel
            {
                Id = Role.Id,
                RoleName = Role.Name

            };
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, Role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewmodel modelObj)
        {
            var Role = await _roleManager.FindByIdAsync(modelObj.Id);
            if (Role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {modelObj.Id} cannot be Founded ";
                return View("NotFound");
            }
            else
            {
               // Role.Id = modelObj.Id;
                Role.Name = modelObj.RoleName;
               var result =await _roleManager.UpdateAsync(Role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(modelObj);
            }
           
           
            
        }


        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string RoleId)
        {
            ViewBag.roleId = RoleId;
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role With Id = {RoleId} cannot  be found";
                return View("NotFound");
            }
            var ModelRole = new List<UserRoleViewModel>();
            foreach(var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.isSelected = true;
                }
                else
                {
                    userRoleViewModel.isSelected = false;
                }
                ModelRole.Add(userRoleViewModel);
            }
            return View(ModelRole);
        }


        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role With Id = {roleId} cannot  be found";
                return View("NotFound");
            }
            for( int i = 0; i< model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].isSelected && !(await _userManager.IsInRoleAsync(user,role.Name)))
                {
                    result=await _userManager.AddToRoleAsync(user, role.Name);
                }else if(!model[i].isSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                 {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", "Administration", new { Id = role });
                    }
                }
            
            }

            return RedirectToAction("EditRole", "Administration", new { Id = roleId });
           
        }
    }
}
