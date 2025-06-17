using System.Linq.Expressions;
using Framework.Core;
using Framework.Persistent;

namespace RqCalc.DataBase.EntityFramework._DBContext
{
    internal class ExpandInterfaceVisitor : ExpressionVisitor
    {
        private readonly Type projectionType;


        public ExpandInterfaceVisitor(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            this.projectionType = interfaceType;
        }


        public override Expression Visit(Expression node)
        {
            var accumVisitor = this.projectionType.GetReferencedTypes(property => property.PropertyType.IsInterface)
                .Select(refType => new OverrideCallInterfacePropertiesVisitor(refType))
                .Concat(new ExpressionVisitor[] { ExpandPathVisitor.Value, ExpandExplicitPropertyVisitor.Value, })
                .ToCyclic();

            return accumVisitor.Visit(node);
        }
    }
}