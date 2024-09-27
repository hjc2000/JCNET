using System.Text;

namespace JCNET.字符串处理;

/// <summary>
///		字符串扩展
/// </summary>
public static class StringExtension
{
	/// <summary>
	///		查找 target 在 self 中的位置
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target"></param>
	/// <param name="start_count">从此处开始查找</param>
	/// <returns></returns>
	public static int IndexOf(this ReadOnlySpan<char> self, ReadOnlySpan<char> target, int start_count)
	{
		ReadOnlySpan<char> sub_span = self[start_count..];
		int index = sub_span.IndexOf(target);
		if (index == -1)
		{
			return -1;
		}

		return index + start_count;
	}

	/// <summary>
	///		查找 target 在 self 中的位置
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target"></param>
	/// <param name="start_count">从此处开始查找</param>
	/// <returns></returns>
	public static int IndexOf(this ReadOnlyMemory<char> self, ReadOnlySpan<char> target, int start_count)
	{
		return self.Span.IndexOf(target, start_count);
	}

	/// <summary>
	///		查找 target 在 self 中的位置
	/// </summary>
	/// <param name="self"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	public static int IndexOf(this ReadOnlyMemory<char> self, ReadOnlySpan<char> target)
	{
		return self.IndexOf(target, 0);
	}

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

	/// <summary>
	///		将 match 之前的头部截掉。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match">从这里之前的部分要被截掉。</param>
	/// <param name="trim_match">截掉的部分是否包括 match 本身。true 表示包括，false 表示不包括。</param>
	/// <returns></returns>
	public static TrimResult TrimHeadBeforeFirstMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		int index = self.Span.IndexOf(match);
		if (index == -1)
		{
			return new TrimResult()
			{
				Success = false,
			};
		}

