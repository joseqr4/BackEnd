using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class UsersCompany
    {
       [Key]
        public string IdUser { get;set;}

        public Int64 idCompany { get; set; }

        [ForeignKey("idCompany")]
        public Company Company { get; set; }

        [ForeignKey("IdUser")]
        public ApplicationUser Email { get; set; }

        [DefaultValue(false)]
        public Boolean Enable { get; set; }

    }
}
