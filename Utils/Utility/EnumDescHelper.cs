using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;


namespace Suijing.Utils.Utility
{
    public static class EnumDescHelper
    {
        public static string GetEnumDesc(this Enum enumType)
        {
            return enumType.ToString().GetEnumDesc(enumType.GetType());
        }

        public static string GetEnumDesc(this int? value, Type enumType)
        {
            return value.HasValue ? ((int)value).GetEnumDesc(enumType) : string.Empty;
        }

        public static string GetEnumDesc(this int value, Type enumType)
        {
            var name = Enum.GetName(enumType, value);
            return name.GetEnumDesc(enumType);
        }

        public static string GetEnumDesc(this string name, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType Require Enum Type");
            }
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
            {
              //  return string.Empty;
               return name;
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                return attr.Description;
            }
        }

        public static int GetEnumByDesc(this string desc, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType Require Enum Type");
            }
            if (string.IsNullOrEmpty(desc))
                throw new ArgumentException("desc Cannot Be NullOrEmpty");
            Array Values = Enum.GetValues(enumType);
            foreach (int value in Values)
            {
                if (value.GetEnumDesc(enumType) == desc)
                    return value;
            }
            throw new ArgumentException("desc argumen is illegal,can not find it's value");
        }

        /// <summary>
        /// 枚举DropDownSelect帮助类
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="defaultItem"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumSelectItemList(Type enumType, String selectValue = "")
        {
            return GetEnumSelectItemList(enumType, null, null, selectValue);
        }

        /// <summary>
        /// 枚举DropDownSelect帮助类
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="defaultItem"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumSelectItemList(Type enumType, string defaultDes, string defaultValue, String selectValue = "")
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType requires a Enum ");
            }
            List<SelectListItem> selectListItem = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(defaultDes))
            {
                selectListItem.Add(new SelectListItem() {   Text = defaultDes, Value = defaultValue });
            } 

            foreach (Enum value in Enum.GetValues(enumType))
            {
                if (value.ToString().ToLower() == "none")
                {
                    continue;
                }
                SelectListItem selectItem = new SelectListItem { Text = value.GetEnumDesc(), Value = Convert.ToInt32(value).ToString() };

                if (value.ToString() == selectValue)
                {
                    selectItem.Selected = true;
                }

                selectListItem.Add(selectItem);
            }

            return selectListItem;
        }
    }
}
