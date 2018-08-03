using LiteDB;
using System.Collections.Generic;

namespace TestDataGenerator.Services
{
    public interface IDataService
    {
        IEnumerable<T> GetAll<T>();

        bool RemoveAllRecords<T>();
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

        public bool RemoveAllRecords<T>()
        {
            return _db.Database.DropCollection(typeof(T).Name);
        }
    }
}