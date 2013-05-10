namespace Consoletest001.Sqlite.Entity
{
    //t_table
    public class TableInfo
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
        /// ownerid
        /// </summary>		
        private int _ownerid;
        public int ownerid
        {
            get { return _ownerid; }
            set { _ownerid = value; }
        }
        /// <summary>
        /// tableName
        /// </summary>		
        private string _tablename;
        public string tableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }
        /// <summary>
        /// sql
        /// </summary>		
        private string _sql;
        public string sql
        {
            get { return _sql; }
            set { _sql = value; }
        }
        /// <summary>
        /// filePath
        /// </summary>		
        private string _filepath;
        public string filePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

    }
}

