namespace JCNET.字符串处理;

/// <summary>
///		字符串长度比较器
/// </summary>
public class StringLengthComparer : IComparer<string>
{
	/// <summary>
	///		字符串长度比较器
	/// </summary>
	/// <param name="order">排序方式</param>
	public StringLengthComparer(OrderEnum order)
	{
		_order = order;
	}

	/// <summary>
	///		排序方式
	/// </summary>
	public enum OrderEnum
	{
		/// <summary>
		///		从短到长排序
		/// </summary>
		FromShortToLong,

		/// <summary>
		///		从长到短排序
		/// </summary>
		FromLongToShort,
	}

	private OrderEnum _order = OrderEnum.FromShortToLong;

	/// <summary>
	///		从短到长排序
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	private int CompareFromShortToLong(string? x, string? y)
	{
		if (x is null && y is null)
		{
			return 0;
		}

		if (x is null && y is not null)
		{
			// 空视为比非空更短
			return -1;
		}

		if (x is not null && y is null)
		{
			return 1;
		}

		return x!.Length.CompareTo(y!.Length);
	}

	/// <summary>
	///		比较
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public int Compare(string? x, string? y)
	{
		if (_order == OrderEnum.FromShortToLong)
		{
			return CompareFromShortToLong(x, y);
		}

		return -CompareFromShortToLong(x, y);
	}
}
