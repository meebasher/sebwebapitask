using AutoMapper;
using Rates.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Profiles
{
    public class AgreemetsProfile : Profile
    {
        /// <summary>
        /// This is configuration for a mapper
        /// So mapper could map properties correctly
        /// </summary>
        public AgreemetsProfile()
        {
            CreateMap<Entities.Agreement, Model.AgreementDto>()
                .ForMember(dest => dest.BaseRateCode,
                memberOptions => memberOptions
                .MapFrom(sourceMember => DetermineNewBaseRateCode(sourceMember.NewBaseRateCode,sourceMember.BaseRateCode)))
                .ForMember(dest => dest.CurrentInterestRate,
                memberOptions => memberOptions
                .MapFrom(sourceMember => InterestRateGetter.CalculateInterestRate(DetermineNewBaseRateCode(sourceMember.NewBaseRateCode, sourceMember.BaseRateCode), sourceMember.Margin)))
                .ForMember(dest => dest.InitialInterestRate,
                memberOptions => memberOptions
                .MapFrom(sourceMember => InterestRateGetter.CalculateInterestRate(sourceMember.BaseRateCode,sourceMember.Margin)))
                .AfterMap((src, dest) => dest.InterestRateDifference = dest.CurrentInterestRate - dest.InitialInterestRate);
            CreateMap<Model.AgreementForCreationDto, Entities.Agreement>();
            CreateMap<Model.AgreementForUpdateDto, Entities.Agreement>();
            CreateMap<Entities.Agreement, Model.AgreementForUpdateDto>();
        }

        /// <summary>
        /// Determines whether base rate code were updated or not
        /// </summary>
        /// <param name="newBaseRateCode">Updated base rate code</param>
        /// <param name="InitialBaseRateCode">Initial base rate code</param>
        /// <returns>newest base rate code</returns>
        private string DetermineNewBaseRateCode(string newBaseRateCode, string InitialBaseRateCode)
        {
            return string.IsNullOrEmpty(newBaseRateCode) ? InitialBaseRateCode : newBaseRateCode;
        }

        

    }
}
