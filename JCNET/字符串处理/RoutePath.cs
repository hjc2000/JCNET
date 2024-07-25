using System.Numerics;

namespace JCNET.字符串处理;

/// <summary>
///		路由路径。
/// </summary>
public class RoutePath
	: IComparisonOperators<RoutePath, RoutePath, bool>
{
	/// <summary>
	///		构造函数。
	///		将路由路径的绝对 URL 构造成 Uri 对象传进来。
	/// </summary>
	/// <param name="uri"></param>
	public RoutePath(Uri uri)
	{
		Path = uri.AbsolutePath;
		Path = Path.Replace("//", "/");
		if (Path.EndsWith('/'))
		{
			Path = Path[..(Path.Length - 1)];
		}
	}

	/// <summary>
	///		本对象表示的路径。
	/// </summary>
	private string Path { get; }

	#region 相等性
	/// <summary>
	///		当 left 大于 right 时，表示 right 是 left 的子路径。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator >(RoutePath left, RoutePath right)
	{
		return !(left <= right);
	}

	/// <summary>
	///		当 left 大于等于 right 时，表示 right 是 left 的子路径，或者两个路径相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator >=(RoutePath left, RoutePath right)
	{
		return !(left < right);
	}

	/// <summary>
	///		当 left 小于 right 时，表示 left 是 right 的子路径。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator <(RoutePath left, RoutePath right)
	{
		if (!left.Path.StartsWith(right.Path))
		{
			// left 不以 right 开头，一切都免谈
			return false;
		}

		if (left == right)
		{
			return false;
		}

		/* 对于路由，不认为 /page 属于 / 的子路径，而是认为它们是同一级别的，都是 1 级路由。
		 * 因为 blazor 初始时显示 / 的页面，然后 /page 等导航到其它页面。/page/sub_page
		 * 这种算作 2 级路由。
		 * 
		 * 根路径 / 传进来后，经过构造函数处理，会变成空字符串。
		 */
		if (right.Path == string.Empty && left.Path != string.Empty)
		{
			return false;
		}

		if (left.Path[right.Path.Length] == '/')
		{
			return true;
		}

		return false;
	}

	/// <summary>
	///		当 left 小于等于 right 时，表示 left 是 right 的子路径，或者两个路径相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator <=(RoutePath left, RoutePath right)
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
	public static bool operator ==(RoutePath? left, RoutePath? right)
	{
		if (left is null)
		{
			return false;
		}

		if (right is null)
		{
			return false;
		}

		return left.Path == right.Path;
	}

	/// <summary>
	///		不等运算符。
	///		只有 2 个本类对象表示的路径字符串相等时才认为相等。
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static bool operator !=(RoutePath? left, RoutePath? right)
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

		if (obj is not RoutePath)
		{
			return false;
		}

		return this == (obj as RoutePath);
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
