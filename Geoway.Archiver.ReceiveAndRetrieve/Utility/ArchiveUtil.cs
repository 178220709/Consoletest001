using Geoway.Archiver.ReceiveAndRetrieve.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using System.IO;
    using System.Windows.Forms;
    using Definition;
    using Geoway.ADF.MIS.Core.Public.util;
    using EnumMessageBoxExResult = Archiver.Utility.Definition.EnumMessageBoxExResult;
    using Geoway.Archiver.Utility;
    using Geoway.ADF.MIS.Utility.Log;
    using Geoway.Archiver.Utility.Class;

    /// <summary>
    /// 归档通用操作集合
    /// </summary>
    public class ArchiveUtil
    {

   
        
        public static string GenSourceFilePath(string filePath,UpLoadSetting upLoadSetting)
        {
            try
            {
                // 检查路径是否含有盘符或是否为网上邻居所选取路径
                if (!filePath.Contains(":\\") && !filePath.StartsWith("\\\\"))
                {
                    return filePath;
                }
                //网上邻居所选取路径
                if (filePath.StartsWith("\\\\"))
                {
                    string ip;
                    string dominName;
                    NetControl.ParseDomainName(filePath,out ip,out dominName);
                    return ip;//返回IP
                }

                // 获取盘符格式:D:\
                string driveName = filePath.Substring(0, 3);

                // 获取文件所在磁盘类型
                
                //入库速度变慢原因之二，下行可用修改2替换
                //EnumDriveType driveType = NetworkDriver.SingleInstace.GetLocalDriveTypeEnum(driveName);
                //修改2： 
                EnumDriveType driveType = upLoadSetting.DriveType;
                // 根据获取的磁盘类型生成原文件路径前缀
                string preString = string.Empty;
                switch (driveType)
                {
                    case EnumDriveType.enumFixed:
                        preString = DNSOpers.SingleInstance.GetLocalIPString();
                        filePath = preString + "/" + filePath;
                        break;
                    case EnumDriveType.enumRemote:
                        preString = NetworkDriver.SingleInstace.GetMappedShareName(driveName);
                        filePath = preString + filePath.Substring(2);
                        break;
                    case EnumDriveType.enumRemovable:
                        preString = upLoadSetting.DiskSn;
                        filePath = preString + "/" + filePath.Substring(3);
                        break;
                    default:
                        preString = "";
                        break;
                }

                return filePath;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return "";
            }
        }

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
                    GUIUtil.ShowMessageDialog(msg);
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