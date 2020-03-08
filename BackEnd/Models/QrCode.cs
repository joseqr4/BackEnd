using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class QrCode
    {

       
        public int Id { get; set; }

        public int IdDiscount { get; set; }
        
        public int IdCommerce { get; set; }

        public string IdUser { get; set; }

        [ForeignKey("IdCommerce")]
        public Commerce Commerce { get; set; }

        [ForeignKey("IdDiscount")]
        public Discounts Discounts { get; set; }

        [ForeignKey("IdUser")]
        public ApplicationUser Email { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public String Image { get; set; }

        public DateTime TimeValidation { get; set; }

        public Boolean Consumed { get; set; }

        public Boolean Valued { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateConsumed { get; set; }


    }
}
