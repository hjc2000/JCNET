namespace JCNET.字符串处理;

/// <summary>
///		切割字符串的扩展
/// </summary>
public static class CutExtension
{
	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this ReadOnlyMemory<char> self, ReadOnlySpan<char> middle)
	{
		if (self.Length == 0 || middle.Length == 0)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		int index = self.IndexOf(middle);
		if (index == -1)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		return new CuttingMiddleResult()
		{
			Success = true,
			Left = self[..index],
			Right = self[(index + middle.Length)..]
		};
	}

	/// <summary>
	///		将字符串切除中间部分，留下左边和右边的部分。
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddle(this string self, ReadOnlySpan<char> middle)
	{
		return self.AsMemory().CutMiddle(middle);
	}

	/// <summary>
	///		全字匹配地切除中间部分
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this ReadOnlyMemory<char> self,
		ReadOnlySpan<char> middle)
	{
		if (self.Length == 0 || middle.Length == 0)
		{
			return new CuttingMiddleResult()
			{
				Success = false,
			};
		}

		int finding_offset = 0;
		while (true)
		{
			if (finding_offset >= self.Length)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			int index = self.IndexOf(middle, finding_offset);
			if (index == -1)
			{
				return new CuttingMiddleResult()
				{
					Success = false,
				};
			}

			// 找到了，需要进一步验证是否全字匹配
			if (index > 0)
			{
				// middle 不是在开头位置，需要检查前一个字符
				if (!self.Span[index - 1].IsWordSeperation())
				{
					// 前一个字符不是分隔符，不满足全字匹配
					finding_offset = index + middle.Length;
					continue;
				}
			}

			if (index + middle.Length < self.Length)
			{
				// middle 不是在结尾位置，需要检查后一个字符
				if (!self.Span[index + middle.Length].IsWordSeperation())
				{
					// 后一个字符不是分隔符，不满足全字匹配
					finding_offset = index + middle.Length;
					continue;
				}
			}

			// 经过验证，是全字匹配的
			return new CuttingMiddleResult()
			{
				Success = true,
				Left = self[..index],
				Right = self[(index + middle.Length)..],
			};
		}
	}

	/// <summary>
	///		全字匹配地切除中间部分
	///		<br/>* middle 如果为空字符串，则会导致切除中间失败，这根本没法切。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="middle"></param>
	/// <returns></returns>
	public static CuttingMiddleResult CutMiddleWholeMatch(this string self,
		ReadOnlySpan<char> middle)
	{
		return self.AsMemory().CutMiddleWholeMatch(middle);
	}
}

/// <summary>
///		将字符串切除中间部分后的结果。
/// </summary>
public class CuttingMiddleResult
{
	/// <summary>
	///		是否切除成功。
	///		<br/>* 如果中间部分不存在就会切除失败
	/// </summary>
	public bool Success { get; set; } = false;

	/// <summary>
	///		切除中间部分后留下的左边部分
	/// </summary>
	public ReadOnlyMemory<char> Left { get; set; } = new Memory<char>();

	/// <summary>
	///		切除中间部分后留下的右边部分
	/// </summary>
	public ReadOnlyMemory<char> Right { get; set; } = new Memory<char>();

	/// <summary>
	///		转化为字符串
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return Json.ToJson(this);
	}
}
