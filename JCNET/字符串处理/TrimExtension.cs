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

	/// <summary>
	///		将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///		不包括 start_string 和 end_string 的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start_string"></param>
	/// <param name="end_string"></param>
	/// <returns>
	///		<br/>* 如果 start_string 或 end_string 不存在，则返回空字符串。
	///		<br/>* 如果 start_string 或 end_string 为空字符串，视为没有任何匹配，也就是他们中间没有
	///			   任何内容，于是返回空字符串。
	///	</returns>
	public static ReadOnlyMemory<char> GetBetween(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> start_string, ReadOnlySpan<char> end_string)
	{
		if (self.Length == 0 || start_string.Length == 0 || end_string.Length == 0)
		{
			return string.Empty.AsMemory();
		}

		int origin_length = self.Length;
		self = self.TrimHeadBeforeFirstMatch(start_string, true);
		if (self.Length == origin_length)
		{
			// 经过裁剪后长度没有任何变化，说明 start_string 和 end_string 都不存在
			return string.Empty.AsMemory();
		}

		origin_length = self.Length;
		self = self.TrimTailAfterFirstMatch(end_string, true);
		if (self.Length == origin_length)
		{
			// 经过裁剪后长度没有任何变化，说明 start_string 和 end_string 都不存在
			return string.Empty.AsMemory();
		}

		return self;
	}

	/// <summary>
	///		将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///		不包括 start_string 和 end_string 的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start_string"></param>
	/// <param name="end_string"></param>
	/// <returns>
	///		<br/>* 如果 start_string 或 end_string 不存在，则返回空字符串。
	///		<br/>* 如果 start_string 或 end_string 为空字符串，视为没有任何匹配，也就是他们中间没有
	///			   任何内容，于是返回空字符串。
	///	</returns>
	public static ReadOnlyMemory<char> GetBetween(this string self,
		ReadOnlySpan<char> start_string, ReadOnlySpan<char> end_string)
	{
		return self.AsMemory().GetBetween(start_string, end_string);
	}
}
