using System.Runtime.Serialization;

namespace RqCalc.Core;

[DataContract]
public class CardGroupInfo
{
    public CardGroupInfo(string key, string value, int orderKey)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

        this.Key = key;
        this.Value = value;
        this.OrderKey = orderKey;
    }


    [DataMember]
    public string Key { get; private set; }

    [DataMember]
    public string Value { get; private set; }

    [DataMember]
    public int OrderKey { get; private set; }
}