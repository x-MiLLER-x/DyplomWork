using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyplomWork_2._0_WPF_
{
    public class User
    {
        private string login, pass, email, gender, country;
        private int  age, weight, height;
        private bool comboBoxSavingData;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public bool ComboBoxSavingData
        {
            get { return comboBoxSavingData; }
            set { comboBoxSavingData = value; }
        }

        public User() { }

        public User(string login, string pass, string email, bool comboBoxSavingData) 
        {
            this.login = login;
            this.pass = pass;   
            this.email = email; 
            this.comboBoxSavingData = comboBoxSavingData; 
        }
        public User(string login, string pass, string email, string gender, int age, int weight, int height, string country)
        {
            this.login = login;
            this.pass = pass;
            this.email = email;
            this.gender = gender;
            this.age = age;
            this.weight = weight;
            this.height = height;
            this.country = country;
        }
    }
}
