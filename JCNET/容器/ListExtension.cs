namespace JCNET.容器;

/// <summary>
///		List 扩展
/// </summary>
public static class ListExtension
{
	/// <summary>
	///		将 to_remove 的项目从 self 中移除。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="self"></param>
	/// <param name="to_remove"></param>
	/// <returns></returns>
	public static List<T> Remove<T>(this List<T> self, IEnumerable<T> to_remove)
	{
		foreach (T item in to_remove)
		{
			self.Remove(item);
		}

		return self;
	}
}
