using Framework.Core;

namespace Framework.Reactive;

public class LazyCompositeDisposable(Func<IEnumerable<IDisposable>> getItems) : IDisposable
{
    public LazyCompositeDisposable(IEnumerable<IDisposable> items)
        : this(() => items)
    {

    }


    public void Dispose() => getItems().Foreach(item => item.Dispose());
}
