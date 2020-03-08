using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class CompanyModelBussines
    {

        public Int64 Rut { get; set; }

        
        public int Id { get; set; }

        [ForeignKey("Rut")]
        public Company Company { get; set; }

        [ForeignKey("Id")]
        public BusinessModel IdB { get; set; }

    }
}
