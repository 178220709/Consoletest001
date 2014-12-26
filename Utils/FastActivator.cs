using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Suijing.Utils
{
    public static class FastActivator
    {
        private static Dictionary<Type, Func<object>> factoryCache = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Creates an instance of the specified type using a generated factory to avoid using Reflection.
        /// </summary>
        /// <typeparam name="T">The type to be created.</typeparam>
        /// <returns>The newly created instance.</returns>
        public static T Create<T>()
        {
            return TypeFactory<T>.Create();
        }

        /// <summary>
        /// Creates an instance of the specified type using a generated factory to avoid using Reflection.
        /// </summary>
        /// <param name="type">The type to be created.</param>
        /// <returns>The newly created instance.</returns>
        public static object Create(Type type)
        {
            Func<object> f;

            if (!factoryCache.TryGetValue(type, out f))
                lock (factoryCache)
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        factoryCache[type] = f = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
                    }

            return f();
        }

        public static object Create(Type type, params object[] arguments)
        {
            Func<object> f;

            if (!factoryCache.TryGetValue(type, out f))
                lock (factoryCache)
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        var ctor = type.GetConstructors().FirstOrDefault();

                        if (ctor != null)
                        {
                            var args = arguments.Select(a => Expression.Constant(a));
                            factoryCache[type] = f = Expression.Lambda<Func<object>>(Expression.New(ctor, args)).Compile();
                        }
                    }

            return f();
        }

        private static class TypeFactory<T>
        {
            public static readonly Func<T> Create = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        }

        public static Action<object, object> GetValueSetter(this PropertyInfo propertyInfo)
        {
            var instance = Expression.Parameter(typeof(object), "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(
                Expression.Convert(instance, propertyInfo.DeclaringType),
                propertyInfo.GetSetMethod(),
                Expression.Convert(argument, propertyInfo.PropertyType));
            return Expression.Lambda<Action<object, object>>(setterCall, instance, argument).Compile();
        }
    }
}
