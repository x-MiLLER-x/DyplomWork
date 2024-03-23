using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Windows;
using DnsClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Diagnostics.Metrics;
using System.Windows.Media.Media3D;
using Microsoft.Graph.Models;

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
            catch (Exception)
            {
                MessageBox.Show("Error connecting to the database.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        //choosing collection
        public IMongoCollection<BsonDocument> GetCollection()
        {
            return db.GetCollection<BsonDocument>("users");
        }

        //update user (update data user from db)
        public User updateUsersDataFromDB(User us)
        {
            String loginUser = us.Login;

            DB db = new DB();

            //Get access to the "users" collection
            IMongoCollection<BsonDocument> collection = db.GetCollection();

            //Create a filter document to check the same login or email
            BsonDocument filter_login = new BsonDocument("login", loginUser);

            //To select data, use the find() function
            List<BsonDocument> userDocuments = collection.Find(filter_login).ToList();

            us.Email = userDocuments[0]["email"].AsString;
            us.ComboBoxSavingData = userDocuments[0]["comboBoxSavingData"].AsBoolean;
            us.AnySavedMeal = userDocuments[0]["anySavedMeal"].AsBoolean;
            if (us.ComboBoxSavingData)
            {
                us.Gender = userDocuments[0]["gender"].AsString;
                us.Age = userDocuments[0]["age"].AsInt32;
                us.Weight = userDocuments[0]["weight"].AsInt32;
                us.Height = userDocuments[0]["height"].AsInt32;
                us.Country = userDocuments[0]["country"].AsString;
            }
            if (us.AnySavedMeal)
            {
                us.Meals = userDocuments[0]["meals"].AsBsonArray.Select(x => x.AsString).ToList();
                us.Images = userDocuments[0]["images"].AsBsonArray.Select(x => x.AsString).ToList();
            }
            return us;
        }

        //replace user (change user in db)
        public bool try_saving_details(User us)
        {
            String loginUser = us.Login;
            String passUser = us.Pass;
            String gender = us.Gender;
            int age = us.Age;
            int weight = us.Weight;
            int height = us.Height;
            String country = us.Country;
            List<string> meals = us.Meals;
            List<string> images = us.Images;

            DB db = new DB();

            //Get access to the "users" collection
            IMongoCollection<BsonDocument> collection = db.GetCollection();

            //Create a filter document to check the same login or email
            BsonDocument filter_login = new BsonDocument("login", loginUser);

            //To select data, use the find() function
            List<BsonDocument> userDocuments;
            try
            {
                userDocuments = collection.Find(filter_login).ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error connecting to the database.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            us.Email = userDocuments[0]["email"].AsString;
            String emailUser = us.Email;
            BsonDocument user = new BsonDocument { };
            // Create a new user.
            if (us.AnySavedMeal)
            {
                user = new BsonDocument
            {
                {"login", loginUser},
                {"pass", passUser },
                {"email", emailUser},
                {"gender", gender },
                {"age", age },
                {"weight", weight },
                {"height", height },
                {"country", country },
                {"comboBoxSavingData", us.ComboBoxSavingData },
                {"anySavedMeal", us.AnySavedMeal },
                {"meals", new BsonArray(meals)},
                {"images", new BsonArray(images)}
        };
            }
            else
            {
                user = new BsonDocument
            {
                {"login", loginUser},
                {"pass", passUser },
                {"email", emailUser},
                {"gender", gender },
                {"age", age },
                {"weight", weight },
                {"height", height },
                {"country", country },
                {"comboBoxSavingData", us.ComboBoxSavingData },
                {"anySavedMeal", us.AnySavedMeal },
        };
            }

            //Replace in DB
            collection.ReplaceOne(filter_login, user);
            return true;
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

            List<BsonDocument> users;
            //To select data, use the find() function
            try
            {
                users = collection.Find(filter).ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error connecting to the database.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //If the user exists... 
            if (users.Any())
            {
                return true;
            }
            else
            {
                MessageBox.Show("Uncorrectly data", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        //set user
        public bool try_registration(User us)
        {
            String loginUser = us.Login;
            String passUser = us.Pass;
            String emailUser = us.Email;
            bool comboBoxSavingData = us.ComboBoxSavingData;

            DB db = new DB();

            //Get access to the "users" collection
            IMongoCollection<BsonDocument> collection = db.GetCollection();

            //Create a filter document to check the same login or email
            BsonDocument filter_login = new BsonDocument();
            BsonDocument filter_email = new BsonDocument();

            filter_login = new BsonDocument("login", loginUser);

            filter_email = new BsonDocument("email", emailUser);

            //To select data, use the find() function
            List<BsonDocument> login = collection.Find(filter_login).ToList();

            //If the login exists... 
            if (login.Any())
            {
                MessageBox.Show("The login is already in use.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //To select data, use the find() function
            List<BsonDocument> email = collection.Find(filter_email).ToList();

            //If the email exists... 
            if (email.Any())
            {
                MessageBox.Show("The email is already in use.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //If the test is successful, create a new user.
            BsonDocument user = new BsonDocument
            {
                {"login", loginUser},
                {"pass", passUser },
                {"email", emailUser},
                {"comboBoxSavingData", comboBoxSavingData }
            };

            //Insert in DB
            collection.InsertOne(user);
            return true;
        }
    }
}
