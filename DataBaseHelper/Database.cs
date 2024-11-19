using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoBackEnd.Models;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace MongoBackEnd.DataBaseHelper
{
    public class Database
    {
        /// <summary>
        /// Obtiene la informacion de los usuarios en la base de datos
        /// </summary>
        /// <returns></returns>
        public List<User> getUsers()
        {
            IMongoDatabase db = initDB();

            var users = db.GetCollection<BsonDocument>("Users");

            List<BsonDocument> userArray = users.Find(new BsonDocument()).ToList();

            List<User> userList = new List<User>();

            foreach (BsonDocument bsonUser in userArray)
            {
                User user = BsonSerializer.Deserialize<User>(bsonUser);
                userList.Add(user);
            }
            return userList;
        }

        public User findUser(string _id)
        {
            IMongoDatabase db = initDB();

            var users = db.GetCollection<BsonDocument>("Users");

            var filter = new BsonDocument
                {
                    { "_id", MongoDB.Bson.ObjectId.Parse(_id)}
                };

            BsonDocument userFound = users.Find(filter).First();

            User user = BsonSerializer.Deserialize<User>(userFound);
            user.idStr = user._id.ToString();
            return user;
        }

        private static IMongoDatabase initDB()
        {
            MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");

            IMongoDatabase db = mongoClient.GetDatabase("mongoBackEnd");
            return db;
        }

        public void insertUser(User user)
        {
            IMongoDatabase db = initDB();

            var users = db.GetCollection<BsonDocument>("Users");

            var doc = new BsonDocument
            {
                {"name",user.name},
                {"email",user.email},
                {"phone",user.phone},
                {"address",user.address},
                {"dateIn",user.dateIn},
            };

            users.InsertOne(doc);
        }

        public void deleteUser(string _id)
        {
            if (_id != null)
            {
                IMongoDatabase db = initDB();

                var users = db.GetCollection<BsonDocument>("Users");

                var filter = new BsonDocument
                {
                    { "_id", MongoDB.Bson.ObjectId.Parse(_id)}
                };
                var res = users.DeleteOne(filter);
            }
        }

        internal void updateUser(User user)
        {
            IMongoDatabase db = initDB();

            var users = db.GetCollection<BsonDocument>("Users");

            var doc = Builders<BsonDocument>.Update.Set("name", user.name).Set("email", user.email).
                Set("phone", user.phone).Set("address", user.address).Set("dateIn", user.dateIn);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(user.idStr));

            var res = users.UpdateOne(filter, doc);
        }
    }
}

