using System.Text;

namespace JCNET.字符串处理;

/// <summary>
///		字符串扩展
/// </summary>
public static class StringExtension
{
	/// <summary>
	///		剪切字符串。
	///		<br/>* 将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///			   不包括 start_string 和 end_string 的内容。
	/// </summary>
	/// <param name="value"></param>
	/// <param name="start_string"></param>
	/// <param name="end_string"></param>
	/// <returns></returns>
	public static string Cut(this string value, string start_string, string end_string)
	{
		int start_index = value.IndexOf(start_string);
		if (start_index == -1)
		{
			return string.Empty;
		}

		start_index += start_string.Length;
		int end_index = value.IndexOf(end_string, start_index);
		if (end_index == -1)
		{
			return string.Empty;
		}

		if (start_index >= end_index)
		{
			return string.Empty;
		}

		return value[start_index..end_index];
	}

	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	/// </summary>
	/// <param name="value"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this string value, string middle)
	{
		string[] sub_strings = value.Split(middle, 2);
		if (sub_strings.Length < 2)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		return new CuttingMiddleResult()
		{
			Success = true,
			Left = sub_strings[0],
			Right = sub_strings[1],
		};
	}

	/// <summary>
	///		全字匹配地切除中间部分
	/// </summary>
	/// <param name="value"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this string value, string middle)
	{
		int finding_offset = 0;
		while (true)
		{
			if (finding_offset >= value.Length)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			int index = value.IndexOf(middle, finding_offset);
			if (index == -1)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			// 找到了，但这时候是非全字匹配的
			finding_offset = index + middle.Length;
			if (index > 0)
			{
				// middle 不是在开头位置，需要检查前一个字符
				if (char.IsLetter(value[index - 1]) ||
					char.IsNumber(value[index - 1]) ||
					value[index - 1] == '_')
				{
					// 前一个字符是单词或下划线，不满足全字匹配
					continue;
				}
			}

			if (index + middle.Length < value.Length)
			{
				// middle 不是在结尾位置，需要检查后一个字符
				if (char.IsLetter(value[index + middle.Length]) ||
					char.IsNumber(value[index + middle.Length]) ||
					value[index + middle.Length] == '_')
				{
					// 后一个字符是单词或下划线，不满足全字匹配
					continue;
				}
			}

			return new CuttingMiddleResult()
			{
				Success = true,
				Left = value[..index],
				Right = value[(index + middle.Length)..],
			};
		}
	}

	/// <summary>
	///		全字匹配替换
	/// </summary>
	/// <param name="str"></param>
	/// <param name="match"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static string ReplaceWholeMatch(this string str, string match, string replacement)
	{
		if (match == replacement)
		{
			return str;
		}

		string remain = str;
		StringBuilder sb = new();
		while (true)
		{
			CuttingMiddleResult result = remain.CutMiddleWholeMatch(match);
			if (!result.Success)
			{
				sb.Append(remain);
				return sb.ToString();
			}

			sb.Append($"{result.Left}{replacement}");
			remain = result.Right;
		}
	}

	/// <summary>
	///		查找字符串中出现 left_word 和 right_word 相邻的，两者之间仅由空白字符分隔的部分，
	///		将其替换为 replacement。
	///		<br/>* 例如：hello      world 的 2 个单词之间出现很多空格，换行，缩进等，
	///			   本函数能够识别出来，将这两个单词及其中间部分给替换成 replacement。
	/// </summary>
	/// <param name="str"></param>
	/// <param name="left_word"></param>
	/// <param name="right_word"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public static string ReplaceTwoWord(this string str,
		string left_word, string right_word, string replacement)
	{
		string remain = str;
		StringBuilder sb = new();
		while (true)
		{
			if (remain.Length == 0)
			{
				return sb.ToString();
			}

			int left_word_index = remain.IndexOf(left_word);
			if (left_word_index == -1)
			{
				sb.Append(remain);
				return sb.ToString();
			}

			if (left_word_index > 0)
			{
				// 找到了，并且不是开头，需要检查左边一个字符
				if (!char.IsWhiteSpace(remain[left_word_index - 1]))
				{
					// 左边一个字符不是空白字符
					sb.Append(remain[..(left_word_index + left_word.Length)]);
					remain = remain[(left_word_index + left_word.Length)..];
					continue;
				}
			}

			int right_word_index = remain.IndexOf(right_word);
			if (right_word_index == -1)
			{
				sb.Append(remain);
				return sb.ToString();
			}

			if (right_word_index + right_word.Length < remain.Length)
			{
				// 找到了，并且不是最后一个字符，需要检查右边一个字符
				if (!char.IsWhiteSpace(remain[right_word_index + right_word.Length]))
				{
					// 右边一个字符不是空白字符
					sb.Append(remain[..(right_word_index + right_word.Length)]);
					remain = remain[(right_word_index + right_word.Length)..];
					continue;
				}
			}

			if (remain.ContainsNotWhiteSpaceChar(left_word_index + left_word.Length, right_word_index))
			{
				// 左单词和右单词之间存在非空白字符
				sb.Append(remain[..(right_word_index + right_word.Length)]);
				remain = remain[(right_word_index + right_word.Length)..];
				continue;
			}

			// 经过了一道道关卡，到这里可以进行替换了
			sb.Append(remain[..left_word_index]);
			sb.Append(replacement);
			remain = remain[(right_word_index + right_word.Length)..];
		} //while (true)
	}

	/// <summary>
	///		str 中存在非空白字符
	/// </summary>
	/// <param name="str"></param>
	/// <returns>存在非空白字符则返回 true，否则返回 false。</returns>
	public static bool ContainsNotWhiteSpaceChar(this string str)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (!char.IsWhiteSpace(str[i]))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	///		str 的 [start, end) 上存在非空白字符
	/// </summary>
	/// <param name="str"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns>存在非空白字符则返回 true，否则返回 false。</returns>
	public static bool ContainsNotWhiteSpaceChar(this string str, int start, int end)
	{
		for (int i = start; i < end; i++)
		{
			if (!char.IsWhiteSpace(str[i]))
			{
				return true;
			}
		}

		return false;
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
	public string Left { get; set; } = string.Empty;

	/// <summary>
	///		切除中间部分后留下的右边部分
	/// </summary>
	public string Right { get; set; } = string.Empty;

	/// <summary>
	///		转化为字符串
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return Json.ToJson(this);
	}
}
