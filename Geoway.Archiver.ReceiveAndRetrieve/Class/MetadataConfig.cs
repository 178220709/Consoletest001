namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class MetadataConfig
    {
        private ADF.MIS.DataModel.MetaField _metaField;
        private string _configFileName;
        private object _value;

        /// <summary>
        /// 统一赋值
        /// </summary>
        [System.ComponentModel.DisplayName("统一赋值")]
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        
        /// <summary>
        /// 配置字段名称
        /// </summary>
        [System.ComponentModel.DisplayName("源数据项匹配项")]
        public string ConfigFileName
        {
            get { return _configFileName; }
            set { _configFileName = value; }
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        [System.ComponentModel.DisplayName("元数据项")]
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