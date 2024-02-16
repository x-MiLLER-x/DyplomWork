using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Windows;

namespace DyplomWork_2._0_WPF_
{
    internal class DB
    {
        private static string connectionString = "mongodb+srv://Yevtushenko:yevtushenkocluster@yevtushenkocluster.iklei6s.mongodb.net/";

        private MongoClient client;
        private IMongoDatabase db;

        //connecting to db
        public DB()
        {
            try
            {
                client = new MongoClient(connectionString);
                db = client.GetDatabase("dyplomWork");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the database: {ex.Message}");
                return;
            }
        }
        //choosing collection
        public IMongoCollection<BsonDocument> GetCollection()
        {
            return db.GetCollection<BsonDocument>("users");
        }

        //get user
        public bool try_autorization(User us)
        {
            String loginUser = us.Login;
            String passUser = us.Pass;

            DB db = new DB();

            //Get access to the "users" collection
            IMongoCollection<BsonDocument> collection = db.GetCollection();

            //Create a filter document
            BsonDocument filter = new BsonDocument();

            filter = new BsonDocument("$and",
                            new BsonArray { new BsonDocument("login", loginUser),
                                 new BsonDocument("pass", passUser)});


            //To select data, use the find() function
            List<BsonDocument> users = collection.Find(filter).ToList();

            //If the user exists... 
            if (users.Any())
            {
                return true;
            }
            else
            {
                MessageBox.Show("Uncorrectly data");
                return false;
            }
        }
        
        //set user
        public bool try_registration(User us)
        {
            String loginUser = us.Login;
            String passUser = us.Pass;
            String emailUser = us.Email;

            DB db = new DB();

            //Get access to the "users" collection
            IMongoCollection<BsonDocument> collection = db.GetCollection();

            //Create a filter document to check the same login or email
            BsonDocument filter_login = new BsonDocument();
            BsonDocument filter_email = new BsonDocument();

            filter_login = new BsonDocument ("login", loginUser);

            filter_email = new BsonDocument ("email", emailUser);

            //To select data, use the find() function
            List<BsonDocument> login = collection.Find(filter_login).ToList();

            //If the login exists... 
            if (login.Any())
            {
                MessageBox.Show("The login is already in use.");
                return false;
            }

            //To select data, use the find() function
            List<BsonDocument> email = collection.Find(filter_email).ToList();

            //If the login exists... 
            if (email.Any())
            {
                MessageBox.Show("The email is already in use.");
                return false;
            }

            //If the test is successful, create a new user.
            BsonDocument user = new BsonDocument
            {
                {"login", loginUser},
                {"pass", passUser },
                { "email", emailUser},
            };

            //Insert in DB
            collection.InsertOne(user);
            return true;
        }
    }
}
