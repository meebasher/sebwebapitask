using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Model
{
    public class UserForCreationDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public ICollection<AgreementForCreationDto> Agreements { get; set; }
        = new List<AgreementForCreationDto>();
    }
}
