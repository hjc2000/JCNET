namespace JCNET.容器;

/// <summary>
///		字典扩展
/// </summary>
public static class DictionaryExtension
{
	/// <summary>
	///		将 pairs 添加到 self 中。
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="self"></param>
	/// <param name="pairs"></param>
	public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> self,
		IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		where TKey : notnull
	{
		foreach (KeyValuePair<TKey, TValue> pair in pairs)
		{
			self.Add(pair.Key, pair.Value);
		}
	}

	/// <summary>
	///		将 pair 添加到 self 中。
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="self"></param>
	/// <param name="pair"></param>
	public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> self,
		KeyValuePair<TKey, TValue> pair)
		where TKey : notnull
	{
		self.Add(pair.Key, pair.Value);
	}

	/// <summary>
	///		从 self 中移除 pairs 中具有的键。
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="self"></param>
	/// <param name="pairs"></param>
	public static void Remove<TKey, TValue>(this Dictionary<TKey, TValue> self,
		IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		where TKey : notnull
	{
		foreach (KeyValuePair<TKey, TValue> pair in pairs)
		{
			self.Remove(pair.Key);
		}
	}
}
