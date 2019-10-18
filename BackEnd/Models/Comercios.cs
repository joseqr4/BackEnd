using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Comercios
    {
        public int Id { get; set; }
        
        [StringLength(30)]
        public string Nombre { get; set; }


    }
}
