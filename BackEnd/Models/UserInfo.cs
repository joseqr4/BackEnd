using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class UserInfo: IdentityUser
    {
        [PersonalData]
       
        public string Name { get; set; }
        [PersonalData]
      
        public string Last_name { get; set; }
     

        [StringLength(30, MinimumLength = 5)]
        public string Password { get; set; }

        [StringLength(30, MinimumLength = 5)]
        public string Password_Confirmation { get; set; }

        


    }
}
