using Framework.Core;

namespace Framework.Reactive;

public record MergeResultEventArgs<T>(MergeResult<T, T> MergeResult);
