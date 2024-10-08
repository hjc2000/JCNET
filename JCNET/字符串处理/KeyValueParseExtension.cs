﻿namespace JCNET.字符串处理;

/// <summary>
///		解析字符串中的键值对
/// </summary>
public static class KeyValueParseExtension
{
	/// <summary>
	///		解析 key = value 形式的字符串。key, value 支持使用单引号、双引号包起来，就像：
	///		key = "value" , key = 'value' , 'key' = 'value' ...... 等
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static ParseKeyValueResult? ParseKeyValueEx(this string str)
	{
		try
		{
			str = str.Trim();
			int index = str.IndexOf('=');

			// 获取等于号前面的内容，可能长这样： "key"，'key'，key
			string key = str[..index];

			// 获取等于号后面的内容
			string value = str[(index + 1)..];

			ParseKeyValueResult result = new()
			{
				Key = Trim(key),
				Value = Trim(value)
			};
			return result;
		}
		catch
		{
			return null;
		}
	}

	/// <summary>
	///		去除字符串开头和结尾位置的引号和空白符
	/// </summary>
	/// <returns></returns>
	private static string Trim(string value)
	{
		if (value.StartsWith('\"') || value.StartsWith('\''))
		{
			value = value[1..];
		}

		if (value.EndsWith('\"') || value.EndsWith('\''))
		{
			value = value[..^1];
		}

		value = value.Trim();
		return value;
	}
}

/// <summary>
///		键值对解析结果
/// </summary>
public class ParseKeyValueResult
{
	/// <summary>
	///		解析出来的键
	/// </summary>
	public string Key { get; set; } = string.Empty;

	/// <summary>
	///		解析出来的值
	/// </summary>
	public string Value { get; set; } = string.Empty;
}
