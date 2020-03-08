using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class UserDiscountConsumed
    {
        public int Id { get; set; }
        public string UserNameID { get; set; }

        public int DiscountID { get; set; }

        public int CommerceId { get; set; }

        [ForeignKey("DiscountID")]
        public Discounts Discount { get; set; }

        [ForeignKey("UserNameID")]
        public ApplicationUser UserName { get; set; }

        [ForeignKey("CommerceId")]
        public Commerce Commerce { get; set; }

        public DateTime? DateConsumed { get; set; }

    }
}
