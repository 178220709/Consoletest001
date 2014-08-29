using System;

namespace MyMvcDemo.Extend
{
    /// <summary>
    /// 标记该controller和里面的action方法是否用于首页菜单
    /// </summary>
    public class ModuleAttribute : Attribute
    {
        public ModuleAttribute()
        {
            Sort = 0;
        }
        public String Name { get; set; }
        public String CSS { get; set; }

        public int Sort { get; set; }


    }
}