using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Models;
using MongoDB.Driver;

namespace efcoremongodb.Services
{
    public class RestaurantDbContext 
    {
        private readonly IMongoDatabase _database;

        public RestaurantDbContext(IMongoDatabase database)
        {
            _database = database;
        }
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("comments");
        public IMongoCollection<Branch> Branches => _database.GetCollection<Branch>("branches"); // Added Branch collection
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products"); // Added Product collection
        public IMongoCollection<BranchProduct> BranchProducts => _database.GetCollection<BranchProduct>("branch products"); // Added Product collection
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("orders"); // Added Order collection
    }
}