		if (trim_match)
		{
			// 截掉的部分包括 match 本身
			return new TrimResult()
			{
				Success = true,
				Remain = self[(index + match.Length)..],
			};
		}
		else
		{
			// 截掉的部分不包括 match 本身
			return new TrimResult()
			{
				Success = true,
				Remain = self[index..],
			};
		}
	}

	/// <summary>
	///		将 match 之前的头部截掉。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="trim_match"></param>
	/// <returns></returns>
	public static TrimResult TrimHeadBeforeFirstMatch(this string self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		return self.AsMemory().TrimHeadBeforeFirstMatch(match, trim_match);
	}

	/// <summary>
	///		将 self 从 match 处截掉尾巴。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match">从这里往后的尾巴将会被截掉。</param>
	/// <param name="trim_match">截掉的部分是否包括 match 本身。true 表示包括，false 表示不包括。</param>
	/// <returns></returns>
	public static TrimResult TrimTailAfterFirstMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		int index = self.Span.IndexOf(match);
		if (index == -1)
		{
			return new TrimResult()
			{
				Success = false,
			};
		}

		if (trim_match)
		{
			return new TrimResult()
			{
				Success = true,
				Remain = self[..index],
			};
		}
		else
		{
			return new TrimResult()
			{
				Success = true,
				Remain = self[..(index + match.Length)],
			};
		}
	}

	/// <summary>
	///		将 self 从 match 处截掉尾巴。
	///		<br/>* match 是从头部往后查找，找到第一个就算找到。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="trim_match"></param>
	/// <returns></returns>
	public static TrimResult TrimTailAfterFirstMatch(this string self,
		ReadOnlySpan<char> match, bool trim_match)
	{
		return self.AsMemory().TrimTailAfterFirstMatch(match, trim_match);
	}

	/// <summary>
	///		剪切字符串。
	///		<br/>* 将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///			   不包括 start_string 和 end_string 的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start_string"></param>
	/// <param name="end_string"></param>
	/// <returns></returns>
	public static TrimResult Cut(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> start_string, ReadOnlySpan<char> end_string)
	{
		TrimResult result = self.TrimHeadBeforeFirstMatch(start_string, true);
		if (!result.Success)
		{
			return result;
		}

		result = result.Remain.TrimTailAfterFirstMatch(end_string, true);
		return result;
	}

	/// <summary>
	///		剪切字符串。
	///		<br/>* 将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///			   不包括 start_string 和 end_string 的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="start_string"></param>
	/// <param name="end_string"></param>
	/// <returns></returns>
	public static TrimResult Cut(this string self,
		ReadOnlySpan<char> start_string, ReadOnlySpan<char> end_string)
	{
		return self.AsMemory().Cut(start_string, end_string);
	}

	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this ReadOnlyMemory<char> self, ReadOnlySpan<char> middle)
	{
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
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> middle)
	{
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
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this string self,
		ReadOnlySpan<char> middle)
	{
		return self.AsMemory().CutMiddleWholeMatch(middle);
	}

	/// <summary>
	///		全字匹配替换
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static ReadOnlyMemory<char> ReplaceWholeMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> match, ReadOnlySpan<char> replacement)
	{
		if (match == replacement)
		{
			return self;
		}

		StringBuilder sb = new();
		ReadOnlyMemory<char> remain = self;
		while (true)
		{
			CuttingMiddleResult result = remain.CutMiddleWholeMatch(match);
			if (!result.Success)
			{
				sb.Append(remain);
				return sb.ToString().AsMemory();
			}

			sb.Append(result.Left);
			sb.Append(replacement);
			remain = result.Right;
		}
	}

	/// <summary>
	///		全字匹配替换
	/// </summary>
	/// <param name="self"></param>
	/// <param name="match"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static ReadOnlyMemory<char> ReplaceWholeMatch(this string self,
		ReadOnlySpan<char> match, ReadOnlySpan<char> replacement)
	{
		return self.AsMemory().ReplaceWholeMatch(match, replacement);
	}

	/// <summary>
	///		查找字符串中出现 left_word 和 right_word 相邻的，两者之间仅由空白字符分隔的部分，
	///		将其替换为 replacement。
	///		<br/>* 例如：hello      world 的 2 个单词之间出现很多空格，换行，缩进等，
	///			   本函数能够识别出来，将这两个单词及其中间部分给替换成 replacement。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="left_word"></param>
	/// <param name="right_word"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static ReadOnlyMemory<char> ReplaceTwoWord(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> left_word, ReadOnlySpan<char> right_word,
		ReadOnlySpan<char> replacement)
	{
		int offset = 0;
		StringBuilder sb = new();
		while (true)
		{
			if (offset >= self.Length)
			{
				return sb.ToString().AsMemory();
			}

			int left_word_index = self.IndexOf(left_word, offset);
			if (left_word_index == -1)
			{
				sb.Append(self[offset..]);
				return sb.ToString().AsMemory();
			}

			if (left_word_index > 0)
			{
				// 找到了，并且不是开头，需要检查左边一个字符
				if (!self.Span[left_word_index - 1].IsWordSeperation())
				{
					// 左边一个字符不是分隔符
					// 递增偏移量，推进查找进度
					sb.Append(self[offset..(left_word_index + left_word.Length)]);
					offset = left_word_index + left_word.Length;
					continue;
				}
			}

			int right_word_index = self.IndexOf(right_word, left_word_index + left_word.Length);
			if (right_word_index == -1)
			{
				sb.Append(self[offset..]);
				return sb.ToString().AsMemory();
			}

			if (right_word_index + right_word.Length < self.Length)
			{
				// 找到了，并且不是最后一个字符，需要检查右边一个字符
				if (!self.Span[right_word_index + right_word.Length].IsWordSeperation())
				{
					// 右边一个字符不是分隔符
					sb.Append(self[offset..(right_word_index + right_word.Length)]);
					offset = right_word_index + right_word.Length;
					continue;
				}
			}

			if (self.ContainsNotWhiteSpaceChar(left_word_index + left_word.Length, right_word_index))
			{
				// 左单词和右单词之间存在非空白字符
				sb.Append(self[offset..(left_word_index + left_word.Length)]);
				offset = left_word_index + left_word.Length;
				continue;
			}

			// 经过了一道道关卡，到这里可以进行替换了
			sb.Append(self[offset..left_word_index]);
			sb.Append(replacement);
			offset = right_word_index + right_word.Length;
		} //while (true)
	}

	/// <summary>
	///		查找字符串中出现 left_word 和 right_word 相邻的，两者之间仅由空白字符分隔的部分，
	///		将其替换为 replacement。
	///		<br/>* 例如：hello      world 的 2 个单词之间出现很多空格，换行，缩进等，
	///			   本函数能够识别出来，将这两个单词及其中间部分给替换成 replacement。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="left_word"></param>
	/// <param name="right_word"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static ReadOnlyMemory<char> ReplaceTwoWord(this string self,
		ReadOnlySpan<char> left_word, ReadOnlySpan<char> right_word,
		ReadOnlySpan<char> replacement)
	{
		return self.AsMemory().ReplaceTwoWord(left_word, right_word, replacement);
	}
}

/// <summary>
///		去除内容后的结果
/// </summary>
public class TrimResult
{
	/// <summary>
	///		去除内容成功
	/// </summary>
	public bool Success { get; set; } = false;

	/// <summary>
	///		去除内容成功后，剩余的部分放在这里
	/// </summary>
	public ReadOnlyMemory<char> Remain { get; set; } = new Memory<char>();
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
