namespace JCNET.Math;

public class Range<T> where T : IComparable<T>
{
	public Range() { }

	/// <summary>
	///		左区间是开区间
	/// </summary>
	public bool LeftIsOpen { get; set; } = false;

	/// <summary>
	///		右区间是开区间
	/// </summary>
	public bool RightIsOpen { get; set; } = false;

	public T Left { get; set; } = default!;
	public T Right { get; set; } = default!;

	public bool OutOfRange(T value)
	{
		if (LeftIsOpen && value.CompareTo(Left) == 0)
		{
			return true;
		}

		if (RightIsOpen && value.CompareTo(Right) == 0)
		{
			return true;
		}

		// 排除了开区间时在区间左右端点的情况后，就按照闭区间的标准去衡量是否超出范围。
		return value.CompareTo(Left) < 0 || value.CompareTo(Right) > 0;
	}
}
