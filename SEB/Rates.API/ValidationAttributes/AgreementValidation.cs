using Rates.API.Enums;
using Rates.API.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.ValidationAttributes
{
    public class AgreementValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var agreement = (AgreementForCreationDto)validationContext.ObjectInstance;
            string agreementBaseRateCode = agreement.BaseRateCode.ToString().Trim();
            string enumBaseRateCode = BaseRateCodes.VILIBOR1m.ToString().Trim();
            int parsedIntValue=default;
            var margin = agreement.Margin.ToString().Replace(',','.');
            decimal parsedDecValue=default;
            bool agreementParsed = int.TryParse(agreement.Amount.ToString().Trim(), out parsedIntValue);
            bool durationParsed = int.TryParse(agreement.Duration.ToString().Trim(), out parsedIntValue);
            bool marginParsed = decimal.TryParse(margin , NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
    CultureInfo.InvariantCulture, out parsedDecValue);
            if (!agreementParsed)
            {
                return new ValidationResult("Amount must be an integer value", new[] { nameof(AgreementForCreationDto) });
            }
            if (!durationParsed)
            {
                return new ValidationResult("Duration must be an integer value", new[] { nameof(AgreementForCreationDto) });
            }
            if (!marginParsed)
            {
                return new ValidationResult("Margin must be a decimal value", new[] { nameof(AgreementForCreationDto) });
            }
            if (agreementBaseRateCode == BaseRateCodes.VILIBOR1m.ToString() 
                || agreementBaseRateCode == BaseRateCodes.VILIBOR1y.ToString()
                || agreementBaseRateCode == BaseRateCodes.VILIBOR3m.ToString()
                || agreementBaseRateCode == BaseRateCodes.VILIBOR6m.ToString())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Base rate code is not supported. Please, try different base rate code", new[] { nameof(AgreementForCreationDto) });
            }
        }
    }
}
