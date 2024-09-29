namespace JCNET.字符串处理;

/// <summary>
///		提供一些扩展的 Contains 方法
/// </summary>
public static class ContainsExtension
{
	/// <summary>
	///		self 中存在非空白字符
	/// </summary>
	/// <param name="self"></param>
	/// <returns>存在非空白字符则返回 true，否则返回 false。</returns>
	public static bool ContainsNotWhiteSpaceChar(this ReadOnlySpan<char> self)
	{
		for (int i = 0; i < self.Length; i++)
		{
			if (!char.IsWhiteSpace(self[i]))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	///		self 的 [start, end) 上存在非空白字符
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns></returns>
	public static bool ContainsNotWhiteSpaceChar(this ReadOnlySpan<char> self, int start, int end)
	{
		return self[start..end].ContainsNotWhiteSpaceChar();
	}

	/// <summary>
	///		self 中存在非空白字符
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static bool ContainsNotWhiteSpaceChar(this ReadOnlyMemory<char> self)
	{
		return self.Span.ContainsNotWhiteSpaceChar();
	}

	/// <summary>
	///		self 的 [start, end) 上存在非空白字符
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns>存在非空白字符则返回 true，否则返回 false。</returns>
	public static bool ContainsNotWhiteSpaceChar(this ReadOnlyMemory<char> self, int start, int end)
	{
		return self.Span.ContainsNotWhiteSpaceChar(start, end);

	}
}
