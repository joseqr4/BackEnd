using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Interests
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        //private List<InterestsUsers>? InterestsUser { get; set; }

    
    }
}
