using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Commerce
    {
        public int Id { get; set; }

        [StringLength(40)]
        [Required]
        public string Alias { get; set; }
        [Required]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Phone { get; set; }

        //public List<Discounts>? Discounts { get; set; }

        //[Required]
        //public Company IdCompany { get; set; }

        //[Required]
        //public Category IdCategory { get; set; }


    }
}
