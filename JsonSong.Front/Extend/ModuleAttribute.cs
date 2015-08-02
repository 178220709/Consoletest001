using System;
using Suijing.Utils.Constants;

namespace JsonSong.Front.Extend
{
    /// <summary>
    /// 标记该controller和里面的action方法是否用于首页菜单
    /// </summary>
    public class ModuleAttribute : Attribute
    {
        public ModuleAttribute()
        {
            Sort = 0;
            CSS = MyConstants.Bootstrap.Icon.Globe;
        }
        public String Name { get; set; }
        public String CSS { get; set; }

        public int Sort { get; set; }
    }

    /// <summary>
    /// 被打上此标签的action将不在index侧边栏中显示(httpPost也由此功能)
    /// </summary>
    public class IgnoreAttribute : Attribute
    {

    }
}