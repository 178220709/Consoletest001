using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    class KeywordIndexHelper
    {
        /// <summary>
        /// 添加关键字索引
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">关键字类型</param>
        /// <returns></returns>
        public static bool AddKeywordIndex(string keyword, EnumKeywordIndexType type)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return false;
            }
            else
            {
                KeywordIndexDAL dal = KeywordIndexDAL.Singleton.Select(keyword, type);
                if (dal == null)
                {
                    dal = new KeywordIndexDAL();
                    dal.IndexType = type;
                    dal.IndexValue = keyword;
                    return dal.Insert();
                }
                else
                {
                    return dal.IncreaseTimes();
                }
            }
        }

        /// <summary>
        /// 删除关键字索引
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">关键字类型</param>
        /// <returns></returns>
        public static bool DeleteKeywordIndex(string keyword, EnumKeywordIndexType type)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return false;
            }
            else
            {
                KeywordIndexDAL dal = KeywordIndexDAL.Singleton.Select(keyword, type);
                if (dal == null)
                {
                    return true;
                }
                else
                {
                    if (dal.Times > 1)
                    {
                        return dal.DecreaseTimes();
                    }
                    else
                    {
                        return dal.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// 获取关键字已有列表
        /// </summary>
        /// <param name="type">关键字类型</param>
        /// <returns></returns>
        public static string[] GetKeywordList(EnumKeywordIndexType type)
        {
            IList<KeywordIndexDAL> lst = KeywordIndexDAL.Singleton.Select(type);
            if (lst == null || lst.Count <= 0)
            {
                return null;
            }
            else
            {
                List<string> keys = new List<string>();
                foreach (KeywordIndexDAL var in lst)
                {
                    keys.Add(var.IndexValue);
                }
                return keys.ToArray();
            }
        }
    }
}
