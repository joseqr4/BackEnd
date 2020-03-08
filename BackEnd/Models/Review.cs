using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string IdUsers { get; set; }

        public int IdDiscount { get; set; }

        public int IDCommerce { get; set; }

        [ForeignKey("IdDiscount")]
        private Discounts Discounts { get; set; }
        
        [ForeignKey("IdUser")]
        private ApplicationUser Email { get; set; }

        [ForeignKey("IDCommerce")]
        private Commerce Commerces { get; set; }

        [Column(TypeName = "decimal(2,1)")]
        [DefaultValue(0)]
        public Decimal Value { get; set; }

        [Column(TypeName = "decimal(2,1)")]
        [DefaultValue(0)]
        public Decimal Article { get; set; }

        [Column(TypeName = "decimal(2,1)")]
        [DefaultValue(0)]
        public Decimal CommerceValued { get; set; }

        public DateTime DateValoration { get; set; }


    }
}
