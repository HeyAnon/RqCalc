using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

using Framework.Persistent;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    internal class ExpandInterfaceVisitor : ExpressionVisitor
    {
        private readonly Type _projectionType;


        public ExpandInterfaceVisitor(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            this._projectionType = interfaceType;
        }


        public override Expression Visit(Expression node)
        {
            var accumVisitor = this._projectionType.GetReferencedTypes(property => property.PropertyType.IsInterface)
                .Select(refType => new OverrideCallInterfacePropertiesVisitor(refType))
                .Concat(new ExpressionVisitor[] { ExpandPathVisitor.Value, ExpandExplicitPropertyVisitor.Value, })
                .ToCyclic();

            return accumVisitor.Visit(node);
        }
    }
}