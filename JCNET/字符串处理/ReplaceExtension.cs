using System.Text;

namespace JCNET.字符串处理;

/// <summary>
///		提供一些扩展的 Replace 方法
/// </summary>
public static class ReplaceExtension
{
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
		if (self.Length == 0 || match.Length == 0)
		{
			return self;
		}

		if (match == replacement)
		{
			return self;
		}

		StringBuilder sb = new();
		ReadOnlyMemory<char> remain = self;
		while (true)
		{
			if (remain.Length == 0)
			{
				return sb.ToString().AsMemory();
			}

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
		if (self.Length == 0 || left_word.Length == 0 || right_word.Length == 0)
		{
			return self;
		}

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
