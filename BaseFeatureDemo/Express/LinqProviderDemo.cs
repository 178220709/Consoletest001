using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace BaseFeatureDemo.Express
{
    public class LinqProviderDemo
    {
        static void Main1(string[] args)
        {
            var query = from x in new Integers()
                        where x < 10
                        select x;
            foreach (var item in query)
                Console.WriteLine(item);
            var query1 = from x in new Integers().AsEnumerable()
                         where x < 10
                         select x;
            foreach (var item in query1)
                Console.WriteLine(item);
        }
    }

    class Integers : IQueryable<int>
    {
        private LinqToIntegerProvider provider;

        private Expression expression;

        public Integers()
        {
            provider = new LinqToIntegerProvider();
            expression = Expression.Constant(this);
        }

        public Integers(LinqToIntegerProvider p, Expression ex)
        {
            provider = p;
            expression = ex;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return (provider.Execute<IEnumerable<int>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType
        {
            get { return typeof(int); }
        }

        public Expression Expression
        {
            get { return expression; }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }
    }

    class LinqToIntegerProvider : IQueryProvider
    {
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Integers(this, expression) as IQueryable<TElement>;

        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            string s = expression.ToString();
            if (s.Contains("Where"))
            {
                string max = Regex.Match(s, @"x\s<\s(\d+)").Groups[1].Value;
                return (TResult)foo(int.Parse(max));
            }
            else
            {
                return (TResult)foo(int.MaxValue);
            }

        }

        private IEnumerable<int> foo(int max)
        {
            for (int i = 0; i < max; i++)
                yield return i;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
