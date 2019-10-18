using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class UserInfo
    {
        [PersonalData]
        public string Nombre { get; set; }
        [PersonalData]
        public string Apellido { get; set; }

        [Key]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 5)]
        public string Password { get; set; }


    }
}
