using System.Numerics;

namespace JCNET.数学;

public class Range<T> where T : IComparisonOperators<T, T, bool>
{
	public Range() { }

	public Range(T left, T right)
	{
		Left = left;
		Right = right;
	}

	public Range(T left, T right, bool leftIsOpen, bool rightIsOpen)
	{
		LeftIsOpen = leftIsOpen;
		RightIsOpen = rightIsOpen;
		Left = left;
		Right = right;
	}

	public T Left { get; set; } = default!;
	public T Right { get; set; } = default!;

	/// <summary>
	///		左区间是开区间
	/// </summary>
	public bool LeftIsOpen { get; set; } = false;

	/// <summary>
	///		右区间是开区间
	/// </summary>
	public bool RightIsOpen { get; set; } = false;

	/// <summary>
	///		是否超出范围
	/// </summary>
	/// <param name="value"></param>
	/// <returns>超出范围返回 true，没超出则返回 false。</returns>
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

	/// <summary>
	///		是否在范围内
	/// </summary>
	/// <param name="value"></param>
	/// <returns>在范围内返回 true，不在范围内返回 false。</returns>
	public bool InRange(T value)
	{
		return !OutOfRange(value);
	}
}
