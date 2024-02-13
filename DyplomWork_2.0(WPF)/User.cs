using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyplomWork_2._0_WPF_
{
    internal class User
    {
        private string login, pass, email;

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

        public User() { }

        public User(string login, string pass, string email) 
        {
            this.login = login;
            this.pass = pass;   
            this.email = email; 
        }
    }
}
