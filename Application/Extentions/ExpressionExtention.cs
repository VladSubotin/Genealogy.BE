using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Extentions
{
    public static class ExpressionExtention
    {
        public static Expression<Func<T, bool>> MergeAnd<T>(this Expression<Func<T, bool>> ex1, Expression<Func<T, bool>> ex2)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(ex1, parameter),
                Expression.Invoke(ex2, parameter)
            );
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> MergeOr<T>(this Expression<Func<T, bool>> ex1, Expression<Func<T, bool>> ex2)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(ex1, parameter),
                Expression.Invoke(ex2, parameter)
            );
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
