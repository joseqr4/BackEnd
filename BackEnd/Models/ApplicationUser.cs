using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [StringLength(120)]
        public string Name { get; set; }
        [PersonalData]
        public string Last_Name { get; set; }

        public string password_confirmation { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public String? Photo { get; set; }

        public DateTime? Date_birth { get; set; }

 
        //public List<InterestsUsers>? InterestsUsuarios { get; set; }






    }
}
