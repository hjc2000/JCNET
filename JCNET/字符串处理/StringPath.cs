using System.Numerics;

namespace JCNET.字符串处理;

/// <summary>
///		字符串路径
/// </summary>
public class StringPath :
	IAdditionOperators<StringPath, StringPath, StringPath>,
	IAdditionOperators<StringPath, string, StringPath>
{
	/// <summary>
	///		字符串路径
	/// </summary>
	/// <param name="path"></param>
	public StringPath(string path)
	{
		_path = CorrectPath(path);
	}

	private readonly string _path;

	/// <summary>
	///		更正路径字符串为规范形式。
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string CorrectPath(string path)
	{
		path = path.Replace('\\', '/');
		while (path.Contains("//"))
		{
			path = path.Replace("//", "/");
		}

		if (path.EndsWith("/"))
		{
			path = path[..(path.Length - 1)];
		}

		if (path.StartsWith("./"))
		{
			path = path["./".Length..];
		}

		return path;
	}

	/// <summary>
	///		获取路径字符串。
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return _path;
	}

	/// <summary>
	///		本路径是不是绝对路径。
	/// </summary>
	public bool IsAbsolutePath
	{
		get
		{
			if (_path.StartsWith("/"))
			{
				return true;
			}

			if (_path.Length > 2 && _path[1] == ':' && _path[2] == '/')
			{
				return true;
			}

			return false;
		}
	}

	/// <summary>
	///		将相对路径 right 加到绝对路径 left 上。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static StringPath operator +(StringPath left, StringPath right)
	{
		if (!left.IsAbsolutePath)
		{
			throw new ArgumentException("左操作数必须是绝对路径");
		}

		if (right.IsAbsolutePath)
		{
			throw new ArgumentException("右操作数必须是相对路径");
		}

		return new StringPath($"{left._path}/{right._path}");
	}

	/// <summary>
	///		将相对路径 right 加到绝对路径 left 上。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static StringPath operator +(StringPath left, string right)
	{
		return left + new StringPath(right);
	}
}
