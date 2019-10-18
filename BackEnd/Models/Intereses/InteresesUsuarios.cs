using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Intereses
{
    public class InteresesUsuarios
    {
       
        public ApplicationUser Usuario { get; set; }
 
            [ForeignKey("InteresesFK")]
        public intereses Intereses { get; set; }
        public int InteresesFK { get; set; }


    }
}
