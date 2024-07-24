namespace JCNET.数学;

/// <summary>
///		表示 Base * 10 ^ Exponent 这种科学计数法的数
///		
///		<br/>*	在值等于 0 时，认为有效数字始终是 1 位。但是，在测量中，量纲为 m 的时候，
///				(0 * 10 ^ 0) m 和 (0 * 10 ^ -3) m 是有区别的。前者表示在 m 的精度上
///				测量结果是 0，后者表示在 mm 精度上测量，结果仍然是 0.
///				这个时候本类的有效数字位数就变成用来指示精度量级。例如 (0 * 10 ^ -3) 对应
///				SignificantDigits = 3.
/// </summary>
public class ScientificNotation
{
	/// <summary>
	///		无参构造函数。
	///		构造出来表示 0 * 10 ^ 0
	/// </summary>
	public ScientificNotation()
	{
		SignificantDigits = 0;
		Base = 0;
		Exponent = 0;
	}

	/// <summary>
	///		构造函数
	/// </summary>
	/// <param name="num">值</param>
	/// <param name="significant_digits">有效数字位数</param>
	public ScientificNotation(double num, int significant_digits)
	{
		SignificantDigits = significant_digits;
		if (num == 0)
		{
			Base = 0;
			Exponent = 0;
			return;
		}

		Exponent = (int)System.Math.Log10(num);
		Base = num / System.Math.Pow(10, Exponent);

		// 经过上面的计算后，Base 一定是绝对值小于 10 的。
		// 规范化的科学计数法要求基数的绝对值大于等于 1，除非整个数确切地是 0.
		while (System.Math.Abs(Base) < 1)
		{
			Exponent--;
			Base *= 10;
		}
	}

	/// <summary>
	///		基数
	/// </summary>
	public double Base { get; }

	/// <summary>
	///		10 的指数
	/// </summary>
	public int Exponent { get; }

	/// <summary>
	///		有效数字位数。
	/// </summary>
	public int SignificantDigits { get; }

	/// <summary>
	///		转化为 double
	/// </summary>
	/// <returns></returns>
	public double ToDouble()
	{
		return Base * System.Math.Pow(10, Exponent);
	}

	/// <summary>
	///		转化为类似 0.1 * 10 ^ 1 这样的字符串。
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		if (Base == 0)
		{
			return $"{Base} * 10 ^ {-SignificantDigits}";
		}

		string base_str = Base.ToString();

		// 规范化的科学计数法中，有效长度，即去掉了小数点后的长度，就会等于有效数字位数。
		int EffectiveLength()
		{
			if (base_str.Contains('.'))
			{
				return base_str.Length - 1;
			}

			return base_str.Length;
		}

		if (EffectiveLength() < SignificantDigits)
		{
			// 在末尾补上 0 之前，如果原来没有小数点，需要补上小数点
			if (!base_str.Contains('.'))
			{
				base_str += ".";
			}

			base_str = base_str.PadRight(base_str.Length + SignificantDigits - EffectiveLength(), '0');
		}
		else if (EffectiveLength() > SignificantDigits)
		{
			base_str = base_str[0..(SignificantDigits + 1)];
			if (base_str.EndsWith('.'))
			{
				base_str = base_str[0..(base_str.Length - 1)];
			}
		}

		return $"{base_str} * 10 ^ {Exponent}";
	}
}
