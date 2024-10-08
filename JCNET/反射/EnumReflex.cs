﻿namespace JCNET.反射;

/// <summary>
///		枚举类型的反射
/// </summary>
public static class EnumReflex
{
	/// <summary>
	///		将枚举类转化为字典。字典键为枚举值，字典值为枚举值的名称。
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	/// <returns></returns>
	public static Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : Enum
	{
		Dictionary<TEnum, string> values = Enum.GetValues(typeof(TEnum))
			.Cast<TEnum>()
			.ToDictionary(
				(TEnum e) =>
				{
					return e;
				},
				(TEnum e) =>
				{
					return e.ToString();
				}
			);

		return values;
	}
}
