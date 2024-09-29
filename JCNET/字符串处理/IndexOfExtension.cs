namespace JCNET.字符串处理;

/// <summary>
///		提供一些扩展的 IndexOf
/// </summary>
public static class IndexOfExtension
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
}
