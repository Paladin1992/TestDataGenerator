using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    public interface IDataService
    {
        void AddUser(User user);

        User GetUserByEmail(string email);

        IEnumerable<User> GetUsers();

        void RemoveUser(string email);

        void CreateSetup();

        void UpdateSetup();

        void DeleteSetup();
    }

    public class DataService : IDataService
    {
        private readonly LiteRepository _db;

        public DataService(LiteRepository repo)
        {
            _db = repo;
        }

        public void AddUser(User user)
        {
            _db.Insert(user);
        }

        public void CreateSetup()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetup()
        {
            throw new NotImplementedException();
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

        public void UpdateSetup()
        {
            throw new NotImplementedException();
        }
    }
}
