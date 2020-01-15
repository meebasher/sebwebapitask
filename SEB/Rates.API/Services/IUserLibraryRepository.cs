using Rates.API.Entities;
using Rates.API.Helpers;
using Rates.API.Model;
using Rates.API.ResourceParameters;
using System;
using System.Collections.Generic;

namespace Rates.API.Services
{
    public interface IUserLibraryRepository
    {
        /// <summary>
        /// Add a collection of agreements to a specified user
        /// </summary>
        /// <param name="userId">users id</param>
        /// <returns>Agreements</returns>
        IEnumerable<Agreement> GetAgreements(ulong userId);
        /// <summary>
        /// Get specified agreement
        /// </summary>
        /// <param name="userId">users id</param>
        /// <param name="agreementId">agreements id</param>
        /// <returns></returns>
        Agreement GetAgreement(ulong userId, ulong agreementId);
        /// <summary>
        /// Add agreement to a specified user
        /// </summary>
        /// <param name="userId">useres id</param>
        /// <param name="agreement">agreement</param>
        void AddAgreement(ulong userId, Agreement agreement);
        /// <summary>
        /// Update a specified agreement with values from agreementForUpdateDto
        /// </summary>
        /// <param name="agreement">destination</param>
        /// <param name="agreementForUpdateDto">source</param>
        void UpdateAgreement(Agreement agreement, AgreementForUpdateDto agreementForUpdateDto);
        /// <summary>
        /// Delete a specified agreement
        /// </summary>
        /// <param name="agreement"></param>
        void DeleteAgreement(Agreement agreement);
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();
        /// <summary>
        /// Get paged users
        /// </summary>
        /// <param name="usersResourceParameters">metadata for paging</param>
        /// <returns></returns>
        PagedList<User> GetUsers(UsersResourceParameters usersResourceParameters);
        /// <summary>
        /// Get specified user
        /// </summary>
        /// <param name="userId">users id</param>
        /// <returns></returns>
        User GetUser(ulong userId);
        /// <summary>
        /// Get a collection of specified users
        /// </summary>
        /// <param name="userIds">collection containing users id</param>
        /// <returns></returns>
        IEnumerable<User> GetUsers(IEnumerable<ulong> userIds);
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user">user</param>
        void AddUser(ref User user);
        /// <summary>
        /// Delete specified user
        /// </summary>
        /// <param name="user">user</param>
        void DeleteUser(User user);
        /// <summary>
        /// Update specified user
        /// </summary>
        /// <param name="user">user</param>
        void UpdateUser(User user);
        /// <summary>
        /// Checks whether a specified user exists or not
        /// </summary>
        /// <param name="userId">user</param>
        /// <returns>>whether user exists</returns>
        bool UserExists(ulong userId);
        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>whether chages were saved</returns>
        bool Save();
    }
}
