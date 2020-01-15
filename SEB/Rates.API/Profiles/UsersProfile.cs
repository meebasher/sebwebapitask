using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Profiles
{
    public class UsersProfile:Profile
    {
        /// <summary>
        /// This is configuration for a mapper
        /// So mapper could map properties correctly
        /// </summary>
        public UsersProfile()
        {
            CreateMap<Entities.User, Model.UserDto>()
                .ForMember(dest => dest.Name,
                memberOptions => memberOptions
                .MapFrom(sourceMember => $"{sourceMember.FirstName} {sourceMember.LastName}"));

            CreateMap<Model.UserForCreationDto, Entities.User>();
        }
    }
}
