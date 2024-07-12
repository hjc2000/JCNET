namespace JCNET.字符串处理;

/// <summary>
///		流式阅读字符串
/// </summary>
public class StringStreamReader
{
	public StringStreamReader(string str)
	{
		_str = str;
	}

	private int _pos = 0;

	/// <summary>
	///		当前读到字符串中的什么位置了
	/// </summary>
	public int Position
	{
		get
		{
			return _pos;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			if (value > _str.Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_pos = value;
		}
	}

	/// <summary>
	///		字符串的总长度
	/// </summary>
	public int Length
	{
		get
		{
			return _str.Length;
		}
	}

	/// <summary>
	///		剩余可读的长度
	/// </summary>
	/// <remarks>
	///		随着不断阅读，Position 不断推进，本属性会越来越小。
	/// </remarks>
	public int RemainLength
	{
		get
		{
			return _str.Length - _pos;
		}
	}

	private string _str;

	/// <summary>
	///		读取指定长度的字符，并推进流的位置。
	/// </summary>
	/// <param name="length"></param>
	/// <returns></returns>
	public string Read(int length)
	{
		string ret = _str[_pos..(_pos + length)];
		_pos += length;
		return ret;
	}
}
