namespace JCNET.字符串处理;

public struct ParseRangeResult
{
	public ParseRangeResult() { }

	public string Left { get; set; } = string.Empty;
	public string Right { get; set; } = string.Empty;
}

public static class RangeParseExtension
{
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
