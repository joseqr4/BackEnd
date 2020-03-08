using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class CommerceDiscounts
    {

        public int CommerceID { get; set; }

        public int DiscountsID { get; set; }

        [ForeignKey("DiscountsID")]
        public Discounts Discounts { get; set; }

        [ForeignKey("CommerceID")]
        public Commerce Commerce { get; set; }


    }
}
