﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace OgcToolkit.Ogc.Filter.V110
{

    public static class FilterQueryable
    {

        public static IQueryable<T> Where<T>(this IQueryable<T> source, Filter filter, XmlNamespaceManager namespaceManager=null, bool mayRootPathBeImplied=false, IOperatorImplementationProvider operatorImplementationProvider=null)
        {
            return (IQueryable<T>)Where((IQueryable)source, filter, namespaceManager, mayRootPathBeImplied, operatorImplementationProvider);
        }

        public static IQueryable Where(this IQueryable source, Filter filter, XmlNamespaceManager namespaceManager=null, bool mayRootPathBeImplied=false, IOperatorImplementationProvider operatorImplementationProvider=null)
        {
            Debug.Assert(source!=null);
            if (source==null)
                throw new ArgumentNullException("source");
            Debug.Assert(filter!=null);
            if (filter==null)
                throw new ArgumentNullException("filter");

            LambdaExpression lambda=filter.CreateLambda(source, namespaceManager, mayRootPathBeImplied, operatorImplementationProvider);
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { source.ElementType },
                    source.Expression,
                    Expression.Quote(lambda)
                )
            );
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortBy ordering, XmlNamespaceManager namespaceManager)
        {
            return (IQueryable<T>)OrderBy((IQueryable)source, ordering, namespaceManager);
        }

        public static IQueryable OrderBy(this IQueryable source, SortBy ordering, XmlNamespaceManager namespaceManager)
        {
            if (source==null)
                throw new ArgumentNullException("source");
            if (ordering==null)
                throw new ArgumentNullException("ordering");

            Expression query=ordering.CreateExpression(source.ElementType, source.Expression, namespaceManager);
            return source.Provider.CreateQuery(query);
        }
    }
}
