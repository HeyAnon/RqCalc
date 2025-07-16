namespace RqCalc.Application.IndexedDict;

public interface IIndexedDict<T> : IEnumerable<T>
{
    int BitSize { get; }

    int Count { get; }

    T this[int index] { get; }

    int this[T value] { get; }
}