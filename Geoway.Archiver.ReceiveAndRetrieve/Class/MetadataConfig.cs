namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class MetadataConfig
    {
        private ADF.MIS.DataModel.MetaField _metaField;
        private string _configFileName;
        private object _value;

        /// <summary>
        /// ͳһ��ֵ
        /// </summary>
        [System.ComponentModel.DisplayName("ͳһ��ֵ")]
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        
        /// <summary>
        /// �����ֶ�����
        /// </summary>
        [System.ComponentModel.DisplayName("Դ������ƥ����")]
        public string ConfigFileName
        {
            get { return _configFileName; }
            set { _configFileName = value; }
        }
        /// <summary>
        /// �ֶ�����
        /// </summary>
        [System.ComponentModel.DisplayName("Ԫ������")]
        public ADF.MIS.DataModel.MetaField MetaField
        {
            get { return _metaField; }
            set { _metaField = value; }
        }

        public override string ToString()
        {
            return _metaField.AliasName;
        }


    }
}