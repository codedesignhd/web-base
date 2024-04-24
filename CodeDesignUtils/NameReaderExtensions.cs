using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CodeDesign.Models.Extensions
{
    public static class NameReaderExtensions
    {
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null";
        private static readonly string invalidExpressionMessage = "Invalid expression";

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }
            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }
            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }
            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }
            throw new ArgumentException(invalidExpressionMessage);
        }
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }
            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }

        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression) where T : class
        {
            return GetMemberName(expression.Body);
        }
        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression) where T : class
        {
            return GetMemberName(expression.Body);
        }
        public static List<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions) where T : class
        {
            List<string> memberNames = new List<string>();
            foreach (var cExpression in expressions)
            {
                memberNames.Add(GetMemberName(cExpression.Body));
            }
            return memberNames;
        }

        public static string NameOf<T>(Expression<Func<T, object>> propertyExpression)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Expression is not a MemberExpression", nameof(propertyExpression));
            }
            return memberExpression.Member.Name;
        }

        public static IEnumerable<string> NameOf<T>(params Expression<Func<T, object>>[] expressions) where T : class
        {
            foreach (var cExpression in expressions)
            {
                yield return NameOf(cExpression);
            }
        }
    }
}
