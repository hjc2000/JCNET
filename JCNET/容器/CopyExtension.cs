namespace JCNET.容器;

public static class CopyExtension
{
	/// <summary>
	///		将数据从源复制到列表中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static List<T> CopyToList<T>(this IEnumerable<T> source)
	{
		List<T> new_list = [];
		foreach (T item in source)
		{
			new_list.Add(item);
		}

		return new_list;
	}

	/// <summary>
	///		将数据从源复制到数组中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static T[] CopyToArray<T>(this IEnumerable<T> source)
	{
		T[] array = new T[source.LongCount()];
		long index = 0;
		foreach (T item in source)
		{
			array[index++] = item;
		}

		return array;
	}

	/// <summary>
	///		将数据从源复制到哈希表中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static HashSet<T> CopyToHashSet<T>(this IEnumerable<T> source)
	{
		HashSet<T> set = [];
		foreach (T item in source)
		{
			set.Add(item);
		}

		return set;
	}

	/// <summary>
	///		将数据从源复制到队列中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static Queue<T> CopyToQueue<T>(this IEnumerable<T> source)
	{
		Queue<T> queue = new();
		foreach (T item in source)
		{
			queue.Enqueue(item);
		}

		return queue;
	}
}
