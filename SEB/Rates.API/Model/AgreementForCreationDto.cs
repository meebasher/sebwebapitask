using Rates.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Model
{
    [AgreementValidation]
    public class AgreementForCreationDto
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string BaseRateCode { get; set; }
        [Required]
        public decimal Margin { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
