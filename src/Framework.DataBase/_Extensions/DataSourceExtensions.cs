using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.DataBase._Extensions;

public static class DataSourceExtensions
{
    private class RootState<T>(IDictionary<Type, Dictionary<T, T>> dictionary) : Dictionary<Type, Dictionary<T, T>>(dictionary)
        where T : notnull;

    public static IDataSource<TPersistentDomainObjectBase> LoadToMemory<TPersistentDomainObjectBase>(this IDataSource<TPersistentDomainObjectBase> dataSource, MemoryTypeCache<TPersistentDomainObjectBase> memoryTypeCache)
        where TPersistentDomainObjectBase : class
    {
        var createDictMethod = typeof(DataSourceExtensions).GetMethod(nameof(CreateDict), BindingFlags.Static | BindingFlags.NonPublic)!;


        var lists = memoryTypeCache.ImplTypes.ToList(pair => new
        {
            DomainType = pair.Key,

            ImplType = pair.Value,

            Dict = createDictMethod.MakeGenericMethod(typeof(TPersistentDomainObjectBase), pair.Key, pair.Value).Invoke<Dictionary<TPersistentDomainObjectBase, TPersistentDomainObjectBase>>(null, dataSource)
        });

        var rootDict = new RootState<TPersistentDomainObjectBase>(lists.ToDictionary(pair => pair.DomainType, pair => pair.Dict));

        foreach (var pair in lists)
        {
            var method = typeof(InitializeHelper<,,>).MakeGenericType(pair.DomainType, pair.ImplType, typeof(TPersistentDomainObjectBase))
                .GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public)!;

            method.Invoke(null, [rootDict]);
        }

        InitializeHelper<TPersistentDomainObjectBase, TPersistentDomainObjectBase, TPersistentDomainObjectBase>.Initialize(rootDict);

        var implTypeResolver = TypeResolverHelper.Create(lists.ToDictionary(pair => pair.DomainType, pair => pair.ImplType));

