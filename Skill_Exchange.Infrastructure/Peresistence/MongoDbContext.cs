using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Infrastructure.Peresistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Users");
        
        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
    }
}