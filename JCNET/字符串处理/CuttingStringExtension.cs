namespace JCNET.字符串处理;

/// <summary>
///		剪切字符串的扩展
/// </summary>
public static class CuttingStringExtension
{
	/// <summary>
	///		剪切字符串。将字符串掐头去尾，取出位于 start_string 和 end_string 之间的子字符串，
	///		不包括 start_string 和 end_string 的内容。
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
