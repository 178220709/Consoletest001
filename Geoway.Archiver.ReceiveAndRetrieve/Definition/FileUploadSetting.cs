using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    /// <summary>
    /// 从外部文件的上传设置
    /// </summary>
    [Serializable]
    public class FileUploadSetting
    {
        public FileUploadSetting()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="useOutsideFile">是否使用外部文件</param>
        /// <param name="isUploadFile">是否上传文件实体</param>
        public FileUploadSetting(bool isUploadFile, bool useOutsideFile)
        {
            this.UseOutsideFile = useOutsideFile;
            this.IsUploadFile = isUploadFile;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="useOutsideFile">是否使用外部文件</param>
        /// <param name="isUploadFile">是否上传文件实体</param>
        /// <param name="outsidePath">外部文件目录</param>
        public FileUploadSetting(bool isUploadFile, bool useOutsideFile, string outsidePath, string fileFormat)
            : this(isUploadFile, useOutsideFile)
        {
            this.OutsidePath = outsidePath;
            this.FileFormat = fileFormat;
        }
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool UseOutsideFile = false;

        /// <summary>
        /// 是否上传文件
        /// </summary>
        public bool IsUploadFile = false;

        /// <summary>
        /// 外部文件路径
        /// </summary>
        public string OutsidePath = string.Empty;

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileFormat =string.Empty;

        /// <summary>
        /// 命名规则
        /// </summary>
        public CustomNameRuler NameRuler = null;
    }
}
