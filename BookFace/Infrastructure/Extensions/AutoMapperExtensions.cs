using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BookFace.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TResult ProjectToSingle<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, IConfigurationProvider mapper)
        {
           return source.Where(predicate).ProjectTo<TResult>(mapper).FirstOrDefault();
        }
    }
}
