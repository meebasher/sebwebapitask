using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rates.API.Entities
{
    public class User
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public ICollection<Agreement> Agreements { get; set; }
            = new List<Agreement>();
    }
}
