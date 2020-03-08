using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class InterestsUsers
    {
    
        public string IdUser { get; set; }

        public int InterestsId { get; set; }


        [ForeignKey("InterestsId")]
        public Interests Interests { get; set; }

        [ForeignKey("IdUser")]
        public ApplicationUser User { get; set; }


        //public List<ApplicationUser>? UserName { get; set; }


    }
}
