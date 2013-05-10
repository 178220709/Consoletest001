namespace Consoletest001.Sqlite.Entity
{
    public class PhoneInfo
    {
        public PhoneInfo(string name,string phoneNum,string homeNum)
        {
            this.Name = name;
            this.PhoneNumber = phoneNum;
            this.HomeNumber = homeNum;
        }
        public PhoneInfo()
        {
            this.Name = "";
            this.PhoneNumber = "";
            this.HomeNumber = "";
        }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string HomeNumber { get; set; }


    }
}
