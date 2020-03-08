using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Company
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Rut { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        public string email { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public String? Image { get; set; }

        [DefaultValue(false)]
        public Boolean Enable { get; set; }



    }
}
