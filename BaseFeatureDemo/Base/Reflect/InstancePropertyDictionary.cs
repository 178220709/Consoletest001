using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace BaseFeatureDemo.Reflect
{
    /// <summary>
    /// Instance的属性高速读/写（无需转换类型）字典
    /// </summary>
    class InstancePropertyDictionary
    {
        /// <summary>
        /// Get委托
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        delegate TResult Get<TResult>();

        /// <summary>
        /// Set委托
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        delegate void Set<TValue>(TValue value);

        /// <summary>
        /// Instance
        /// </summary>
        object Target;

        /// <summary>
        /// Instance类型
        /// </summary>
        Type TargetType;
        public InstancePropertyDictionary(object instance)
        {
            this.Target = instance;
            this.TargetType = Target.GetType();
         //   LoadProperty();
        }

        #region Set委托
        /// <summary>
        /// Key是属性的名字
        /// Value是强类型的委托
        /// </summary>
        Dictionary<string, Set<Int32>> setInt32Dic = new Dictionary<string, Set<Int32>>();
        Dictionary<string, Set<string>> setStringDic = new Dictionary<string, Set<string>>();
        #endregion

        #region Get委托
        Dictionary<string, Get<Int32>> getInt32Dic = new Dictionary<string, Get<Int32>>();
        Dictionary<string, Get<string>> getStringDic = new Dictionary<string, Get<string>>();
        #endregion

        /// <summary>
        /// 装载一个类的属性
        /// </summary>
        public void LoadProperty(params string[] names)
        {
            var props = TargetType.GetProperties();
            foreach (var name in names)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == name)
                    {
                        CreateGetSet(prop);
                    }
                }
            }
        }

        /// <summary>
        /// 创建属性的Getter/Setter委托
        /// </summary>
        /// <param name="property"></param>
        void CreateGetSet(PropertyInfo property)
        {
            string propName = property.Name;
            var propType = property.PropertyType;
          
            var propSetMethod = property.GetSetMethod();
            var propGetMethod = property.GetGetMethod();
            if (typeof(Int32) == propType)
            {
                var set = CreateSet<Int32>(propSetMethod);
                setInt32Dic.Add(propName, set);

                var get = CreateGet<Int32>(propGetMethod);
                getInt32Dic.Add(propName, get);
            }
            else if (typeof(string) == propType)
            {
                var set = CreateSet<string>(propSetMethod);
                setStringDic.Add(propName, set);

                var get = CreateGet<string>(propGetMethod);
                getStringDic.Add(propName, get);
            }
            //剩下的else if请自己实现
        }


        Set<T> CreateSet<T>(MethodInfo methodInfo)
        {
            var result = (Set<T>)System.Delegate.CreateDelegate(typeof(Set<T>), Target, methodInfo);
            return result;
        }

        Get<T> CreateGet<T>(MethodInfo methodInfo)
        {
            var result = (Get<T>)System.Delegate.CreateDelegate(typeof(Get<T>), Target, methodInfo);
            return result;
        }

        /// <summary>
        /// Set值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetValue(string propertyName, Int32 value)
        {
            //去字典取得强类委托型
            var dg = setInt32Dic[propertyName];
            dg.Invoke(value);
        }

        public void SetValue(string propertyName, string value)
        {
            var dg = setStringDic[propertyName];
            dg.Invoke(value);
        }

        /// <summary>
        /// Get值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Int32 GetInt32(string propertyName)
        {
            var dg = getInt32Dic[propertyName];
            return dg.Invoke();
        }

        public string GetString(string propertyName)
        {
            var dg = getStringDic[propertyName];
            return dg.Invoke();
        }

    }

    public class ThisTest
    {
        public static void maintTest()
        {
            //假设是一个一个Entity对象
            var instance = new EntityTest();
//得到其Property Dictionary
            var propDic = new InstancePropertyDictionary(instance);
//无需转换为Object地赋值
          
//不存在类型转换地取值
            Int32 int32Value = propDic.GetInt32("Dt");
            string stringValue = propDic.GetString("Dt");
        }

        public static void maintTest2()
        {
            //假设是一个一个Entity对象
            var instance = new EntityTest();
            //得到其Property Dictionary
            var propDic = new InstancePropertyDictionary(instance);
            //无需转换为Object地赋值

            //不存在类型转换地取值
            Int32 int32Value = propDic.GetInt32("Age");
            string stringValue = propDic.GetString("Dt");
        }

    }

    class EntityTest
    {
        private DateTime _dt = DateTime.Now;

        public DateTime Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public Int32 Age = 1;

    }



}