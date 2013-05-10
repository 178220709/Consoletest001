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
        /// 构造函数
        /// </summary>
   
        /// <param name="title">合同和sheet的标题</param>
        /// <param name="colInfos">params参数，可接受多个键值对信息，key为中文名，value为在dt中的名称</param>
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
        /// 绑定数据,可直接从数据库中取出数据时使用，
        /// </summary>
        /// <param name="dt">只包含需要数据的datatable</param>
        /// <param name="colNames">字段在数据库中的列名</param>
        public  void BindTableData(DataTable dt,params  string[] colNames)
        {

            ICellStyle cellStyle = this.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.CENTER;
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;
   
            int i = 2;  //注意内容的行数并不是从第一行开始的
            int colCount = this._params.Length;

            //先遍历dt 取出行数（dr数目），每行第一列添加一个序号的表头，再遍历表头信息数组填充数据
            foreach (DataRow dr in dt.Rows)
            {
                IRow row = this._sheet1.CreateRow(i);
                ICell cell0 = row.CreateCell(0);
                cell0.CellStyle = cellStyle;
                cell0.SetCellValue(i-1);//设置序号
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
        /// 【注意起始行的位置】在外部绑定每一行的数据
        /// </summary>
        /// <param name="rowIndex">当前行数,正文从第3行(index=2)开始</param>
        /// <param name="colValues">调用者取出并处理后的string值</param>
        public void BindRowData(int rowIndex, params string[] colValues)
        {
            ICellStyle cellStyle = this.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.CENTER;
            cellStyle.BorderBottom = BorderStyle.THIN;
            cellStyle.BorderTop = BorderStyle.THIN;
            cellStyle.BorderLeft = BorderStyle.THIN;
            cellStyle.BorderRight = BorderStyle.THIN;

           
            int colCount = this._params.Length;

            //先遍历dt 取出行数（dr数目），每行第一列添加一个序号的表头，再遍历表头信息数组填充数据

            IRow row = this._sheet1.CreateRow(rowIndex);
            ICell cell0 = row.CreateCell(0);
            cell0.CellStyle = cellStyle;
            cell0.SetCellValue(rowIndex - 1); //设置序号
            for (int j = 0; j < colCount; j++)
            {
                ICell cell = row.CreateCell(j + 1);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(colValues[j]);
            }
        }

        /// <summary>
        /// 设置Workbook的2个属性信息
        /// </summary>
        private void SetFileInfo()
        {
            //设置Workbook的DocumentSummaryInformation信息
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "CHMTECH";
            this.Workbook.DocumentSummaryInformation = dsi;

            //设置Workbook的SummaryInformation信息
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "CHMTECH EXCEL-EXPORT";
            this.Workbook.SummaryInformation = si;
        }


        /// <summary>
        /// 设置第一行为title行 里面的一些样式直接写死了
        /// </summary>
        /// <param name="titleStr">第一行title的context</param>
        /// <param name="mergedCount">合并的列数，一般为列头信息个数</param>
        /// <returns></returns>
        private void SetTitleRow(string titleStr, int mergedCount)
        {
            IRow titleRow = this._sheet1.CreateRow(0);
            titleRow.Height = 30 * 20;
            ICellStyle titleStyle = this.Workbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.CENTER; //字体排列

            //合并titleRow的格子 因为多了个序号，所以合并的时候也需要多合并一格
            this._sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, mergedCount ));

            IFont font = this.Workbook.CreateFont(); //set font style
            font.FontHeight = 40 * 20;
            font.Boldweight = (short)FontBoldWeight.BOLD; //bold
            font.FontHeightInPoints = 16; //字体大小
            titleStyle.SetFont(font);
            ICell titleCell = titleRow.CreateCell(0);
            titleCell.SetCellValue(new HSSFRichTextString(titleStr)); //title context
            titleCell.CellStyle = titleStyle; //bind style


        }

        //设置第二行的列头信息，注意序号带来的index+1问题
        private void SetSecondRow(string[] headArr)
        {
           
            ICellStyle style = this.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_YELLOW.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;

            IRow irow = this._sheet1.CreateRow(1);
            //序号格子
            HSSFCell indexCell = (HSSFCell)irow.CreateCell(0);
            indexCell.SetCellValue("序号");
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
        /// 打印xls文件
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
        /// 提供下载，通过response输出2进制信息。
        /// </summary>
        /// <param name="Response">当前页面的response</param>
        /// <param name="fileName">文件名，无须输入后缀名</param>
        public void OutPutDownload(HttpResponse Response,string fileName)
        {
            //导出，让用户下载

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