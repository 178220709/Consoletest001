using System.ComponentModel;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class AccessoryInfo
    {
        public int _id;

        [DisplayName("序号")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [DisplayName("附件名称")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _path;
        [DisplayName("附件路径"), Browsable(false)]
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}