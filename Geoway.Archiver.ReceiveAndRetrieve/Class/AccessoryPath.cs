namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using System.Collections.Generic;

    public class AccessoryPath
    {
        private List<AccessoryInfo> _lstAccessoryInfoes = new List<AccessoryInfo>();
        
        public List<AccessoryInfo> LstAccessoryInfoes
        {
            get { return _lstAccessoryInfoes; }
            set { _lstAccessoryInfoes = value; }
        }

        public AccessoryPath(List<AccessoryInfo> lstAccessoryInfoes)
        {
            _lstAccessoryInfoes = lstAccessoryInfoes;
        }

        public override string ToString()
        {
            List<string> lstPathes = new List<string>();
            foreach (AccessoryInfo accessoryInfo in _lstAccessoryInfoes)
            {
                lstPathes.Add(accessoryInfo.Path);
            }

            return string.Join(",", lstPathes.ToArray());
        }
    }
}