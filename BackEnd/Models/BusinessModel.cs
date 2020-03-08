using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class BusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Descripcion { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public String? Image { get; set; }

        public int? CategoriID { get; set; }

        [ForeignKey("CategoriID")]
        public Category Category { get; set; }
    }
}
