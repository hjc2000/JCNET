using System.Text;

namespace JCNET;

/// <summary>
///		基于 StringBuilder 的 TextWriter。
/// </summary>
public class StringBuilderLogWriter : TextWriter
{
	/// <summary>
	///		构造函数。内部的 StringBuilder 的容量为 10000 个字符。
	/// </summary>
	public StringBuilderLogWriter()
	{
		_sb.Capacity = 10000;
	}

	/// <summary>
	///		构造函数。
	/// </summary>
	/// <param name="capacity">用来设置内部的 StringBuilder 的容量。</param>
	public StringBuilderLogWriter(int capacity)
	{
		_sb.Capacity = capacity;
	}

	/// <summary>
	///		转发内部的 StringBuilder 的 Capacity。
	/// </summary>
	public int Capacity
	{
		get
		{
			return _sb.Capacity;
		}
		set
		{
			_sb.Capacity = value;
		}
	}

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
