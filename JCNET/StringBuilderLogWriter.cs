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
	///		写一个字符串。
	/// </summary>
	/// <param name="value"></param>
	public override void Write(string? value)
	{
		if (value is null)
		{
			return;
		}

		if (value.Length > MaxCapacity)
		{
			// 要放进来的字符串长度超过 MaxCapacity，则清空 _sb，然后将 value 的后段放进去。
			_sb.Clear();
			_sb.Append(value[(value.Length - MaxCapacity)..]);
			return;
		}

		if (_sb.Length + value.Length > MaxCapacity)
		{
			// 将 value 追加到 _sb 后会导致长度超过 MaxCapacity

			// 计算需要将头部移除的长度
			int remove_length = Math.Min(_sb.Length, value.Length);

			// 一次至少移除一半，不要频繁进行小片段的移除，则会降低性能。
			remove_length = Math.Max(remove_length, _sb.Length / 2);
			_sb.Remove(0, remove_length);
		}

		_sb.Append(value);
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
