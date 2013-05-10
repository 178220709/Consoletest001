using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class MetaDataReader
    {
        #region �ֶ�

        private EnumImageMetaFormat _enumImageFormat;     
        private string _metaFileFullName = "";          
        private Dictionary<string, string> _fieldAndValuePairs = null;            
        #endregion

        #region ����

        //��ȡԪ�����ļ����ݼ���
        public Dictionary<string, string> FieldAndValuePairs
        {
            get
            {
                parseMeta();
                return _fieldAndValuePairs;
            }
        }
        //��ȡԪ�����ļ�·��
        public string MetaFileFullName
        {
            set 
            { 
                _metaFileFullName = value;
                SetMetaFileFullName(_metaFileFullName);
            }
        }
        //���û��ȡԪ�����ļ���ʽ
        public EnumImageMetaFormat ImageMetaFormat
        {
            get { return _enumImageFormat; }
            //set { _enumImageFormat = value; }
        }

        #endregion

        #region ��������

        #endregion

        #region ˽�з���

        private void SetMetaFileFullName(string metaFileFullName)
        {
            //����Ԫ�����ļ�·�����ж��Ƿ���Ҫ����Ԫ�����ļ�
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

        //����Ԫ�����ļ����ݳɼ�����ʽ
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

        //�����ı���ʽԪ�����ļ�
        private void parseTextMetaFile()
        {
            ParseFileContent2Pairs parse = new ParseFileContent2Pairs();
            _fieldAndValuePairs = parse.ParseTextFileContent2Pairs(_metaFileFullName);
        }

        //����Excel����ʽԪ�����ļ�
        private void parseExcelMetaFile()
        {
            ParseFileContent2Pairs parse = new ParseFileContent2Pairs();
            _fieldAndValuePairs = parse.ParseExcelFileContent2Pairs(_metaFileFullName);
        }

        //����XML��ʽԪ�����ļ�
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
