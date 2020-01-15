using Rates.API.DbContexts;
using Rates.API.Entities;
using Rates.API.Helpers;
using Rates.API.Model;
using Rates.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rates.API.Services
{
    public class UserLibraryRepository : IUserLibraryRepository, IDisposable
    {
        private readonly UserLibraryContext _context;

        public UserLibraryRepository(UserLibraryContext context )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAgreement(ulong userId, Agreement agreement)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (agreement == null)
            {
                throw new ArgumentNullException(nameof(agreement));
            }
            // always set the UserId to the passed-in userId
            agreement.UserId = userId;
            ulong lastAgreementId = _context.Agreements.Max(u => u.Id);
            agreement.Id = lastAgreementId + 1;
            _context.Agreements.Add(agreement); 
        }         

        public void DeleteAgreement(Agreement agreement)
        {
            _context.Agreements.Remove(agreement);
        }
  
        public Agreement GetAgreement(ulong userId, ulong agreementId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (agreementId <= 0)
            {
                throw new ArgumentNullException(nameof(agreementId));
            }

            return _context.Agreements
              .Where(c => c.UserId == userId && c.Id == agreementId).FirstOrDefault();
        }

        public IEnumerable<Agreement> GetAgreements(ulong userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return _context.Agreements
                        .Where(c => c.UserId == userId)
                        .OrderBy(c => c.Amount).ToList();
        }

        public void UpdateAgreement(Agreement agreement, AgreementForUpdateDto agreementForUpdateDto)
        {
            agreement.Amount = agreementForUpdateDto.Amount;
            agreement.Duration = agreementForUpdateDto.Duration;
            agreement.BaseRateCode = !string.IsNullOrEmpty(agreementForUpdateDto.BaseRateCode) && !string.IsNullOrEmpty(agreement.NewBaseRateCode)
                ? agreement.NewBaseRateCode
                : agreement.BaseRateCode;
            agreement.NewBaseRateCode = !string.IsNullOrEmpty(agreementForUpdateDto.BaseRateCode) ? agreementForUpdateDto.BaseRateCode : agreement.NewBaseRateCode;
        }

        public void AddUser(ref User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var lastUserId = _context.Users.Max(u => u.Id);
            user.Id = lastUserId + 1;

            foreach (var agreement in user.Agreements)
            {
                var lasagreementId = _context.Agreements.Max(u => u.Id);
                agreement.Id = lastUserId + 1;
            }

            _context.Users.Add(user);
        }

        public bool UserExists(ulong userId)
        {
            if (userId <= 0) 
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.Any(a => a.Id == userId);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Remove(user);
        }
        
        public User GetUser(ulong userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.FirstOrDefault(a => a.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList<User>();
        }

        public PagedList<User> GetUsers(UsersResourceParameters usersResourceParameters)
        {
            if (usersResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(usersResourceParameters));
            }

            var users = _context.Users as IQueryable<User>;
            if (!string.IsNullOrWhiteSpace(usersResourceParameters.LastName))
            {
                var lastName = usersResourceParameters.LastName.Trim().ToLower();
                users = _context.Users.Where(u => u.LastName.ToLower() == lastName);
            }
            if (!string.IsNullOrWhiteSpace(usersResourceParameters.SearchQuery))
            {
                var searchQuery = usersResourceParameters.SearchQuery.Trim();
                users = users.Where(u => u.FirstName.Contains(searchQuery) 
                || u.LastName.Contains(searchQuery));
            }
            return PagedList<User>.Create(users, usersResourceParameters.PageNumber, usersResourceParameters.PageSize);
        }

        public IEnumerable<User> GetUsers(IEnumerable<ulong> userIds)
        {
            if (userIds == null)
            {
                throw new ArgumentNullException(nameof(userIds));
            }

            return _context.Users.Where(a => userIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateUser(User userId)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
