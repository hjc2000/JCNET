namespace JCNET.流;

/// <summary>
///		能够将多个流在字节层面拼接在一起，形成一个字节流。
/// </summary>
public partial class JoinedStream : Stream
{
	public JoinedStream(IAsyncEnumerator<Stream> stream_enumerator)
	{
		StreamEnumerator = stream_enumerator;
	}

	public JoinedStream(IAsyncEnumerable<Stream> stream_souce)
		: this(stream_souce.GetAsyncEnumerator())
	{

	}

	private bool _disposed = false;
	public override async ValueTask DisposeAsync()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		GC.SuppressFinalize(this);

		await base.DisposeAsync();
		if (_currentStream != null)
		{
			await _currentStream.DisposeAsync();
		}
	}
}

public partial class JoinedStream
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="buffer"></param>
	/// <param name="offset"></param>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="TaskCanceledException"></exception>
	public override async Task<int> ReadAsync(byte[] buffer, int offset, int count,
		CancellationToken cancellationToken)
	{
		Memory<byte> memory = new(buffer, offset, count);
		return await ReadAsync(memory, cancellationToken);
	}

	public override async ValueTask<int> ReadAsync(Memory<byte> buffer,
		CancellationToken cancellationToken = default)
	{
		while (true)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				throw new TaskCanceledException();
			}

			// 当前流为空，需要加载一个新的流
			if (_currentStream == null)
			{
				bool move_next_result = await StreamEnumerator.MoveNextAsync();
				if (!move_next_result)
				{
					return 0;
				}

				_currentStream = StreamEnumerator.Current;
			}

			// 执行到这里说明 _currentStream 不为空。
			int haveRead = await _currentStream.ReadAsync(buffer, cancellationToken);
			if (haveRead == 0)
			{
				// 当前流已经结束了，先释放此流
				await _currentStream.DisposeAsync();
				_currentStream = null;
				continue;
			}

			_position += haveRead;
			return haveRead;
		}
	}

	private Stream? _currentStream = null;
	private IAsyncEnumerator<Stream> StreamEnumerator { get; set; }
}

public partial class JoinedStream
{
	#region 不支持的功能
	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}
	public override void Flush()
	{
		throw new NotSupportedException();
	}
	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}
	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}
	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
	#endregion

	public override bool CanRead
	{
		get
		{
			return true;
		}
	}

	public override bool CanSeek
	{
		get
		{
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			return false;
		}
	}

	private long _position;
	public override long Position
	{
		get
		{
			return _position;
		}

		set
		{
			throw new NotSupportedException();
		}
	}
}
