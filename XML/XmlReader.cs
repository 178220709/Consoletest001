using System;
using System.Windows.Forms;
using System.Xml;

namespace DemoXmlReader
{
    class XmlReader : IDisposable
    {
        private string xmlPath;
        private const string ERRMSG = "Error ocurred While Reading";
        private ListBox listbox;
        private ComboBox combobox;
        private XmlTextReader xtr;

        #region XmlReader������
        public XmlReader()
        {
            this.xmlPath = string.Empty;
            this.xtr = null;
            this.listbox = null;
        }
        public XmlReader(string xmlPath, ListBox listbox)
        {
            this.xmlPath = xmlPath;
            this.listbox = listbox;

        }

        public XmlReader(string xmlPath, ComboBox combobox)
        {
            this.combobox = combobox;
            this.xmlPath = xmlPath;
        }
        #endregion

        #region XmlReader ��Դ�ͷŷ���
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (this.xtr != null)
            {

                this.xtr.Close();
                this.xtr = null;
            }
            if (this.xmlPath != null)
            {
                this.xmlPath = null;

            }
        }
        #endregion

        #region XmlReader ������
        public ListBox listBox
        {
            get { return listbox; }
            set { listbox = value; }
        }
        public ComboBox comboBox
        {
            get { return combobox; }
            set { combobox = value; }
        }
        public string XmlPath
        {
            get { return xmlPath; }
            set { xmlPath = value; }
        }
        #endregion

        #region ����xml�ļ�
        public void EachXmlToListBox()
        {
            listbox.Items.Clear();
            xtr = new XmlTextReader(xmlPath);
            try
            {

                while (xtr.Read())
                {
                    if (xtr.NodeType != XmlNodeType.XmlDeclaration)
                        listbox.Items.Add(xtr.Value);

                }

            }
            catch (XmlException xe)
            {
                throw new XmlException(ERRMSG + xe.Message);
            }
            finally
            {

                if (xtr != null)
                {
                    xtr.Close();
                }
            }

        }
        public void EachXmlToComboBox()
        {
            combobox.Items.Clear();
            xtr = new XmlTextReader(xmlPath);
            try
            {

                while (xtr.Read())
                {
                    if (xtr.NodeType != XmlNodeType.XmlDeclaration)
                        combobox.Items.Add(xtr.Value);

                }

            }
            catch (XmlException xe)
            {
                throw new XmlException(ERRMSG + xe.Message);
            }
            finally
            {

                if (xtr != null)
                {
                    xtr.Close();
                }
            }

        }


        #endregion

        #region ��ȡXML�ļ�
        public void ReadXml()
        {
            string attAndEle = string.Empty;
            listbox.Items.Clear();
            this.xtr = new XmlTextReader(this.xmlPath);  //ͨ��·������ȡXML�ļ�
            try
            {
                while (xtr.Read())  //��������ʽһ���ڵ�һ���ڵ�ı�����ȡ�� if the next node was read successfully,return true
                {
                    //public enum XmlNodeType Ϊö�����ͣ�None Element Attribute Text CDATA EntityReference Entity ProcessingInstruction Comment Document DocumentType DocumentFragment Notation Whitespace SignificantWhitespace EndElement EndEntity XmlDeclaration 
                    if (xtr.NodeType == XmlNodeType.XmlDeclaration)  //XmlDeclarationΪXML ���������磬<?xml version='1.0'?>��
                    {
                        listbox.Items.Add(string.Format("<?{0}{1}?>", xtr.Name, xtr.Value)); //nameΪ�ڵ�����valueΪ�ڵ�����

                    }
                    else if (xtr.NodeType == XmlNodeType.Element)  //ElementԪ�أ����磬<item>��
                    {
                        attAndEle = string.Format("<{0}", xtr.Name);
                        if (xtr.HasAttributes)  //if the current node has attributes,return true
                        {
                            while (xtr.MoveToNextAttribute())  // if there is a next attribute,return true
                            {
                                attAndEle += string.Format("{0}=''''{1}'''' ", xtr.Name, xtr.Value);
                            }
                        }
                        attAndEle = attAndEle.Trim() + ">";
                        listbox.Items.Add(attAndEle);
                    }
                    else if (xtr.NodeType == XmlNodeType.EndElement) //An end element tag (for example, </item> ). 
                    {
                        listbox.Items.Add(string.Format("</{0}>", xtr.Name));

                    }
                    else if (xtr.NodeType == XmlNodeType.Text)  //The text content of a node. 
                    {
                        listbox.Items.Add(xtr.Value);
                    }
                }
            }
            catch (XmlException xmlExp)
            {
                throw new XmlException(ERRMSG + this.xmlPath + xmlExp.ToString());
            }
            finally
            {
                if (this.xtr != null)
                    this.xtr.Close();
            }

        }
        #endregion

        #region ��ȡXML�����ı��ڵ�ֵ
        public void ReadXmlTextToListBox()
        {
            listbox.Items.Clear();
            xtr = new XmlTextReader(xmlPath);
            try
            {
                while (xtr.Read())
                {
                    if (xtr.NodeType == XmlNodeType.Text)
                    {
                        listbox.Items.Add(xtr.Value);
                    }
                }
            }
            catch (XmlException xe)
            {
                throw new XmlException(ERRMSG + xe.Message);
            }
            finally
            {

                xtr.Close();
            }
        }
        #endregion
    }
}