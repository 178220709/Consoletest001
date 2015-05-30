using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonSong.ManagerUI.Extend
{
    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //�������л�ȡ�ύ�Ĳ�������
            var json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName] as string;
            //�ύ�����Ƕ���
            if (string.IsNullOrEmpty(json))
            {
                json = controllerContext.HttpContext.Request[bindingContext.ModelName] as string;
            } 
            
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                JObject jsonBody = JObject.Parse(json);
                JsonSerializer js = new JsonSerializer();
                object obj = js.Deserialize(jsonBody.CreateReader(), typeof(T));
                return obj;
            }
            //�ύ����������
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                IList<T> list = new List<T>();
                JArray jsonRsp = JArray.Parse(json);
                if (jsonRsp != null)
                {
                    for (int i = 0; i < jsonRsp.Count; i++)
                    {
                        JsonSerializer js = new JsonSerializer();
                        try
                        {
                            object obj = js.Deserialize(jsonRsp[i].CreateReader(), typeof(T));
                            list.Add((T)obj);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                return list;
            }
            return null;
        }
    }
}