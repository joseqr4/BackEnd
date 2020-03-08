using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class UserCommerce
    {

       

       
        public string IdUser { get; set; }
        public int CommerceID { get; set; }

        [ForeignKey("CommerceID")]
        public Commerce Commerce { get; set; }

        [ForeignKey("IdUser")]
        public ApplicationUser User { get; set; }


    }
}
