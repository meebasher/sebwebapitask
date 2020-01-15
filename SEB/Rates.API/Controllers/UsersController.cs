using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rates.API.Extentions;
using Rates.API.Helpers;
using Rates.API.Model;
using Rates.API.ResourceParameters;
using Rates.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rates.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        #region Fields
        private readonly IUserLibraryRepository _userLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyCheckerService _propertyCheckerService;
        #endregion Fields

        public UsersController(IUserLibraryRepository userLibraryRepository, IMapper mapper, IPropertyCheckerService propertyCheckerService)
        {

            _userLibraryRepository = userLibraryRepository ?? throw new ArgumentNullException(nameof(userLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        #region GetRequsets
        //Get all users request
        [HttpGet(Name = "GetUsers")]
        [HttpHead]
        public IActionResult GetUsers([FromQuery]UsersResourceParameters usersResourceParameters)
        {
            //Checks weather or not users has a valid property
            if (!_propertyCheckerService.TypeHasProperties<UserDto>(usersResourceParameters.Fields))
            {
                //If not, returnb bad request
                return BadRequest();
            }
            //Get user from repository
            var usersFromRepo = _userLibraryRepository.GetUsers(usersResourceParameters);

            #region Paging

            //Check if there is a preveous page
            var previousPageLink = usersFromRepo.HasPrevious ?
               CreateUsersResourceUri(usersResourceParameters,
               ResourceUriType.PreviousPage) : null;
           
            //Check if there is a next page
            var nextPageLink = usersFromRepo.HasNext ?
                CreateUsersResourceUri(usersResourceParameters,
                ResourceUriType.NextPage) : null;

            //Creating metadata
            var paginationMetadata = new
            {
                totalCount = usersFromRepo.TotalCount,
                pageSize = usersFromRepo.PageSize,
                currentPage = usersFromRepo.CurrentPage,
                totalPages = usersFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            //Adding a custom header for a response
            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));
            #endregion Paging

            //Return result using data shaping
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo)
                .ShapeData(usersResourceParameters.Fields));
        }

        //Get user by Id request
        [HttpGet("{Id}", Name = "GetUser")]
        public IActionResult GetUser(ulong Id, string fields)
        {
            //Checks weather or not user has a valid property
            if (!_propertyCheckerService.TypeHasProperties<UserDto>(fields))
            {
                return BadRequest();
            }

            //Gett a user from repository by id
            var userFromRepo = _userLibraryRepository.GetUser(Id);

            //Check whether a user exists
            if (userFromRepo==null)
            {
                return NotFound();
            }

            //Return a user using data shaping
            return Ok(_mapper.Map<UserDto>(userFromRepo).ShapeData(fields));
        }
        #endregion GetRequsets

        #region PostRequest
        //Post user request
        [HttpPost]
        public ActionResult<UserDto> CreateUser(UserForCreationDto user)
        {
            //Map user from entites to a user for creation
            var userEntity = _mapper.Map<Entities.User>(user);
            //Add a user to repository
            _userLibraryRepository.AddUser(ref userEntity);
            //Save changes
            _userLibraryRepository.Save();
            //Map user from repo to UserDto
            var userToReturn = _mapper.Map<UserDto>(userEntity);
            return CreatedAtRoute("GetUser",
                new { Id = userToReturn.Id },
                userToReturn);
        }
        #endregion PostRequest

        #region OptionsRequest
        //Options user request
        [HttpOptions]
        public IActionResult GetUserOptions()
        {
            //Return options
            Response.Headers.Add("Allow", "GET,POST,DELETE,OPTIONS");
            return Ok();
        }
        #endregion OptionsRequest

        #region DeleteRequest
        //Delete user request
        [HttpDelete("Id")]
        public ActionResult DeleteUser(ulong Id)
        {
            //Gett a user from repository by id
            var userFromRepo = _userLibraryRepository.GetUser(Id);
            
            //Check whether required user exists
            if (userFromRepo==null)
            {
                return NotFound();
            }

            //Delete user from repository
            _userLibraryRepository.DeleteUser(userFromRepo);

            //Save changes
            _userLibraryRepository.Save();

            return NoContent();
        }
        #endregion DeleteRequest

        private string CreateUsersResourceUri(
            UsersResourceParameters usersResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUsers",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          pageNumber = usersResourceParameters.PageNumber - 1,
                          pageSize = usersResourceParameters.PageSize,
                          mainCategory = usersResourceParameters.LastName,
                          searchQuery = usersResourceParameters.SearchQuery
                      }); ;
                case ResourceUriType.NextPage:
                    return Url.Link("GetUsers",
                      new
                      {
                          fields = usersResourceParameters.Fields,
                          pageNumber = usersResourceParameters.PageNumber + 1,
                          pageSize = usersResourceParameters.PageSize,
                          mainCategory = usersResourceParameters.LastName,
                          searchQuery = usersResourceParameters.SearchQuery
                      });

                default:
                    return Url.Link("GetUsers",
                    new
                    {
                        fields = usersResourceParameters.Fields,
                        pageNumber = usersResourceParameters.PageNumber,
                        pageSize = usersResourceParameters.PageSize,
                        mainCategory = usersResourceParameters.LastName,
                        searchQuery = usersResourceParameters.SearchQuery
                    });
            }

        }
    }
}
