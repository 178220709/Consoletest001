using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using geoway.core_public.util;
using System.Windows.Forms;
using Geoway.ImageDB.DataInAndOutput.Model;
using Geoway.ImageDB.Catalog.DataModel;
using Geoway.ImageDB.DataInAndOutput.Definition;
using Geoway.ImageDB.Catalog.Instance;
using Geoway.ImageDB.Catalog.DAL;
using System.Data;
using Geoway.ImageDB.DataInAndOutput.DAL;
using geoway.core_public.security;
using Geoway.ImageDB.DataInAndOutput.Utility;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Geoway.ImageDB.Utility.Definition;
using Geoway.ImageDB.Utility.Class;

namespace Geoway.ImageDB.DataInputAndOutput.DataArchiving.Class
{
    /// <summary>
    /// 归档通用操作集合
    /// </summary>
    public class ArchiveUtil
    {
        /// <summary>
        /// 获取文件系统上的目录或者文件大小

        /// </summary>
        /// <param name="path">目录或文件路径</param>
        /// <returns></returns>
        public static long GetFileSystemEntrySize(string path)
        {
            long size = 0;
            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                return fileInfo.Length;
            }
            else
            {
                string[] lstPath = Directory.GetFileSystemEntries(path);
                foreach (string str in lstPath)
                {
                    size += GetFileSystemEntrySize(str);
                }
            }
            return size;
        }

        /// <summary>
        /// 根据数据量大小获取带单位的大小字符串
        /// </summary>
        /// <param name="size">大小（单位：字节）</param>
        /// <returns></returns>
        public static string GetSizeStringWithUnit(long size)
        {
            string[] uints = new string[] { "B", "KB", "MB", "GB", "TB" };
            double tmp = size;
            double tarSize = size;
            int i = 0;
            for (; i < 5; i++)
            {
                tmp = tmp / 1024;
                if (tmp < 1)
                {
                    break;
                }
                tarSize = tmp;
            }
            return tarSize.ToString("#0.##") + " " + uints[i];
        }

        /// <summary>
        /// 验证路径是否合法
        /// </summary>
        /// <param name="path">待检查路径</param>
        /// <returns></returns>
        public static bool IsPathValid(string path)
        {
            char[] invalidChars = System.IO.Path.GetInvalidPathChars();

            foreach (char ch in invalidChars)
            {
                if (path.Contains(ch.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 初始化GridView
        /// </summary>
        /// <param name="gridView"></param>
        public static void InitGridView(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            //gridView.GroupPanelText = geoway.core_public.i18n.I18n.Core_Message_GridGroup();

            gridView.ActiveFilterEnabled = false;

            //gridView.OptionsBehavior.Editable = false;
            gridView.OptionsBehavior.ImmediateUpdateRowPosition = false;

            gridView.OptionsCustomization.AllowFilter = false;
            gridView.OptionsCustomization.AllowColumnMoving = false;

            gridView.OptionsMenu.EnableColumnMenu = false;
            gridView.OptionsMenu.EnableFooterMenu = false;
            gridView.OptionsMenu.EnableGroupPanelMenu = false;

            gridView.OptionsView.ShowGroupPanel = false;
            gridView.OptionsView.ColumnAutoWidth = false;
            //gridView.OptionsView.ShowFilterPanel = false;
            gridView.OptionsView.ShowIndicator = false;

            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
        }

        /// <summary>
        /// 验证控件Text属性是否为空或空格
        /// </summary>
        /// <param name="txb">控件</param>
        /// <param name="msg">为空时的提示信息,为空则不提示</param>
        /// <returns></returns>
        public static bool ValidateControlTextEmpty(Control txb, string msg)
        {
            if (string.IsNullOrEmpty(txb.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    UIUtil.ShowDevMessageDialog(msg);
                    txb.Focus();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取任务状态名称

        /// </summary>
        /// <param name="state">状态枚举值</param>
        /// <returns></returns>
        public static string GetTaskStateString(EnumExecuteState state)
        {
            switch (state)
            {
                case EnumExecuteState.NoExecute:
                    return "未执行";
                case EnumExecuteState.Executing:
                    return "执行中";
                case EnumExecuteState.ExecuteFailed:
                    return "执行失败";
                case EnumExecuteState.ExecuteSuccessful:
                    return "执行成功 ";
                default:
                    return "未知";
            }
        }
    }

    /// <summary>
    /// 任务数据替换选项
    /// </summary>
    public class ReplaceOption
    {
        public EnumMessageBoxExResult SameTaskData = EnumMessageBoxExResult.Undefined;
        public EnumMessageBoxExResult SameRegisterInfo = EnumMessageBoxExResult.Undefined;
        public EnumMessageBoxExResult SameServerFile = EnumMessageBoxExResult.Undefined;

        public bool Cancel
        {
            get
            {
                return SameTaskData == EnumMessageBoxExResult.Cancel ||
                    SameRegisterInfo == EnumMessageBoxExResult.Cancel ||
                    SameServerFile == EnumMessageBoxExResult.Cancel;
            }
        }
    }

    /// <summary>
    /// 任务验证选项
    /// </summary>
    public class ValidateOption
    {
        public bool ValidateMetadata = true;
        public bool ValidateSnapshot = true;
        public bool ValidateThumb = true;
        public bool ValidateDataFile = true;
        public bool ValidateExtent = true;
    }
}