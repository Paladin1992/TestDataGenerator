using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Creates a user account in the database and returns a ResponseModel
        /// that contains information about the success of the operation,
        /// and the Exception itself, if one had occured.
        /// </summary>
        /// <param name="user">The user who we want to create an account for.</param>
        /// <returns></returns>
        ResponseModel CreateAccount(User user);

        User GetUserByEmail(string email);

        void ChangePassword(int userId, string newPasswordHash);

        User ForgottenPasswordRequest(string email);
    }

    public class AccountService : IAccountService
    {
        private readonly IDataService _dataService;

        public AccountService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void ChangePassword(int userId, string newPasswordHash)
        {
            throw new NotImplementedException();
        }

        public ResponseModel CreateAccount(User user)
        {
            try
            {
                _dataService.AddUser(user);
                return new ResponseModel(true);
            }
            catch (Exception ex)
            {
                return new ResponseModel(false, ex);
            }
        }

        public User ForgottenPasswordRequest(string email)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            return _dataService.GetUserByEmail(email);
        }
    }
}
