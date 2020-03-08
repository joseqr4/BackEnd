using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class DiscountsInterests
    {

        public int DiscountsID { get; set; }

        public int InterestsId { get; set; }

        [ForeignKey("DiscountsID")]
        public Discounts Discounts { get; set; }

        [ForeignKey("InterestsId")]
        public Interests INteres { get; set; }


    }
}
