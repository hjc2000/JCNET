using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace JCNET.Math;

/// <summary>
///		分数类
/// </summary>
public readonly struct Fraction : IComparisonOperators<Fraction, Fraction, bool>
{
	public Fraction() { }

	public Fraction(BigInteger num)
	{
		Num = num;
		Den = 1;
	}

	public Fraction(BigInteger num, BigInteger den)
	{
		if (den == 0)
		{
			throw new ArgumentException("分母不能为 0");
		}

		Num = num;
		Den = den;
	}

	/// <summary>
	///		使用字符串进行初始化。
	/// </summary>
	/// <remarks>
	///		传进来的字符串可以是分数的字符串，也可以是整型或浮点的字符串，例如：
	///			"5/2", "5", "5.12"
	///		这些都是合法的。
	/// </remarks>
	/// <param name="str"></param>
	public Fraction(string str)
	{
		int index = str.IndexOf('/');
		if (index == 0)
		{
			// 第 1 个字符就是 / 号
			Num = 0;
			Den = 1;
			return;
		}

		if (index > 0)
		{
			// 存在 / 号，且不是第 1 个字符
			if (str.EndsWith('/'))
			{
				throw new ArgumentException("不能以 '/' 结尾");
			}

			string num_string = str[..index];
			string den_string = str[(index + 1)..];
			Num = BigInteger.Parse(num_string);
			Den = BigInteger.Parse(den_string);
			if (Den == 0)
			{
				throw new ArgumentException("分母不能为 0");
			}

			return;
		}

		// 不包含 / 号，推测传进来的是浮点字符串或整数字符串
		index = str.IndexOf('.');
		if (index < 0)
		{
			// 传进来的是整型字符串
			Num = BigInteger.Parse(str);
			Den = 1;
			return;
		}

		// 传进来的是浮点字符串
		// 计算小数点后有多少位
		int count = str.Length - 1 - index;
		str = str.Remove(index, 1);
		Num = BigInteger.Parse(str);
		Den = BigInteger.Pow(10, count);
	}

	public BigInteger Num { get; } = 0;
	public BigInteger Den { get; } = 1;

	/// <summary>
	///		化简。返回化简后的新分数对象。
	/// </summary>
	/// <returns></returns>
	public Fraction Simplify()
	{
		BigInteger gcd = BigInteger.GreatestCommonDivisor(Num, Den);
		BigInteger num = Num / gcd;
		BigInteger den = Den / gcd;
		if (Den < 0)
		{
			num = -num;
			den = -den;
		}

		return new Fraction(num, den);
	}

	/// <summary>
	///		求负
	/// </summary>
	/// <param name="fraction1"></param>
	/// <returns></returns>
	public static Fraction operator -(Fraction fraction1)
	{
		Fraction ret = new(-fraction1.Num, fraction1.Den);
		return ret.Simplify();
	}

	/// <summary>
	///		加法
	/// </summary>
	/// <param name="fraction1"></param>
	/// <param name="fraction2"></param>
	/// <returns></returns>
	public static Fraction operator +(Fraction fraction1, Fraction fraction2)
	{
		// 两个分数的分母的最小公倍数
		BigInteger den_lcm = fraction1.Den *
			fraction2.Den /
			BigInteger.GreatestCommonDivisor(fraction1.Den, fraction2.Den);

		// 分子放大与分母相同的倍数
		BigInteger num1 = fraction1.Num * (den_lcm / fraction1.Den);
		BigInteger num2 = fraction2.Num * (den_lcm / fraction2.Den);

		Fraction ret = new(num1 + num2, den_lcm);
		return ret.Simplify();
	}

	/// <summary>
	///		减法
	/// </summary>
	/// <param name="fraction1"></param>
	/// <param name="fraction2"></param>
	/// <returns></returns>
	public static Fraction operator -(Fraction fraction1, Fraction fraction2)
	{
		Fraction ret = fraction1 + (-fraction2);
		return ret.Simplify();
	}

	/// <summary>
	///		乘法
	/// </summary>
	/// <param name="fraction1"></param>
	/// <param name="fraction2"></param>
	/// <returns></returns>
	public static Fraction operator *(Fraction fraction1, Fraction fraction2)
	{
		BigInteger num = fraction1.Num * fraction2.Num;
		BigInteger den = fraction1.Den * fraction2.Den;
		Fraction ret = new(num, den);
		return ret.Simplify();
	}

	/// <summary>
	///		除法
	/// </summary>
	/// <param name="fraction1"></param>
	/// <param name="fraction2"></param>
	/// <returns></returns>
	public static Fraction operator /(Fraction fraction1, Fraction fraction2)
	{
		Fraction ret = fraction1 * fraction2.Reciprocal;
		return ret.Simplify();
	}

	#region 比较运算符
	public static bool operator <(Fraction fraction1, Fraction fraction2)
	{
		return fraction1.Num * fraction2.Den < fraction2.Num * fraction1.Den;
	}

	public static bool operator >(Fraction fraction1, Fraction fraction2)
	{
		return fraction1.Num * fraction2.Den > fraction2.Num * fraction1.Den;
	}

	public static bool operator <=(Fraction fraction1, Fraction fraction2)
	{
		return fraction1.Num * fraction2.Den <= fraction2.Num * fraction1.Den;
	}

	public static bool operator >=(Fraction fraction1, Fraction fraction2)
	{
		return fraction1.Num * fraction2.Den >= fraction2.Num * fraction1.Den;
	}

	public static bool operator ==(Fraction fraction1, Fraction fraction2)
	{
		Fraction f1 = fraction1.Simplify();
		Fraction f2 = fraction2.Simplify();
		return f1.Num == f2.Num && f1.Den == f2.Den;
	}

	public static bool operator !=(Fraction fraction1, Fraction fraction2)
	{
		return !(fraction1 == fraction2);
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (obj is not Fraction)
		{
			return false;
		}

		return this == (Fraction)obj;
	}

	public override int GetHashCode()
	{
		return Num.GetHashCode() ^ Den.GetHashCode();
	}
	#endregion

	/// <summary>
	///		倒数
	/// </summary>
	public Fraction Reciprocal
	{
		get
		{
			if (Num == 0)
			{
				throw new InvalidOperationException("分子为 0，不允许取倒数。");
			}

			return new Fraction(Den, Num);
		}
	}

	/// <summary>
	///		分子除以分母的结果
	/// </summary>
	public BigInteger Div
	{
		get
		{
			return Num / Den;
		}
	}
	/// <summary>
	///		分子除以分母的余数
	/// </summary>
	public BigInteger Mod
	{
		get
		{
			return Num % Den;
		}
	}

	public override string ToString()
	{
		return $"{Num} / {Den}";
	}
}
