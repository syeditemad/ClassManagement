using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models

{
   public  class CreateRoleViewModel
    {
        [Required(ErrorMessage ="RoleName is Mandatory")]
        public string RoleName { get; set; }

    }

    public class EditRoleViewmodel
    {

        public EditRoleViewmodel()
        {
            Users = new List<string>();
        }


        public String Id { get; set; }
        [Required(ErrorMessage ="Role Name is Mandatory")]
        public String RoleName { get; set; }
        public List<string> Users { get; set; }
    }

    public class UserRoleViewModel
    {
        public string  UserId { get; set; }
        public string  UserName { get; set; }
        public bool  isSelected { get; set; }
    }
    public class UserRoleModel
    {
        public const string User = "User";
    }
}
