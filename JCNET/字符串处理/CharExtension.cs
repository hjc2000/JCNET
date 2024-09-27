namespace JCNET.字符串处理;

/// <summary>
///		字符扩展
/// </summary>
public static class CharExtension
{
	/// <summary>
	///		检查字符 self 在 IDE 全字匹配规则中，是否是单词的分隔符。
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static bool IsWordSeperation(this char self)
	{
		if (self == '_')
		{
			return false;
		}

		if (char.IsWhiteSpace(self))
		{
			return true;
		}

		if (char.IsPunctuation(self))
		{
			return true;
		}

		return false;
	}
}
