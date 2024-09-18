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

		int end_index = value.IndexOf(end_string);
		if (end_index == -1)
		{
			return string.Empty;
		}

		start_index += start_string.Length;
		if (start_index >= end_index)
		{
			return string.Empty;
		}

		return value[start_index..end_index];
	}
}
