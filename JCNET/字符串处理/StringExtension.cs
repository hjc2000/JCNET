namespace JCNET.字符串处理;

/// <summary>
///		字符串扩展
/// </summary>
public static class StringExtension
{
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
