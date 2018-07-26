using LiteDB;
using System;

namespace TestDataGenerator.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [BsonField("name")]
        public string Name { get; set; }

        [BsonField("email")]
        public string Email { get; set; }

        [BsonField("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonField("passwordSalt")]
        public string PasswordSalt { get; set; }

        [BsonField("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
