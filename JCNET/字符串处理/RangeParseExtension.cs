namespace JCNET.字符串处理;

/// <summary>
///		解析字符串中的范围表达式。
/// </summary>
public static class RangeParseExtension
{
	/// <summary>
	///		解析字符串中的范围表达式
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static ParseRangeResult? ParseRange(this string value)
	{
		int index = value.IndexOf('-');
		if (index < 0)
		{
			// 找不到横杠，直接返回失败
			return null;
		}

		try
		{
			ParseRangeResult result = new()
			{
				Left = value[0..index],
				Right = value[(index + 1)..]
			};

			return result;
		}
		catch
		{
			return null;
		}
	}
}

/// <summary>
///		范围解析结果。
/// </summary>
public class ParseRangeResult
{
	/// <summary>
	///		范围的左端点
	/// </summary>
	public string Left { get; set; } = string.Empty;

	/// <summary>
	///		范围的右端点
	/// </summary>
	public string Right { get; set; } = string.Empty;
}
