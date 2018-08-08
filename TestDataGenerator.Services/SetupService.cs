using LiteDB;
using System.Collections.Generic;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    public interface ISetupService
    {
        IEnumerable<UserSetup> GetAll();

        void CreateSetup(UserSetup userSetup);

        void UpdateSetup(UserSetup userSetup);

        void AddOrUpdateSetup(UserSetup userSetup);
    }

    public class SetupService : ISetupService
    {
        private readonly LiteRepository _db;

        public SetupService(LiteRepository repo)
        {
            _db = repo;
        }

        public IEnumerable<UserSetup> GetAll()
        {
            return _db.Query<UserSetup>().ToEnumerable();
        }

        public void CreateSetup(UserSetup userSetup)
        {
            _db.Insert(userSetup);
        }

        public void UpdateSetup(UserSetup userSetup)
        {
            _db.Update(userSetup);
        }

        public void AddOrUpdateSetup(UserSetup userSetup)
        {
            var setup = _db.Query<UserSetup>().Where(s => s.Id == userSetup.Id).FirstOrDefault();

            if (setup == null) // does not exist -> add
            {
                CreateSetup(userSetup);
            }
            else // exists -> update
            {
                UpdateSetup(userSetup);
            }
        }
    }
}
