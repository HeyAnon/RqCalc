using Framework.Core;

namespace Framework.DataBase.TypeBuilder;

internal class InterfaceTypeMap : TypeMap, IEquatable<InterfaceTypeMap>, ISwitchNameObject<InterfaceTypeMap>
{
    public InterfaceTypeMap(Type sourceType, string name, IEnumerable<KeyValuePair<string, Type>> members)
        : base(name, members) =>
        this.SourceType = sourceType;

    public InterfaceTypeMap(Type sourceType, string name, IEnumerable<TypeMapMember> members)
        : base(name, members) =>
        this.SourceType = sourceType;

    public Type SourceType { get; }


    public override int GetHashCode() => base.GetHashCode() ^ this.SourceType.GetHashCode();

    public override bool Equals(object obj) => this.Equals(obj as InterfaceTypeMap);

    public bool Equals(InterfaceTypeMap other) => base.Equals(other) && this.SourceType == other?.SourceType;

    public new InterfaceTypeMap SwitchName(string newName) => new(this.SourceType, newName, this.Members);
}