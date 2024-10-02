using JCNET.字符串处理;
using JCNET.容器;
using NLua;

namespace JCNET.Lua;

/// <summary>
///		lua 扩展
/// </summary>
public static class LuaExtension
{
	#region require
	/// <summary>
	///		调用 lua 的 require 函数。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="module_path">
	///		这里的路径并不是指文件系统路径，而是模块路径。
	///		例如 A 目录添加到 require 搜索目录了，然后 A/B/my_module.lua 是你的模块，
	///		那么 module_path 就是 B.my_module
	///	</param>
	public static void DoRequire(this NLua.Lua self, string module_path)
	{
		LuaFunction func = self.GetFunction("require");
		func.Call(module_path);
	}

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
	///		设置 package.path。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="path"></param>
	public static void SetCustomRequireSearchPath(this NLua.Lua self, string path)
	{
		path = path.Replace('\\', '/').Replace("//", "/");
		self.DoString($"package.path={path}");
	}

	/// <summary>
	///		向 package.path 添加内容。
	/// </summary>
	/// <param name="self"></param>
	/// <param name="path"></param>
	public static void AddCustomRequireSearchPath(this NLua.Lua self, string path)
	{
		string old_path = self.GetString("package.path");
		if (!old_path.EndsWith(';'))
		{
			old_path += ";";
		}

		path = $"{old_path}{path}";
		path = path.Replace('\\', '/').Replace("//", "/");
		if (!path.EndsWith('/'))
		{
			path += "/";
		}

		self["package.path"] = $"{path}?.lua";
	}
	#endregion

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
			CuttingMiddleResult cut_result = pair.Key.CutMiddleWholeMatch("_G");
			if (cut_result.Success)
			{
				continue;
			}

			if (pair.Key.StartsWith(".package.loaded."))
			{
				continue;
			}

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
	///		获取全局变量
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static Dictionary<string, object> GetGlobalVariables(this NLua.Lua self)
	{
		Dictionary<string, object> global_varialbes = self.GetTableContents(self.GetTable("_G"));
		return global_varialbes;
	}

	/// <summary>
	///		获取 lua 默认的，自带的全局变量。
	/// </summary>
	/// <returns></returns>
	public static Dictionary<string, object> GetDefaultGlobalVariables()
	{
		NLua.Lua lua = new();
		return lua.GetGlobalVariables();
	}

	/// <summary>
	///		获取 _G 表中的用户自定义的全局变量。
	///		<br/>* 所谓自定义，就是比 lua 解释器自带的多出来的部分
	/// </summary>
	/// <param path="self"></param>
	/// <returns></returns>
	public static Dictionary<string, object> GetCustomGlobalVariables(this NLua.Lua self)
	{
		Dictionary<string, object> default_global_variables = GetDefaultGlobalVariables();
		Dictionary<string, object> current_global_variables = self.GetGlobalVariables();
		current_global_variables.Remove(default_global_variables);
		return current_global_variables;
	}

	/// <summary>
	///		递归地获取全局变量中的表的内容。
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static Dictionary<string, object> GetCustomGlobalTableContentsRecurse(this NLua.Lua self)
	{
		Dictionary<string, object> ret = [];
		Dictionary<string, object> custom_global_variables = self.GetCustomGlobalVariables();
		HashSet<string> table_id_set = [];
		foreach (KeyValuePair<string, object> pair in custom_global_variables)
		{
			if (pair.Value is not LuaTable table)
			{
				continue;
			}

			ret.Add(pair.Key, pair.Value);
			Dictionary<string, object> contents = self.GetTableContentsRecurse(table, table_id_set, pair.Key);
			ret.Add(contents);
		}

		return ret;
	}

	#region ToString
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
	#endregion

}
