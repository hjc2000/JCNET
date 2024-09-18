using System.Globalization;
using System.Numerics;

namespace JCNET.字符串处理;

/// <summary>
///		进行字符串和数字之间的转换
/// </summary>
public static class NumberStringConverter
{
	#region 字符串转数字
	private static int ParseBase(string str)
	{
		if (str.StartsWith("0x"))
		{
			return 16;
		}

		return 10;
	}

	/// <summary>
	///		支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static sbyte ToInt8(this string str)
	{
		return Convert.ToSByte(str, ParseBase(str));
	}

	/// <summary>
	///		支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static byte ToUInt8(this string str)
	{
		return Convert.ToByte(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static short ToInt16(this string str)
	{
		return Convert.ToInt16(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static ushort ToUInt16(this string str)
	{
		return Convert.ToUInt16(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static int ToInt32(this string str)
	{
		return Convert.ToInt32(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static uint ToUInt32(this string str)
	{
		return Convert.ToUInt32(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static long ToInt64(this string str)
	{
		return Convert.ToInt64(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static ulong ToUInt64(this string str)
	{
		return Convert.ToUInt64(str, ParseBase(str));
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static double ToDouble(this string str)
	{
		return Convert.ToDouble(str);
	}

	/// <summary>
	/// 	支持 10 进制和 16 进制。16进制的数要以 0x 开头。
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static BigInteger ToBigInt(this string str)
	{
		if (str.StartsWith("0x"))
		{
			str = str[2..];
			return BigInteger.Parse(str, NumberStyles.HexNumber);
		}
		else
		{
			return BigInteger.Parse(str);
		}
	}
	#endregion

	#region 数字转 16 进制字符串
	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this sbyte value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this byte value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this short value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this ushort value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this int value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this uint value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this long value)
	{
		return $"0x{value:x}";
	}

	/// <summary>
	///		得到的 16 进制数字符串以 0x 开头。例如 0xffff
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ToHexString(this ulong value)
	{
		return $"0x{value:x}";
	}
	#endregion

	#region 整数转 bool
	/// <summary>
	///		0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this sbyte value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this byte value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this short value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this ushort value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this int value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this uint value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this long value)
	{
		return value != 0;
	}

	/// <summary>
	/// 	0 转成 false，其他转成 true。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool ToBool(this ulong value)
	{
		return value != 0;
	}
	#endregion

	#region bool 转整数
	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static sbyte ToInt8(this bool value)
	{
		return value ? (sbyte)1 : (sbyte)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static byte ToUInt8(this bool value)
	{
		return value ? (byte)1 : (byte)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static short ToInt16(this bool value)
	{
		return value ? (short)1 : (short)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static ushort ToUInt16(this bool value)
	{
		return value ? (ushort)1 : (ushort)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static int ToInt32(this bool value)
	{
		return value ? 1 : 0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static uint ToUInt32(this bool value)
	{
		return value ? 1 : (uint)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static long ToInt64(this bool value)
	{
		return value ? 1 : 0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static ulong ToUInt64(this bool value)
	{
		return value ? 1 : (ulong)0;
	}

	/// <summary>
	/// 	false 转成 0，true 转成 1。
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static BigInteger ToBigInt(this bool value)
	{
		return value ? 1 : 0;
	}
	#endregion
}
