using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Model
{
    public class AgreementDto
    {

        public int Id { get; set; }
        public int Amount { get; set; }
        public string BaseRateCode { get; set; }
        public decimal Margin { get; set; }
        public int Duration { get; set; }
        public decimal CurrentInterestRate { get; set; }
        public decimal InitialInterestRate { get; set; }
        public decimal InterestRateDifference { get; set; }
    }
}
