using System.Text;

namespace JCNET.字符串处理;

/// <summary>
///		字符串替换扩展
/// </summary>
public static class StringReplacingExtension
{
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
}
