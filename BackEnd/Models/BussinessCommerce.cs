using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class BussinessCommerce
    {
        public int Bussines { get; set; }

        public int Commerce { get; set; }

        [ForeignKey("Company")]
        private Company Rut { get; set; }

        [ForeignKey("Bussines")]
        private BusinessModel rut { get; set; }
    }
}
