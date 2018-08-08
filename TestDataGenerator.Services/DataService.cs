using LiteDB;
using System.Collections.Generic;

namespace TestDataGenerator.Services
{
    public interface IDataService
    {
        IEnumerable<T> GetAll<T>();

        void RemoveAllRecords<T>(bool resetCounter);
    }

    public class DataService : IDataService
    {
        private readonly LiteRepository _db;

        public DataService(LiteRepository repo)
        {
            _db = repo;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _db.Query<T>().ToEnumerable();
        }

        public void RemoveAllRecords<T>(bool resetCounter = false)
        {
            if (resetCounter)
            {
                _db.Database.DropCollection(typeof(T).Name);
            }
            else
            {
                _db.Delete<T>(record => true);
            }
        }
    }
}