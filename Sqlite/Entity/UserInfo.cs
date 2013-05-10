namespace Consoletest001.Sqlite.Entity
{
    //t_user
    public class UserInfo
    {

        /// <summary>
        /// id
        /// </summary>		
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// userid
        /// </summary>		
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// password
        /// </summary>		
        private string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// username
        /// </summary>		
        private string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// email
        /// </summary>		
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// role
        /// </summary>		
        private int _role;
        public int role
        {
            get { return _role; }
            set { _role = value; }
        }

    }
}

