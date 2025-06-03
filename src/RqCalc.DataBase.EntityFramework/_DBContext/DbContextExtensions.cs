//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

//using Framework.Core;
//

//namespace Anon.RQ_Calc.DataBase.EntityFramework
//{
//    public static class DbContextExtensions
//    {
//        public static void LoadToMemory(this DbContext dbContext)
//        {
//            var getTupleObjectsGenericMethod = new Func<DbSet<object>, IEnumerable<Tuple<object>>>(GetTupleObjects).Method.GetGenericMethodDefinition();

//            var request = from property in dbContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)

//                          let dbType = property.PropertyType.GetGenericTypeImplementationArgument(typeof(DbSet<>))

//                          where dbType != null && dbType != typeof(Image) && dbType != typeof(StaticImage)

//                          from obj in getTupleObjectsGenericMethod.MakeGenericMethod(dbType).Invoke<IEnumerable<object>>(null, property.GetValue(dbContext))

//                          select obj;


//            var res = request.ToList();

//            LoadGraph(new Queue<object>(res));

//            return;
//        }


//        private static IEnumerable<Tuple<T>> GetTupleObjects<T>(DbSet<T> dbSet)
//            where T : class
//        {
//            if (dbSet == null) throw new ArgumentNullException(nameof(dbSet));

//            return dbSet.ToList().Select(Tuple.Create<T>);
//        }


//        private static void LoadGraph(this Queue<object> source)
//        {
//            if (source == null) throw new ArgumentNullException(nameof(source));

//            var loadedObjects = new HashSet<object>();
//            var elemTypeCache = new DictionaryCache<Type, Type>(t => t.GetGenericTypeImplementationArgument(typeof(Tuple<>)));

//            while (source.Any())
//            {
//                var p = source.Dequeue();

//                if (loadedObjects.Add(p))
//                {
//                    p.GetSubElements(elemTypeCache).Foreach(source.Enqueue);
//                }
//            }
//        }

//        private static IEnumerable<object> GetSubElements(this object obj, IDictionaryCache<Type, Type> elemTypeCache)
//        {
//            if (obj == null) throw new ArgumentNullException(nameof(obj));
//            if (elemTypeCache == null) throw new ArgumentNullException(nameof(elemTypeCache));

//            var elementType = elemTypeCache[obj.GetType()];

//            return new Func<Tuple<object>, IEnumerable<object>>(GetSubElements<object>).CreateGenericMethod(elementType).Invoke<IEnumerable<object>>(null, obj);
//        }

//        private static IEnumerable<object> GetSubElements<T>(this Tuple<T> obj)
//            where T : class
//        {
//            if (obj == null) throw new ArgumentNullException(nameof(obj));

//            return SubElementsHelper<T>.GetSubElementsFunc(obj.Item1);
//        }

//        private static class SubElementsHelper<T>
//        {
//            public static readonly Func<T, IEnumerable<object>> GetSubElementsFunc = GetGetSubElementsFunc();


//            private static Func<T, IEnumerable<object>> GetGetSubElementsFunc()
//            {
//                var calllProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                                               .Where(prop => prop.PropertyType != typeof(Image) && prop.PropertyType != typeof(StaticImage) && !prop.GetIndexParameters().Any()).ToArray();//.Where(prop => !prop.PropertyType.IsArray && !prop.PropertyType.IsPrimitiveType() && prop.GetGetMethod().Maybe(method => method.IsVirtual)).ToList();

//                return calllProperties.Select(GetGetSubElementsFunc)
//                                      .Aggregate(FuncHelper.Create((T _) => Enumerable.Empty<object>()), (state, f) => source => state(source).Concat(f(source)));
//            }


//            private static Func<T, IEnumerable<object>> GetGetSubElementsFunc(PropertyInfo property)
//            {
//                var method =

//                    property.PropertyType.IsArray
//                 || property.PropertyType.IsPrimitiveType()
//                 || property.PropertyType.IsDictionaryType() ? new Func<Func<T, object>, Func<T, IEnumerable<object>>>(GetGetSubElementsSingleEmptyFunc<object>).CreateGenericMethod(property.PropertyType)

//                   : property.PropertyType.IsCollection() ? new Func<Func<T, IEnumerable<object>>, Func<T, IEnumerable<object>>>(GetGetSubElementsManyFunc<IEnumerable<object>, object>).CreateGenericMethod(property.PropertyType, property.PropertyType.GetCollectionElementType())
//                                                            : new Func<Func<T, object>, Func<T, IEnumerable<object>>>(GetGetSubElementsSingleFunc<object>).CreateGenericMethod(property.PropertyType);

//                var callExpr = Expression.Call(method, Expression.Constant(property.ToLambdaExpression().Compile()));

//                var expr = Expression.Lambda<Func<Func<T, IEnumerable<object>>>>(callExpr);

//                var f = expr.Compile();

//                return f();
//            }

//            private static Func<T, IEnumerable<object>> GetGetSubElementsSingleFunc<TProperty>(Func<T, TProperty> func)
//                 where TProperty : class
//            {
//                return source => GetGetSubElementsSingleFunc(source, func);
//            }

//            private static Func<T, IEnumerable<object>> GetGetSubElementsSingleEmptyFunc<TProperty>(Func<T, TProperty> func)
//            {
//                return source =>
//                {
//                    func(source);

//                    return new object[0];
//                };
//            }

//            private static IEnumerable<object> GetGetSubElementsSingleFunc<TProperty>(T source, Func<T, TProperty> func)
//            {
//                var element = func(source);

//                if (element != null)
//                {
//                    yield return Tuple.Create(element);
//                }
//            }

//            private static Func<T, IEnumerable<object>> GetGetSubElementsManyFunc<TProperty, TElement>(Func<T, TProperty> func)
//                where TProperty : IEnumerable<TElement>
//                where TElement : class
//            {
//                return source => GetGetSubElementsManyFunc<TProperty, TElement>(source, func);
//            }

//            private static IEnumerable<object> GetGetSubElementsManyFunc<TProperty, TElement>(T source, Func<T, TProperty> func)
//                where TProperty : IEnumerable<TElement>
//                where TElement : class
//            {
//                foreach (var element in func(source))
//                {
//                    yield return Tuple.Create(element);
//                }
//            }
//        }
//    }
//}