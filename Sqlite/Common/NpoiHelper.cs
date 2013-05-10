using System;
using System.Data;
using System.IO;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Consoletest001.Sqlite.Common
{
    public class NpoiHelper
    {
        private readonly HSSFWorkbook _workbook;
        private readonly ISheet _sheet1;
        //private ICellStyle _titleStyle;
        private readonly string[] _params;

        public HSSFWorkbook Workbook
        {
            get { return _workbook; }
        }
        /// <summary>
        /// ���캯��
        /// </summary>
   
        /// <param name="title">��ͬ��sheet�ı���</param>
        /// <param name="colInfos">params�������ɽ��ܶ����ֵ����Ϣ��keyΪ��������valueΪ��dt�е�����</param>
        public NpoiHelper( string title, params  string[] colInfos)
        {
            this._workbook = new HSSFWorkbook();
            this._sheet1 = this.Workbook.CreateSheet(title);
            this._params = colInfos;
            this.SetFileInfo();
            this.SetTitleRow(title, colInfos.Length);
            this.SetSecondRow(_params);
        }

      

        /// <summary>
        /// ������,��ֱ�Ӵ����ݿ���ȡ������ʱʹ�ã�
        /// </summary>
        /// <param name="dt">ֻ������Ҫ���ݵ�datatable</param>
        /// <param name="colNames">�ֶ������ݿ��е�����</param>
        public  void BindTableData(DataTable dt,params  string[] colNames)
        {

            ICellStyle cellStyle = this.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.CENTER;
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;
   
            int i = 2;  //ע�����ݵ����������Ǵӵ�һ�п�ʼ��
            int colCount = this._params.Length;

            //�ȱ���dt ȡ��������dr��Ŀ����ÿ�е�һ�����һ����ŵı�ͷ���ٱ�����ͷ��Ϣ�����������
            foreach (DataRow dr in dt.Rows)
            {
                IRow row = this._sheet1.CreateRow(i);
                ICell cell0 = row.CreateCell(0);
                cell0.CellStyle = cellStyle;
                cell0.SetCellValue(i-1);//�������
                for (int j = 0; j < colCount; j++)
                {
                    ICell cell = row.CreateCell(j+1);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(dr[colNames[j]].ToString());
                }
                i++;
            }
        }

        /// <summary>
        /// ��ע����ʼ�е�λ�á����ⲿ��ÿһ�е�����
        /// </summary>
        /// <param name="rowIndex">��ǰ����,���Ĵӵ�3��(index=2)��ʼ</param>
        /// <param name="colValues">������ȡ����������stringֵ</param>
        public void BindRowData(int rowIndex, params string[] colValues)
        {
            ICellStyle cellStyle = this.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.CENTER;
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;

           
            int colCount = this._params.Length;

            //�ȱ���dt ȡ��������dr��Ŀ����ÿ�е�һ�����һ����ŵı�ͷ���ٱ�����ͷ��Ϣ�����������

            IRow row = this._sheet1.CreateRow(rowIndex);
            ICell cell0 = row.CreateCell(0);
            cell0.CellStyle = cellStyle;
            cell0.SetCellValue(rowIndex - 1); //�������
            for (int j = 0; j < colCount; j++)
            {
                ICell cell = row.CreateCell(j + 1);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(colValues[j]);
            }
        }

        /// <summary>
        /// ����Workbook��2��������Ϣ
        /// </summary>
        private void SetFileInfo()
        {
            //����Workbook��DocumentSummaryInformation��Ϣ
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "CHMTECH";
            this.Workbook.DocumentSummaryInformation = dsi;

            //����Workbook��SummaryInformation��Ϣ
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "CHMTECH EXCEL-EXPORT";
            this.Workbook.SummaryInformation = si;
        }


        /// <summary>
        /// ���õ�һ��Ϊtitle�� �����һЩ��ʽֱ��д����
        /// </summary>
        /// <param name="titleStr">��һ��title��context</param>
        /// <param name="mergedCount">�ϲ���������һ��Ϊ��ͷ��Ϣ����</param>
        /// <returns></returns>
        private void SetTitleRow(string titleStr, int mergedCount)
        {
            IRow titleRow = this._sheet1.CreateRow(0);
            titleRow.Height = 30 * 20;
            ICellStyle titleStyle = this.Workbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.CENTER; //��������

            //�ϲ�titleRow�ĸ��� ��Ϊ���˸���ţ����Ժϲ���ʱ��Ҳ��Ҫ��ϲ�һ��
            this._sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, mergedCount ));

            IFont font = this.Workbook.CreateFont(); //set font style
            font.FontHeight = 40 * 20;
            font.Boldweight = (short)FontBoldWeight.BOLD; //bold
            font.FontHeightInPoints = 16; //�����С
            titleStyle.SetFont(font);
            ICell titleCell = titleRow.CreateCell(0);
            titleCell.SetCellValue(new HSSFRichTextString(titleStr)); //title context
            titleCell.CellStyle = titleStyle; //bind style


        }

        //���õڶ��е���ͷ��Ϣ��ע����Ŵ�����index+1����
        private void SetSecondRow(string[] headArr)
        {
           
            ICellStyle style = this.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_YELLOW.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;

            IRow irow = this._sheet1.CreateRow(1);
            //��Ÿ���
            HSSFCell indexCell = (HSSFCell)irow.CreateCell(0);
            indexCell.SetCellValue("���");
            this._sheet1.SetColumnWidth(0, 5 * 256);

            irow.Height = 20 * 20;
            for (int i = 0; i < headArr.Length; i++)
            {
                string cellValue = headArr[i];
                HSSFCell curCell = (HSSFCell)irow.CreateCell(i+1);
                curCell.SetCellValue(cellValue);
                this._sheet1.SetColumnWidth(i+1, 20 * 256);
                curCell.CellStyle = style;
            }
        }

          //public void GetOutPutStream(ref Stream stream)
          //{
          //   this.Workbook.Write(stream);
          //}


        /// <summary>
        /// ��ӡxls�ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public  bool PrintXls( string path)
        {
            try
            {
                FileStream fs = File.Create(path);
                this.Workbook.Write(fs);
                fs.Flush();
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// �ṩ���أ�ͨ��response���2������Ϣ��
        /// </summary>
        /// <param name="Response">��ǰҳ���response</param>
        /// <param name="fileName">�ļ��������������׺��</param>
        public void OutPutDownload(HttpResponse Response,string fileName)
        {
            //���������û�����

            if (!fileName.EndsWith(@".xls"))
            {
                fileName += @".xls";
            }
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.Clear();
            MemoryStream file = new MemoryStream();
            this.Workbook.Write(file);
            Response.BinaryWrite(file.GetBuffer());
            Response.End();
        }


    }
}