using System.Numerics;

namespace JCNET.字符串处理;

/// <summary>
///		纯粹的基于字符串的，URL 的路径。<br/>
///		所谓 URL 的路径即：
///			<br/>* 只包含路径部分，不包含前面的协议，主机名，端口号等。从根路径的那个斜杠开始。
///			<br/>* 不包含查询参数等。
/// </summary>
public class UrlPath
	: IComparisonOperators<UrlPath, UrlPath, bool>
{
	/// <summary>
	///		传入字符串的路径。
	/// </summary>
	/// <param name="path">
	///		路径。
	///		<br/> 传进来后，会将 \\ , // , \ 等路径分隔符全部替换为 /。
	///	</param>
	public UrlPath(string path)
	{
		Path = path;
		Path = Path.Replace(@"\", "/");
		Path = Path.Replace(@"\\", "/");
		Path = Path.Replace(@"//", "/");

		if (Path.StartsWith("./"))
		{
			Path = Path["./".Length..];
		}
	}

	/// <summary>
	///		构造函数。
	/// </summary>
	/// <param name="uri"></param>
	public UrlPath(Uri uri)
	{
		Path = uri.AbsolutePath;
	}

	/// <summary>
	///		本对象表示的路径。
	///		<br/>* 可能是相对路径，也可能是绝对路径。
	/// </summary>
	private string Path { get; }

	/// <summary>
	///		本对象表示的路径是否是绝对路径。
	///		<br/>* 判断依据为是否以 / 开头。
	/// </summary>
	public bool IsAbsolutePath
	{
		get
		{
			return Path.StartsWith('/');
		}
	}

	#region 相等性
	/// <summary>
	///		当 left 大于 right 时，表示 right 是 left 的子路径。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator >(UrlPath left, UrlPath right)
	{
		return !(left <= right);
	}

	/// <summary>
	///		当 left 大于等于 right 时，表示 right 是 left 的子路径，或者两个路径相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator >=(UrlPath left, UrlPath right)
	{
		return !(left < right);
	}

	/// <summary>
	///		当 left 小于 right 时，表示 left 是 right 的子路径。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator <(UrlPath left, UrlPath right)
	{
		if (left.IsAbsolutePath != right.IsAbsolutePath)
		{
			throw new InvalidOperationException("不能将绝对路径与相对路径比较");
		}

		if (!left.Path.StartsWith(right.Path))
		{
			// left 不以 right 开头，一切都免谈
			return false;
		}

		if (left == right)
		{
			return false;
		}

		/* 处理
		 *		left = /path1/path2 ，right = /path1
		 * 即 left 以 right 开头，并且 right 的部分结束后紧跟着一个 /
		 * 
		 * left = /path1/ , right = /path1 这种情况已经先被相等比较的处理了，返回了 false。
		 * 所以 left 在 right 的部分结束后紧接着的 / 后面一定是还有字符的。
		 */
		if (left.Path[right.Path.Length] == '/')
		{
			return true;
		}

		/* 处理
		 *		left = /path1，right = /
		 *		left = /path1/path2，right = /path1/
		 * 即 left 以 right 开头，right 以 / 结尾。
		 */
		if (right.Path.EndsWith('/'))
		{
			return true;
		}

		/* 处理
		 *		left = path1 ，right = ""
		 * 即 right 为空字符串，left 不为空。
		 * 因为 right 为空，表示的是当前路径，所以任何当前路径下的内容都是子路径，除了当前路径本身。
		 */
		if (right.Path == string.Empty)
		{
			return left.Path != string.Empty;
		}

		return false;
	}

	/// <summary>
	///		当 left 小于等于 right 时，表示 left 是 right 的子路径，或者两个路径相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator <=(UrlPath left, UrlPath right)
	{
		if (left == right)
		{
			return true;
		}

		return left < right;
	}

	/// <summary>
	///		相等运算符。
	///		只有 2 个本类对象表示的路径字符串相等时才认为相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator ==(UrlPath? left, UrlPath? right)
	{
		if ((left is null) && (right is null))
		{
			return true;
		}

		if (left is null)
		{
			return false;
		}

		if (right is null)
		{
			return false;
		}

		if (left.IsAbsolutePath != right.IsAbsolutePath)
		{
			throw new InvalidOperationException("不能将绝对路径与相对路径比较");
		}

		if (left.Path == right.Path)
		{
			return true;
		}

		if (left.Path.Length + 1 == right.Path.Length)
		{
			return $"{left.Path}/" == right.Path;
		}
		else if (left.Path.Length == right.Path.Length + 1)
		{
			return left.Path == $"{right.Path}/";
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	///		不等运算符。
	///		只有 2 个本类对象表示的路径字符串相等时才认为相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator !=(UrlPath? left, UrlPath? right)
	{
		return !(left == right);
	}

	/// <summary>
	///		重载的相等比较。
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public override bool Equals(object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (obj is not UrlPath)
		{
			return false;
		}

		return this == (obj as UrlPath);
	}

	/// <summary>
	///		获取基于路径字符串计算的哈希码。
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode()
	{
		return Path.GetHashCode();
	}
	#endregion

	/// <summary>
	///		返回 本对象表示的路径。
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return Path;
	}
}
