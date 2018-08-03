using LiteDB;
using Serilog;
using System;
using System.Collections.Generic;
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
        /// <param name="user">The user we want to create an account for.</param>
        /// <returns></returns>
        ResponseModel CreateAccount(User user);

        ResponseModel UpdateAccount(User user);

        User GetUserByEmail(string email);

        IEnumerable<User> GetUsers();

        void ChangePassword(User user, string newPasswordHash);

        void RemoveUser(string email);
    }

    public class AccountService : IAccountService
    {
        private readonly LiteRepository _db;

        public AccountService(LiteRepository liteRepository)
        {
            _db = liteRepository;
        }

        public ResponseModel CreateAccount(User user)
        {
            try
            {
                _db.Insert(user);
                return new ResponseModel(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new ResponseModel(false, ex);
            }
        }

        public ResponseModel UpdateAccount(User user)
        {
            try
            {
                _db.Update(user);
                return new ResponseModel(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new ResponseModel(false, ex);
            }
        }

        public void ChangePassword(User user, string newPassword)
        {
            if (user != null)
            {
                var (salt, hash) = PBKDF2.HashPassword(newPassword);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                _db.Update(user);
            }
        }

        public User GetUserByEmail(string email)
        {
            return _db.Query<User>().Where(u => u.Email == email).FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return _db.Query<User>().ToList();
        }

        public void RemoveUser(string email)
        {
            _db.Delete<User>(u => u.Email == email);
        }
    }
}