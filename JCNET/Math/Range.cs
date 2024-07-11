using System.Numerics;

namespace JCNET.Math;

public class Range<T> where T : IComparisonOperators<T, T, bool>
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
		if (LeftIsOpen && value == Left)
		{
			return true;
		}

		if (RightIsOpen && value == Right)
		{
			return true;
		}

		// 排除了开区间时在区间左右端点的情况后，就按照闭区间的标准去衡量是否超出范围。
		return value < Left || value > Right;
	}

	public bool InRange(T value)
	{
		return !OutOfRange(value);
	}
}
