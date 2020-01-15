using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Rates.API.Model;
using Rates.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/agreements")]
    public class AgreementsController : ControllerBase
    {
        #region Fields
        private readonly IUserLibraryRepository _userLibraryRepository;
        private readonly IMapper _mapper;
        #endregion Fields
        public AgreementsController(IUserLibraryRepository agreementLibraryRepository, IMapper mapper)
        {
            _userLibraryRepository = agreementLibraryRepository ?? 
                throw new ArgumentNullException(nameof(agreementLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region GetRequests
        [HttpGet]
        public ActionResult<IEnumerable<AgreementDto>> GetAgreementsFromUser(ulong userId)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }
            var usersAgreementsFromRepo = _userLibraryRepository.GetAgreements(userId);
            return Ok(_mapper.Map<IEnumerable<AgreementDto>>(usersAgreementsFromRepo));
        }

        [HttpGet("{agreementId}", Name = "GetAgreementForUser")]
        public ActionResult<AgreementDto> GetAgreementFromUser(ulong userId, ulong agreementId)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }
            var usersAgreementFromRepo = _userLibraryRepository.GetAgreement(userId, agreementId);
            if (usersAgreementFromRepo==null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AgreementDto>(usersAgreementFromRepo));
        }
        #endregion GetRequests

        #region PostRequest
        [HttpPost]
        public ActionResult<AgreementDto> CreateAgreementForUser(ulong userId, AgreementForCreationDto agreement)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var agreementEntity = _mapper.Map<Entities.Agreement>(agreement);
            _userLibraryRepository.AddAgreement(userId, agreementEntity);
            _userLibraryRepository.Save();

            var agreementToReturn = _mapper.Map<AgreementDto>(agreementEntity);
            return CreatedAtRoute("GetAgreementForUser",
                new { userId = userId, agreementId = agreementToReturn.Id }, agreementToReturn);
        }
        #endregion PostRequest

        #region PutRequest
        [HttpPut("{agreementId}")]
        public IActionResult UpdateAgreementForUser(ulong userId, ulong agreementId, AgreementForUpdateDto agreementForUpdate)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var agreementForUserFromRepo = _userLibraryRepository.GetAgreement(userId, agreementId);

            if (agreementForUserFromRepo==null)
            {
                var agreementToAdd = _mapper.Map<Entities.Agreement>(agreementForUpdate);
                agreementToAdd.Id = agreementId;

                _userLibraryRepository.AddAgreement(userId, agreementToAdd);

                _userLibraryRepository.Save();

                var agreementToReturn = _mapper.Map<Entities.Agreement>(agreementForUpdate);

                return CreatedAtRoute("GetAgreementForUser",
                new { agreementId = agreementToAdd.Id, userId, agreementToReturn });
            }

            _userLibraryRepository.UpdateAgreement(agreementForUserFromRepo, agreementForUpdate);

            _userLibraryRepository.Save();

            return NoContent();
        }
        #endregion PutRequest

        #region PatchRequest
        [HttpPatch("{agreementId}")]
        public ActionResult PartiallyUpdateAgreement(ulong userId, ulong agreementId, JsonPatchDocument<AgreementForUpdateDto> patchDocument)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var agreementFromRepo = _userLibraryRepository.GetAgreement(userId, agreementId);

            if (agreementFromRepo == null)
            {
                return NotFound();
            }

            var agreementToPatch = _mapper.Map<AgreementForUpdateDto>(agreementFromRepo);
            
            patchDocument.ApplyTo(agreementToPatch,ModelState);

            if (!TryValidateModel(agreementToPatch))
            {
                return ValidationProblem(ModelState);
            }

            //_mapper.Map(agreementToPatch, agreementFromRepo);

            _userLibraryRepository.UpdateAgreement(agreementFromRepo, agreementToPatch);

            _userLibraryRepository.Save();

            return NoContent(); 
        }
        #endregion PatchRequests

        #region DeleteRequest
        [HttpDelete("userId")]
        public ActionResult DeleteAgreementForUser(ulong userId, ulong agreementId)
        {
            if (!_userLibraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var agreementForUserFromRepo = _userLibraryRepository.GetAgreement(userId, agreementId);

            if (agreementForUserFromRepo==null)
            {
                return NotFound();
            }

            _userLibraryRepository.DeleteAgreement(agreementForUserFromRepo);
            _userLibraryRepository.Save();

            return NoContent();
        }
        #endregion DeleteRequest

        #region OptionsRequest
        //Options user request
        [HttpOptions]
        public IActionResult GetAgreementOptions()
        {
            //Return options
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            return Ok();
        }
        #endregion OptionsRequest
    }
}
