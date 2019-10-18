using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class intereses
    {
        [Key]
        public int Codigo { get; set; }
        public string   Nombre { get; set; }


    }
}
