using System.Text;

namespace JCNET;

/// <summary>
///		基于 StringBuilder 的 TextWriter。
/// </summary>
public class StringBuilderLogWriter : TextWriter
{
	/// <summary>
	///		构造函数。设置 MaxCapacity 为 10000。
	/// </summary>
	public StringBuilderLogWriter()
	{
		MaxCapacity = 10000;
		_sb.Capacity = MaxCapacity;
	}

	/// <summary>
	///		构造函数。
	/// </summary>
	/// <param name="capacity">用来设置 MaxCapacity。</param>
	public StringBuilderLogWriter(int capacity)
	{
		_sb.Capacity = capacity;
		_sb.Capacity = MaxCapacity;
	}

	/// <summary>
	///		最多允许容纳的字符数。
	/// </summary>
	public int MaxCapacity { get; set; }

	/// <summary>
	///		字符编码。本类为：Encoding.Unicode
	/// </summary>
	public override Encoding Encoding
	{
		get
		{
			return Encoding.Unicode;
		}
	}

	private readonly StringBuilder _sb = new();

	/// <summary>
	///		写入时会触发此事件。
	/// </summary>
	public event Action? WriteEvent;

	/// <summary>
	///		写一个字符串。
	/// </summary>
	/// <param name="value"></param>
	public override void Write(string? value)
	{
		_sb.Append(value);
		if (_sb.Length > MaxCapacity)
		{
			_sb.Remove(0, _sb.Length - MaxCapacity);
		}

		WriteEvent?.Invoke();
	}

	/// <summary>
	///		冲洗时会触发此事件。
	/// </summary>
	public event Action? FlushEvent;

	/// <summary>
	///		冲洗。
	/// </summary>
	public override void Flush()
	{
		base.Flush();
		FlushEvent?.Invoke();
	}

	/// <summary>
	///		返回 StringBuilder 构建的字符串。
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return _sb.ToString();
	}
}