        return new MemCachedDataSource<TPersistentDomainObjectBase>(implTypeResolver, rootDict.Values.SelectMany(v => v.Values));
    }


    private static Dictionary<TPersistentDomainObjectBase, TPersistentDomainObjectBase> CreateDict<TPersistentDomainObjectBase, TDomainObject, TImplemented>(IDataSource<TPersistentDomainObjectBase> dataSource)
        where TDomainObject : class, TPersistentDomainObjectBase
        where TImplemented : class, TDomainObject, new()
        where TPersistentDomainObjectBase : class =>
        dataSource.GetFullList<TDomainObject>().ToDictionary(domainObject => domainObject, _ => (TPersistentDomainObjectBase)new TImplemented(), ReferenceComparer<TPersistentDomainObjectBase>.Value);

    private static class InitializeHelper<TDomainObject, TImplemented, TPersistentDomainObjectBase>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TImplemented : class, TDomainObject
        where TPersistentDomainObjectBase : class
    {
        private static readonly Action<TDomainObject, TImplemented, RootState<TPersistentDomainObjectBase>> InitPropertiesAction = GetInitPropertiesAction();


        private static Action<TDomainObject, TImplemented, RootState<TPersistentDomainObjectBase>> GetInitPropertiesAction()
        {
            var initPropertyMethod = new Func<PropertyInfo, Action<TDomainObject, TImplemented, RootState<TPersistentDomainObjectBase>>>(GetInitPropertyAction<object>).Method.GetGenericMethodDefinition();

            var initPropAssignActions = typeof(TDomainObject).GetAllInterfaceProperties().ToList(property =>

                initPropertyMethod.MakeGenericMethod(property.PropertyType).Invoke<Action<TDomainObject, TImplemented, RootState<TPersistentDomainObjectBase>>>(null, property));

            return (domainObj, implObj, rootDict) => domainObj.Maybe(v => initPropAssignActions.Foreach(action => action(v, implObj, rootDict)));
        }


        private static Action<TDomainObject, TImplemented, RootState<TPersistentDomainObjectBase>> GetInitPropertyAction<TProperty>(PropertyInfo property)
        {
            var sourceObjParameter = Expression.Parameter(typeof(TDomainObject), "sourceObject");

            var implObjParameter = Expression.Parameter(typeof(TImplemented), "implObject");

            var propertyParameter = Expression.Parameter(typeof(TProperty), "value");

            var setPropertyAction = Expression.Lambda<Action<TImplemented, TProperty>>(Expression.Assign(Expression.Property(implObjParameter, property.Name), propertyParameter), implObjParameter, propertyParameter).Compile();

            var getPrimitivePropertyValueFunc = Expression.Lambda<Func<TDomainObject, TProperty>>(Expression.Property(sourceObjParameter, property), sourceObjParameter).Compile();


            var dictArgs = typeof(TProperty).GetInterfaceImplementationArguments(typeof(IReadOnlyDictionary<,>));
                
            if (dictArgs != null && !dictArgs[0].IsAssignableToInterface(typeof(TPersistentDomainObjectBase)) && dictArgs[1].IsAssignableToInterface(typeof(TPersistentDomainObjectBase)))
            {
                var getSubRefPropertyValueFunc = new Func<Func<TDomainObject, IReadOnlyDictionary<object, TPersistentDomainObjectBase>>, Func<TDomainObject, RootState<TPersistentDomainObjectBase>, IReadOnlyDictionary<object, TPersistentDomainObjectBase>>>(GetSubRefDictionaryPropertyValueFunc<IReadOnlyDictionary<object, TPersistentDomainObjectBase>, object, TPersistentDomainObjectBase>)
                    .CreateGenericMethod(property.PropertyType, dictArgs[0], dictArgs[1])
                    .Invoke<Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty>>(null, getPrimitivePropertyValueFunc);

                return (sourceObj, implObj, rootState) => setPropertyAction(implObj, getSubRefPropertyValueFunc(sourceObj, rootState));
            }

            var collectionElementType = typeof(TProperty).GetCollectionElementType();

            if (collectionElementType != null && collectionElementType.IsAssignableToInterface(typeof(TPersistentDomainObjectBase)))
            {
                var getSubRefPropertyValueFunc = new Func<Func<TDomainObject, IEnumerable<TPersistentDomainObjectBase>>, Func<TDomainObject, RootState<TPersistentDomainObjectBase>, IEnumerable<TPersistentDomainObjectBase>>>(GetSubRefCollectionPropertyValueFunc<IEnumerable<TPersistentDomainObjectBase>, TPersistentDomainObjectBase>)
                    .CreateGenericMethod(property.PropertyType, collectionElementType)
                    .Invoke<Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty>>(null, getPrimitivePropertyValueFunc);

                return (sourceObj, implObj, rootState) => setPropertyAction(implObj, getSubRefPropertyValueFunc(sourceObj, rootState));
            }
            else if (typeof(TProperty).IsAssignableToInterface(typeof(TPersistentDomainObjectBase)))
            {
                var getSubRefPropertyValueFunc = new Func<Func<TDomainObject, TPersistentDomainObjectBase>, Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TPersistentDomainObjectBase>>(GetSubRefPropertyValueFunc<TPersistentDomainObjectBase>)
                    .CreateGenericMethod(property.PropertyType)
                    .Invoke<Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty>>(null, getPrimitivePropertyValueFunc);

                return (sourceObj, implObj, rootState) => setPropertyAction(implObj, getSubRefPropertyValueFunc(sourceObj, rootState));
            }
            else
            {
                return (sourceObj, implObj, rootState) => setPropertyAction(implObj, getPrimitivePropertyValueFunc(sourceObj));
            }
        }

        private static Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty> GetSubRefPropertyValueFunc<TProperty>(Func<TDomainObject, TProperty> getPrimitivePropertyValueFunc)
            where TProperty : class, TPersistentDomainObjectBase =>
            (sourceObj, rootState) => getPrimitivePropertyValueFunc(sourceObj).Maybe(propValue => rootState[typeof(TProperty)].Pipe(valueDict => (TProperty)valueDict[propValue]));

        private static Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty> GetSubRefCollectionPropertyValueFunc<TProperty, TElement>(Func<TDomainObject, TProperty> getPrimitivePropertyValueFunc)
            where TProperty : class, IEnumerable<TElement>
            where TElement : class, TPersistentDomainObjectBase =>
            (sourceObj, rootState) => getPrimitivePropertyValueFunc(sourceObj).Maybe(propValue => rootState[typeof(TElement)].Pipe(valueDict => (TProperty)(object)propValue.ToList(elementValue => (TElement)valueDict[elementValue])));

        private static Func<TDomainObject, RootState<TPersistentDomainObjectBase>, TProperty> GetSubRefDictionaryPropertyValueFunc<TProperty, TKey, TValue>(Func<TDomainObject, TProperty> getPrimitivePropertyValueFunc)
            where TProperty : class, IReadOnlyDictionary<TKey, TValue>
            where TValue : class, TPersistentDomainObjectBase =>
            (sourceObj, rootState) => getPrimitivePropertyValueFunc(sourceObj).Maybe(propValue => rootState[typeof(TValue)].Pipe(valueDict => (TProperty)(object)propValue.ChangeValue(elementValue => (TValue)valueDict[elementValue])));

        public static void Initialize(RootState<TPersistentDomainObjectBase> rootDict)
        {
            foreach (var pair in rootDict.GetValueOrDefault(typeof(TDomainObject)).EmptyIfNull())
            {
                var sourceObj = (TDomainObject)pair.Key;

                var implObj = (TImplemented)pair.Value;

                InitPropertiesAction(sourceObj, implObj, rootDict);

                continue;
            }
        }
    }
}