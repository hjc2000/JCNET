namespace JCNET.字符串处理;

/// <summary>
///		提供一些扩展的 Trim 方法
/// </summary>
public static class TrimExtension
{
	/// <summary>
	///		将 match 之前的头部截掉。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	///		<br/>* match 如果为空字符串，视为没有任何匹配，也就是原封不动将 self 返回。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match">从这里之前的部分要被截掉。</param>
	/// <param name="trim_match">截掉的部分是否包括 match 本身。true 表示包括，false 表示不包括。</param>
	/// <returns>返回去掉头部之后的结果。如果 match 没有匹配，则没有头部，直接返回 self。</returns>
	public static ReadOnlyMemory<char> TrimHeadBeforeFirstMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		if (self.Length == 0 || match.Length == 0)
		{
			return self;
		}

		int index = self.IndexOf(match);
		if (index == -1)
		{
			return self;
		}

		if (trim_match)
		{
			// 截掉的部分包括 match 本身
			return self[(index + match.Length)..];
		}
		else
		{
			// 截掉的部分不包括 match 本身
			return self[index..];
		}
	}

	/// <summary>
	///		将 match 之前的头部截掉。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	///		<br/>* match 如果为空字符串，视为没有任何匹配，也就是原封不动将 self 返回。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="trim_match"></param>
	/// <returns>返回去掉头部之后的结果。如果 match 没有匹配，则没有头部，直接返回 self。</returns>
	public static ReadOnlyMemory<char> TrimHeadBeforeFirstMatch(this string self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		return self.AsMemory().TrimHeadBeforeFirstMatch(match, trim_match);
	}

	/// <summary>
	///		将 self 从 match 处截掉尾巴。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	///		<br/>* match 如果为空字符串，视为没有任何匹配，也就是原封不动将 self 返回。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match">从这里往后的尾巴将会被截掉。</param>
	/// <param name="trim_match">截掉的部分是否包括 match 本身。true 表示包括，false 表示不包括。</param>
	/// <returns>返回去掉尾巴后的结果。如果 match 没有匹配，则没有尾巴，直接返回 self。</returns>
	public static ReadOnlyMemory<char> TrimTailAfterFirstMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		if (self.Length == 0 || match.Length == 0)
		{
			return self;
		}

		int index = self.Span.IndexOf(match);
		if (index == -1)
		{
			return self;
		}

		if (trim_match)
		{
			return self[..index];
		}
		else
		{
			return self[..(index + match.Length)];
		}
	}

	/// <summary>
	///		将 self 从 match 处截掉尾巴。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	///		<br/>* match 如果为空字符串，视为没有任何匹配，也就是原封不动将 self 返回。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="trim_match"></param>
	/// <returns>返回去掉尾巴后的结果。如果 match 没有匹配，则没有尾巴，直接返回 self。</returns>
	public static ReadOnlyMemory<char> TrimTailAfterFirstMatch(this string self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		return self.AsMemory().TrimTailAfterFirstMatch(match, trim_match);
	}
}
