using Framework.Core;

namespace Framework.DataBase.TypeBuilder;

internal class InterfaceTypeMap : TypeMap, IEquatable<InterfaceTypeMap>, ISwitchNameObject<InterfaceTypeMap>
{
    public InterfaceTypeMap(Type sourceType, string name, IEnumerable<KeyValuePair<string, Type>> members)
        : base(name, members)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        this.SourceType = sourceType;
    }

    public InterfaceTypeMap(Type sourceType, string name, IEnumerable<TypeMapMember> members)
        : base(name, members)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        this.SourceType = sourceType;
    }


    public Type SourceType { get; }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.SourceType.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as InterfaceTypeMap);
    }

    public bool Equals(InterfaceTypeMap other)
    {
        return base.Equals(other) && this.SourceType == other?.SourceType;
    }

    public new InterfaceTypeMap SwitchName(string newName)
    {
        return new InterfaceTypeMap(this.SourceType, newName, this.Members);
    }
}