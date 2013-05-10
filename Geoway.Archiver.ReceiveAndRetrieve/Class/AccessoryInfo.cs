using System.ComponentModel;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class AccessoryInfo
    {
        public int _id;

        [DisplayName("���")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [DisplayName("��������")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _path;
        [DisplayName("����·��"), Browsable(false)]
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}