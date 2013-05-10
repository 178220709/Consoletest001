using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.Utility.Log;
using MAPCOMLib;
using System.Data;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class MapNoConvert
    {
        #region private static 字段

        private static MapFrameCoordClass _coorTran = null;

        #endregion

        #region public static 方法

        /// <summary>
        /// 处理图幅号
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>返回新国标图幅号</returns>
        public static string MapNoProcess(DataRow dataRow)
        {
            try
            {
                if (_coorTran == null)
                {
                    _coorTran = new MapFrameCoordClass();
                }
                
                string newGBMapNo = string.Empty;
                // 处理新旧图幅号
                DataTable dtDataInfo = dataRow.Table;
                if (dtDataInfo.Columns.Contains(FixedFieldName.ALIAS_NAME_F_MAPNUMBER) &&
                    dtDataInfo.Columns.Contains(FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER))
                {
                    object objMapNum = dataRow[FixedFieldName.ALIAS_NAME_F_MAPNUMBER];
                    object objOldMapNum = dataRow[FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER];


                    if ((objMapNum != null && objMapNum.ToString().Trim().Length != 0) &&
                        (objOldMapNum == null || objOldMapNum.ToString().Trim().Length == 0))
                    {// 只采集了新图号
                        string strMapNum = objMapNum.ToString().Trim();
                        MapFormatEnumTag mapNoFormat = _coorTran.GetMapCodeFormat(strMapNum);
                        if (mapNoFormat == MapFormatEnumTag.ENewGB)
                        {
                            objOldMapNum = _coorTran.MapCodeConvertEx(strMapNum, MapFormatEnumTag.EOldGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.ENewJB)
                        {
                            objOldMapNum = _coorTran.MapCodeConvertEx(strMapNum, MapFormatEnumTag.EOldJB_1);
                            newGBMapNo = _coorTran.MapCodeConvert(strMapNum);
                        }
                        else
                        {
                            objOldMapNum = string.Empty;
                        }
                        dataRow[FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER] = objOldMapNum;

                        if (dtDataInfo.Columns.Contains(FixedFieldName.ALIAS_NAME_F_ISNEWMAP))
                        {
                            dataRow[FixedFieldName.ALIAS_NAME_F_ISNEWMAP] = "是";
                        }

                    }
                    else if ((objMapNum == null || objMapNum.ToString().Trim().Length == 0) &&
                        (objOldMapNum != null && objOldMapNum.ToString().Trim().Length != 0))
                    {// 只采集了旧图号
                        string strOldMapNum = objOldMapNum.ToString().Trim();
                        MapFormatEnumTag mapNoFormat = _coorTran.GetMapCodeFormat(strOldMapNum);
                        if (mapNoFormat == MapFormatEnumTag.EOldGB)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldGB_1W)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_0)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_1)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_1_1W)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else
                        {
                            objMapNum = string.Empty;
                        }

                        dataRow[FixedFieldName.ALIAS_NAME_F_MAPNUMBER] = objMapNum;

                        if (dtDataInfo.Columns.Contains(FixedFieldName.ALIAS_NAME_F_ISNEWMAP))
                        {
                            dataRow[FixedFieldName.ALIAS_NAME_F_ISNEWMAP] = "否";
                        }
                    }
                    else if ((objMapNum != null && objMapNum.ToString().Trim().Length != 0) &&
                         (objOldMapNum != null && objOldMapNum.ToString().Trim().Length != 0))
                    {
                        newGBMapNo = objMapNum.ToString();
                    }
                    else
                    {
                        newGBMapNo = string.Empty;
                    }
                }
                return newGBMapNo;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 处理图幅号
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>返回新国标图幅号</returns>
        public static string MapNoProcess(Dictionary<string, object> dicResult)
        {
            try
            {
                if (_coorTran == null)
                {
                    _coorTran = new MapFrameCoordClass();
                }

                string newGBMapNo = string.Empty;
                // 处理新旧图幅号
                if (dicResult.ContainsKey(FixedFieldName.ALIAS_NAME_F_MAPNUMBER) &&
                    dicResult.ContainsKey(FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER))
                {
                    object objMapNum = dicResult[FixedFieldName.ALIAS_NAME_F_MAPNUMBER];
                    object objOldMapNum = dicResult[FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER];


                    if ((objMapNum != null && objMapNum.ToString().Trim().Length != 0) &&
                        (objOldMapNum == null || objOldMapNum.ToString().Trim().Length == 0))
                    {// 只采集了新图号
                        string strMapNum = objMapNum.ToString();
                        MapFormatEnumTag mapNoFormat = _coorTran.GetMapCodeFormat(strMapNum);
                        if (mapNoFormat == MapFormatEnumTag.ENewGB)
                        {
                            objOldMapNum = _coorTran.MapCodeConvertEx(strMapNum, MapFormatEnumTag.EOldGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.ENewJB)
                        {
                            objOldMapNum = _coorTran.MapCodeConvertEx(strMapNum, MapFormatEnumTag.EOldJB_1);
                            newGBMapNo = _coorTran.MapCodeConvert(strMapNum);
                        }
                        else
                        {
                            objOldMapNum = string.Empty;
                        }

                        dicResult[FixedFieldName.ALIAS_NAME_F_OLDMAPNUMBER] = objOldMapNum;
                        if (dicResult.ContainsKey(FixedFieldName.ALIAS_NAME_F_ISNEWMAP))
                        {
                            dicResult[FixedFieldName.ALIAS_NAME_F_ISNEWMAP] = "是";
                        }

                    }
                    else if ((objMapNum == null || objMapNum.ToString().Trim().Length == 0) &&
                        (objOldMapNum != null && objOldMapNum.ToString().Trim().Length != 0))
                    {// 只采集了旧图号
                        string strOldMapNum = objOldMapNum.ToString();
                        MapFormatEnumTag mapNoFormat = _coorTran.GetMapCodeFormat(strOldMapNum);
                        if (mapNoFormat == MapFormatEnumTag.EOldGB)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldGB_1W)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewGB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_0)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_1)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else if (mapNoFormat == MapFormatEnumTag.EOldJB_1_1W)
                        {
                            objMapNum = _coorTran.MapCodeConvertEx(strOldMapNum, MapFormatEnumTag.ENewJB);
                            newGBMapNo = _coorTran.MapCodeConvert(strOldMapNum);
                        }
                        else
                        {
                            objMapNum = string.Empty;
                        }

                        dicResult[FixedFieldName.ALIAS_NAME_F_MAPNUMBER] = objMapNum;

                        if (dicResult.ContainsKey(FixedFieldName.ALIAS_NAME_F_ISNEWMAP))
                        {
                            dicResult[FixedFieldName.ALIAS_NAME_F_ISNEWMAP] = "否";
                        }
                    }
                    else if ((objMapNum != null && objMapNum.ToString().Trim().Length != 0) &&
                        (objOldMapNum != null && objOldMapNum.ToString().Trim().Length != 0))
                    {
                        newGBMapNo = objMapNum.ToString();
                    }
                    else
                    {
                        newGBMapNo = string.Empty;
                    }
                }
                return newGBMapNo;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取新国标图幅号
        /// </summary>
        /// <param name="mapNo"></param>
        /// <returns></returns>
        public static string GetNewGBMapNo(string mapNo)
        {
            try
            {
                if (_coorTran == null)
                {
                    _coorTran = new MapFrameCoordClass();
                }
                return _coorTran.MapCodeConvert(mapNo);
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return string.Empty;
            }
        }

        #endregion
    }
}
