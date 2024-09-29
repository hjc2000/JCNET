namespace JCNET.字符串处理;

/// <summary>
///		字符串扩展
/// </summary>
public static class StringExtension
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

	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this ReadOnlyMemory<char> self, ReadOnlySpan<char> middle)
	{
		if (self.Length == 0 || middle.Length == 0)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		int index = self.IndexOf(middle);
		if (index == -1)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		return new CuttingMiddleResult()
		{
			Success = true,
			Left = self[..index],
			Right = self[(index + middle.Length)..]
		};
	}

	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this string self, ReadOnlySpan<char> middle)
	{
		return self.AsMemory().CutMiddle(middle);
	}

	/// <summary>
	///		全字匹配地切除中间部分
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> middle)
	{
		if (self.Length == 0 || middle.Length == 0)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		int finding_offset = 0;
		while (true)
		{
			if (finding_offset >= self.Length)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			int index = self.IndexOf(middle, finding_offset);
			if (index == -1)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			// 找到了，需要进一步验证是否全字匹配
			if (index > 0)
			{
				// middle 不是在开头位置，需要检查前一个字符
				if (!self.Span[index - 1].IsWordSeperation())
				{
					// 前一个字符不是分隔符，不满足全字匹配
					finding_offset = index + middle.Length;
					continue;
				}
			}

			if (index + middle.Length < self.Length)
			{
				// middle 不是在结尾位置，需要检查后一个字符
				if (!self.Span[index + middle.Length].IsWordSeperation())
				{
					// 后一个字符不是分隔符，不满足全字匹配
					finding_offset = index + middle.Length;
					continue;
				}
			}

			// 经过验证，是全字匹配的
			return new CuttingMiddleResult()
			{
				Success = true,
				Left = self[..index],
				Right = self[(index + middle.Length)..],
			};
		}
	}

	/// <summary>
	///		全字匹配地切除中间部分
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this string self,
		ReadOnlySpan<char> middle)
	{
		return self.AsMemory().CutMiddleWholeMatch(middle);
	}
}

/// <summary>
///		将字符串切除中间部分后的结果。
/// </summary>
public class CuttingMiddleResult
{
	/// <summary>
	///		是否切除成功。
	///		<br/>* 如果中间部分不存在就会切除失败
	/// </summary>
	public bool Success { get; set; } = false;

	/// <summary>
	///		切除中间部分后留下的左边部分
	/// </summary>
	public ReadOnlyMemory<char> Left { get; set; } = new Memory<char>();

	/// <summary>
	///		切除中间部分后留下的右边部分
	/// </summary>
	public ReadOnlyMemory<char> Right { get; set; } = new Memory<char>();

	/// <summary>
	///		转化为字符串
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return Json.ToJson(this);
	}
}
