namespace JCNET.容器;

public class AsyncEnumerable<T> : IAsyncEnumerable<T>
{
	public AsyncEnumerable(IEnumerable<T> enumerable)
	{
		_enumerable = enumerable;
	}

	private IEnumerable<T> _enumerable;

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
	{
		return new AsyncEnumerator<T>(_enumerable.GetEnumerator());
	}
}

/// <summary>
///		接收同步迭代器，包装成异步迭代器。
/// </summary>
/// <typeparam name="T"></typeparam>
internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
	public AsyncEnumerator(IEnumerator<T> enumerator)
	{
		_enumerator = enumerator;
	}

	private IEnumerator<T> _enumerator;

	public async ValueTask<bool> MoveNextAsync()
	{
		await Task.CompletedTask;
		return _enumerator.MoveNext();
	}

	public T Current
	{
		get
		{
			return _enumerator.Current;
		}
	}

	public async ValueTask DisposeAsync()
	{
		await Task.CompletedTask;
		_enumerator.Dispose();
	}
}

public static class IEnumerableExtension
{
	public static IAsyncEnumerable<T> AsIAsyncEnumerableEx<T>(this IEnumerable<T> enumerable)
	{
		return new AsyncEnumerable<T>(enumerable);
	}
}
