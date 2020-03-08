using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Discounts
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount_value { get; set; }

        //Value _ Percentage
        public string  Discount_Type { get; set; }

        public DateTime Date_start { get; set; }

        public DateTime Date_end { get; set; }




    }
}
