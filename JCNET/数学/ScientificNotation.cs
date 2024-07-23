namespace JCNET.数学;

/// <summary>
///		表示 Base * 10 ^ Exponent 这种科学计数法的数
/// </summary>
public class ScientificNotation
{
	public ScientificNotation()
	{
		Base = 0;
		Exponent = 0;
		SignificantDigits = 1;
	}

	public ScientificNotation(double num, int significant_digits)
	{
		if (num == 0)
		{
			Base = 0;
			Exponent = 0;
			SignificantDigits = 1;
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

		SignificantDigits = significant_digits;
	}

	public double Base { get; }
	public int Exponent { get; }

	/// <summary>
	///		有效数字位数。
	/// </summary>
	public int SignificantDigits { get; }

	public double ToDouble()
	{
		return Base * System.Math.Pow(10, Exponent);
	}

	public override string ToString()
	{
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
