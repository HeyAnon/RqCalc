using System;
using System.Collections.Generic;
using System.Reflection;
using Anon.RQ_Calc.DataBase;
using Framework.Core;


using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.TransferData
{
    public class MappingService : IMappingService
    {
        private readonly IDictionaryCache<Type, IDictionaryCache<int, IPersistentIdentityDomainObjectBase>> _cache;
        

        private readonly IDictionaryCache<Tuple<StampIdentityDTO, StampColorIdentityDTO>, IStampVariant> _stampVariantCache;

        private readonly IDictionaryCache<IEquipment, IEquipmentClass> _equipmentClassCache;


        public MappingService(IApplicationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            this.Context = context;


            this._cache = new DictionaryCache<Type, IDictionaryCache<int, IPersistentIdentityDomainObjectBase>>(t => this.GetInternalCache(t).WithLock()).WithLock();
            
            

            this._stampVariantCache = new DictionaryCache<Tuple<StampIdentityDTO, StampColorIdentityDTO>, IStampVariant>(tuple =>
            {
                var stamp = tuple.Item1.ToDomainObject(this);

                var stampColor = tuple.Item2.ToDomainObject(this);

                if (stamp == null && stampColor == null)
                {
                    return null;
                }
                else if (stamp != null && stampColor != null)
                {
                    return stamp.GetByColor(stampColor);
                }
                else
                {
                    throw new Exception("StampError");
                }
            }).WithLock();

            this._equipmentClassCache = new DictionaryCache<IEquipment, IEquipmentClass>(equipmentClass => this.Context.GetEquipmentClass(equipmentClass)).WithLock();
        }



        public IApplicationContext Context { get; }


        public T GetById<T>(int id) 
            where T : class, IPersistentIdentityDomainObjectBase
        {
            return (T)this._cache[typeof(T)][id];
        }
        

        public IStampVariant GetStampVariant(StampIdentityDTO stampId, StampColorIdentityDTO stampColorId)
        {
            return this._stampVariantCache[Tuple.Create(stampId, stampColorId)];
        }

        public IEquipmentClass GetEquipmentClass(IEquipment equipmentClass)
        {
            if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

            return this._equipmentClassCache[equipmentClass];
        }


        private DictionaryCache<int, IPersistentIdentityDomainObjectBase> GetInternalCache(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var method = this.GetType().GetMethod(nameof(GetTypedInternalCache), BindingFlags.NonPublic | BindingFlags.Instance);

            return method.MakeGenericMethod(type).Invoke<DictionaryCache<int, IPersistentIdentityDomainObjectBase>>(this);
        }

        private DictionaryCache<int, IPersistentIdentityDomainObjectBase> GetTypedInternalCache<T>()
            where T : class, IPersistentIdentityDomainObjectBase
        {
            return new DictionaryCache<int, IPersistentIdentityDomainObjectBase>(id => this.Context.DataSource.GetById<T>(id));
        }
    }

    public interface IMappingService
    {
        IApplicationContext Context { get; }

        T GetById<T>(int id)
            where T : class, IPersistentIdentityDomainObjectBase;

        IStampVariant GetStampVariant(StampIdentityDTO stampId, StampColorIdentityDTO stampColorId);

        IEquipmentClass GetEquipmentClass(IEquipment equipmentClass);
    }
}