using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class MetaDataReader
    {
        #region 字段

        private EnumImageMetaFormat _enumImageFormat;     
        private string _metaFileFullName = "";          
        private Dictionary<string, string> _fieldAndValuePairs = null;            
        #endregion

        #region 属性

        //获取元数据文件内容键对
        public Dictionary<string, string> FieldAndValuePairs
        {
            get
            {
                parseMeta();
                return _fieldAndValuePairs;
            }
        }
        //获取元数据文件路径
        public string MetaFileFullName
        {
            set 
            { 
                _metaFileFullName = value;
                SetMetaFileFullName(_metaFileFullName);
            }
        }
        //设置或获取元数据文件格式
        public EnumImageMetaFormat ImageMetaFormat
        {
            get { return _enumImageFormat; }
            //set { _enumImageFormat = value; }
        }

        #endregion

        #region 公开方法

        #endregion

        #region 私有方法

        private void SetMetaFileFullName(string metaFileFullName)
        {
            //更改元数据文件路径并判断是否需要解析元数据文件
            if (metaFileFullName == null || metaFileFullName.Trim() == "")
            {
                _fieldAndValuePairs = null;
            }
            else
            {
                //if (_metaFileFullName.Trim().ToUpper() != metaFileFullName.Trim().ToUpper())

                _enumImageFormat = getMetaFormat(metaFileFullName);
            }
        }

        //解析元数据文件内容成键对形式
        private void parseMeta()
        {
            if (_fieldAndValuePairs != null)
            {
                return;
            }
            else
            {
                if (_fieldAndValuePairs == null)
                    _fieldAndValuePairs = new Dictionary<string, string>();

                switch (_enumImageFormat)
                {
                    case EnumImageMetaFormat.Text:
                        parseTextMetaFile();
                        break;
                    case EnumImageMetaFormat.Excel:
                        parseExcelMetaFile();
                        break;
                    case EnumImageMetaFormat.Xml:
                        parseXMLMetaFile();
                        break;
                    default:
                        break;
                }

            }
        }

        //解析文本格式元数据文件
        private void parseTextMetaFile()
        {
            ParseFileContent2Pairs parse = new ParseFileContent2Pairs();
            _fieldAndValuePairs = parse.ParseTextFileContent2Pairs(_metaFileFullName);
        }

        //解析Excel表格格式元数据文件
        private void parseExcelMetaFile()
        {
            ParseFileContent2Pairs parse = new ParseFileContent2Pairs();
            _fieldAndValuePairs = parse.ParseExcelFileContent2Pairs(_metaFileFullName);
        }

        //解析XML格式元数据文件
        private void parseXMLMetaFile()
        {
            ParseFileContent2Pairs parse = new ParseFileContent2Pairs();
            _fieldAndValuePairs = parse.ParseXmlFileContent2Pairs(_metaFileFullName);
            
        }

        private EnumImageMetaFormat getMetaFormat(string metaFileFullName)
        {
            string extension = System.IO.Path.GetExtension(metaFileFullName).ToUpper();
            EnumImageMetaFormat format;
            switch (extension)
            {
                case ".XLS":
                    format = EnumImageMetaFormat.Excel;
                    break;
                case ".TXT":
                case ".MAT":
                case ".MET":
                case ".DOC":
                    format = EnumImageMetaFormat.Text;
                    break;
                case ".XML":
                    format = EnumImageMetaFormat.Xml;
                    break;
                default:
                    format = EnumImageMetaFormat.None;
                    break;
            }
            return format;
        }

        #endregion
    }
}
