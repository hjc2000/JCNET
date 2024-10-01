using JCNET.容器;
using NLua;

namespace JCNET.Lua;

/// <summary>
///		lua 扩展
/// </summary>
public static class LuaExtension
{
	/// <summary>
	///		获取 package.path 中的路径。这里面存放的是 require 函数搜索模块的路径。
	///		自定义的模块路径需要放在这里。
	/// </summary>
	/// <param path="self"></param>
	/// <returns></returns>
	public static string[] GetCustomRequireSearchPath(this NLua.Lua self)
	{
		string path = self.GetString("package.path");
		string[] paths = path.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		return paths;
	}

	/// <summary>
	///		获取一个 lua 表中的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="table"></param>
	/// <param name="path">table 的路径。会把路径拼接到 table 中的变量的路径前面。</param>
	/// <returns>字典。其中，键为访问该 lua 表中的值的路径，值就是对 lua 表中的值的引用。</returns>
	/// <exception cref="Exception"></exception>
	public static Dictionary<string, object> GetTableContents(this NLua.Lua self,
		NLua.LuaTable table, string path)
	{
		Dictionary<string, object> contents = [];
		Dictionary<object, object> dic = self.GetTableDict(table);
		foreach (KeyValuePair<object, object> pair in dic)
		{
			switch (pair.Key)
			{
			case long key:
				{
					contents[$"{path}[{key}]"] = pair.Value;
					break;
				}
			case string key:
				{
					contents[$"{path}.{key}"] = pair.Value;
					break;
				}
			default:
				{
					throw new Exception("不支持的 lua 表键类型");
				}
			}
		}

		return contents;
	}

	/// <summary>
	///		获取一个 lua 表中的内容。
	/// </summary>
	/// <param path="self"></param>
	/// <param path="table"></param>
	/// <returns>字典。其中，键为访问该 lua 表中的值的路径，值就是对 lua 表中的值的引用。</returns>
	public static Dictionary<string, object> GetTableContents(this NLua.Lua self, NLua.LuaTable table)
	{
		return self.GetTableContents(table, string.Empty);
	}

	/// <summary>
	///		递归地获取表的内容
	/// </summary>
	/// <param name="self"></param>
	/// <param name="table"></param>
	/// <param name="path"></param>
	/// <param name="table_id_set">
	///		遍历过的表的 ID 就放到这里，避免重复递归遍历一个已经遍历过的表，甚至在出现循环引用时
	///		导致无限递归。
	/// </param>
	/// <returns></returns>
	private static Dictionary<string, object> GetTableContentsRecurse(this NLua.Lua self,
		NLua.LuaTable table, HashSet<string> table_id_set, string path)
	{
		Dictionary<string, object> ret = [];
		Dictionary<string, object> contents = self.GetTableContents(table, path);
		foreach (KeyValuePair<string, object> pair in contents)
		{
			// 首先，获取到的表内容肯定是要放到 ret 中的
			ret.Add(pair.Key, pair.Value);
			if (pair.Value is not LuaTable sub_table)
			{
				// 如果该表内容不是一个表的话不需要进一步处理了
				continue;
			}

			bool add_result = table_id_set.Add(self.LuaObjToString(sub_table));
			if (!add_result)
			{
				// 已经在哈希表中了，说明已经遍历过了
				continue;
			}

			// 该 sub_table 还没被遍历过
			// 递归
			Dictionary<string, object> sub_contents = self.GetTableContentsRecurse(
				sub_table, table_id_set, pair.Key);

			ret.Add(sub_contents);
		}

		return ret;
	}

	/// <summary>
	///		递归地获取表的内容
	/// </summary>
	/// <param name="self"></param>
	/// <param name="table"></param>
	/// <param name="path"></param>
	/// <returns></returns>
	public static Dictionary<string, object> GetTableContentsRecurse(this NLua.Lua self,
		NLua.LuaTable table, string path)
	{
		return self.GetTableContentsRecurse(table, [], path);
	}

	/// <summary>
	///		获取 _G 表中的全局变量的路径。
	/// </summary>
	/// <param path="self"></param>
	/// <returns></returns>
	public static List<string> GetGlobalVariablePaths(this NLua.Lua self)
	{
		LuaTable table = self.GetTable("_G");
		Dictionary<string, object> dic = self.GetTableContents(table);
		List<string> paths = [.. dic.Keys];
		return paths;
	}

	/// <summary>
	///		获取 _G 表中的用户自定义全局变量的路径。
	///		<br/>* 所谓自定义，就是比 lua 解释器自带的多出来的部分
	/// </summary>
	/// <param path="self"></param>
	/// <returns></returns>
	public static List<string> GetCustomGlobalVariablePaths(this NLua.Lua self)
	{
		// 全新的空白的 lua 解释器对象。里面只有默认的，自带的全局变量
		NLua.Lua lua = new();

		// 提取默认的，自带的全局变量
		List<string> origin_global_variable_paths = lua.GetGlobalVariablePaths();

		// 提取 self 中的全局变量，然后排除掉 origin_global_variable_paths
		List<string> current_global_variable_paths = self.GetGlobalVariablePaths();
		foreach (string path in origin_global_variable_paths)
		{
			current_global_variable_paths.Remove(path);
		}

		return current_global_variable_paths;
	}

	/// <summary>
	///		调用 lua 的 tostring 函数，将指定路径的变量转化为字符串。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="path">要被转化为字符串的 lua 变量的路径。</param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static string LuaObjToString(this NLua.Lua self, string path)
	{
		object[] ret = self.DoString($"return tostring({path})");
		if (ret.Length == 0)
		{
			throw new Exception("lua 没有返回字符串");
		}

		if (ret[0] is not string str)
		{
			throw new Exception("lua 没有返回字符串");
		}

		return str;
	}

	/// <summary>
	///		调用 lua 的 tostring 函数，将指定路径的变量转化为字符串。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="lua_obj">要被转化为字符串的 lua 对象。</param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static string LuaObjToString(this NLua.Lua self, object lua_obj)
	{
		self.Push(lua_obj);
		NLua.LuaFunction func = self.GetFunction("tostring");
		object[] ret = func.Call(lua_obj);
		if (ret.Length == 0)
		{
			throw new Exception("lua 没有返回字符串");
		}

		if (ret[0] is not string str)
		{
			throw new Exception("lua 没有返回字符串");
		}

		return str;
	}
}
