using System;
using System.Windows.Forms;
using System.Xml;
using XmlReader = DemoXmlReader.XmlReader;

namespace Consoletest001.XML
{
    public partial class XmlForm : Form
    {
        private TextBox txtPath;
        private Button btnOpenXML;
        private Button button2;
        private ComboBox cbxml;
        private Button button3;
        private ListBox lbxml;
    
        public XmlForm()
        {
            InitializeComponent();
        }

        private void btnReaderXML_Click(object sender, EventArgs e)
        {
            
            XmlReader xr = new XmlReader(txtPath.Text, lbxml);
            try
            {
                //  xr.EachXmlToListBox();
                xr.ReadXmlTextToListBox();
                //xr.ReadXml();
            }
            catch (XmlException xe)
            {
                MessageBox.Show(xe.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                xr.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);   //指定默认打开的窗口指向的文件
            ofd.ShowDialog();
            txtPath.Text = ofd.FileName;   //把路径复制给txtPath文本框
        }

        private void btnReaderXmlToCb_Click(object sender, EventArgs e)
        {
            XmlReader xr = new XmlReader(txtPath.Text, cbxml);
            try
            {
                xr.EachXmlToComboBox();
            }
            catch (XmlException xe)
            {
                MessageBox.Show(xe.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                xr.Dispose();
            }

        }

        #region

        private void InitializeComponent()
        {
            this.lbxml = new System.Windows.Forms.ListBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnOpenXML = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbxml = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbxml
            // 
            this.lbxml.FormattingEnabled = true;
            this.lbxml.ItemHeight = 12;
            this.lbxml.Location = new System.Drawing.Point(12, 22);
            this.lbxml.Name = "lbxml";
            this.lbxml.Size = new System.Drawing.Size(214, 328);
            this.lbxml.TabIndex = 0;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(287, 124);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(140, 21);
            this.txtPath.TabIndex = 1;
            // 
            // btnOpenXML
            // 
            this.btnOpenXML.Location = new System.Drawing.Point(332, 47);
            this.btnOpenXML.Name = "btnOpenXML";
            this.btnOpenXML.Size = new System.Drawing.Size(75, 23);
            this.btnOpenXML.TabIndex = 3;
            this.btnOpenXML.Text = "openXML";
            this.btnOpenXML.UseVisualStyleBackColor = true;
            this.btnOpenXML.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(433, 124);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cbxml
            // 
            this.cbxml.FormattingEnabled = true;
            this.cbxml.Location = new System.Drawing.Point(287, 201);
            this.cbxml.Name = "cbxml";
            this.cbxml.Size = new System.Drawing.Size(140, 20);
            this.cbxml.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(433, 201);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnReaderXmlToCb_Click);
            // 
            // XmlForm
            // 
            this.ClientSize = new System.Drawing.Size(515, 467);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbxml);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnOpenXML);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lbxml);
            this.Name = "XmlForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       

    }
}

