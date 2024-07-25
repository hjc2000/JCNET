using Microsoft.Extensions.DependencyInjection;
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
	///		写 1 个字符。
	/// </summary>
	/// <remarks>
	///		至少需要实现这个。这个重载是其他写函数的基础，本函数实现了，其他写函数就都能用了。
	///		当然，其他写函数都是虚函数，也就是可以单独拎出来重载。
	/// </remarks>
	/// <param name="value"></param>
	public override void Write(char value)
	{
		if (_sb.Length + 1 > MaxCapacity)
		{
			// 再追加 1 个字符，_sb 就溢出了，此时需要移除头部。
			_sb.Remove(0, _sb.Length / 2);
		}

		_sb.Append(value);
	}

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

		/* 执行到这里说明 _sb 至少在为空的状态下能够容纳整个 value，所以只要删除头部的子字符串，
		 * 腾出空间就行了。
		 */
		if (_sb.Length + value.Length > MaxCapacity)
		{
			// 将 value 追加到 _sb 后会导致长度超过 MaxCapacity
			RemoveHeadSafly(Math.Max(value.Length, _sb.Length / 2));
		}

		_sb.Append(value);
	}

	/// <summary>
	///		安全地移除头部的指定长度的子字符串。
	/// </summary>
	/// <param name="length">
	///		要移除的头部的子字符串的长度。
	///		如果指定的值超过了现有字符串的长度，则会将现有字符串清空，不会导致异常。
	/// </param>
	public void RemoveHeadSafly(int length)
	{
		if (length > _sb.Length)
		{
			_sb.Clear();
			return;
		}

		_sb.Remove(0, length);
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

/// <summary>
///		StringBuilderLogWriter 为 IServiceCollection 提供的扩展方法。
/// </summary>
public static class StringBuilderLogWriter_IServiceCollectionExtension
{
	/// <summary>
	///		构造一个 StringBuilderLogWriter 对象，以单例模式添加到服务中，并设置为
	///		Console 的输出。
	/// </summary>
	/// <param name="service"></param>
	public static void AddAddSingletonAndSetAsOut(this IServiceCollection service)
	{
		StringBuilderLogWriter writer = new();
		Console.SetOut(writer);
		service.AddSingleton((s) =>
		{
			return writer;
		});
	}
}
