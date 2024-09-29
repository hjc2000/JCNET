﻿using NLua;

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
	/// <param name="self"></param>
	/// <returns></returns>
	public static string[] GetCustomRequireSearchPath(this NLua.Lua self)
	{
		string path = self.GetString("package.path");
		string[] paths = path.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		return paths;
	}

	/// <summary>
	///		获取全局变量名列表。
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static List<string> GetGlobalVariableNames(this NLua.Lua self)
	{
		LuaTable table = self.GetTable("_G");
		Dictionary<object, object> dic = self.GetTableDict(table);
		List<string> key_lists = [];
		foreach (string key in dic.Keys)
		{
			key_lists.Add(key.ToString());
		}

		return key_lists;
	}

	/// <summary>
	///		获取自定义的全局变量的变量名列表。
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static List<string> GetCustomGlobalVariableNames(this NLua.Lua self)
	{
		/* 所谓自定义，就是比 lua 解释器自带的多出来的部分，所以新建一个干净的 Lua 解释器对象，
		 * 获取其中的全局变量名，然后获取 self 中的全局变量名，将 self 的全局变量名剔除掉干净
		 * 的 Lua 解释器对象中的全局变量名，就得到了自定义的全局变量名。
		 */
		NLua.Lua lua = new();
		List<string> origin_global_variable_names = lua.GetGlobalVariableNames();
		List<string> current_global_variable_names = self.GetGlobalVariableNames();
		foreach (string name in origin_global_variable_names)
		{
			current_global_variable_names.Remove(name);
		}

		return current_global_variable_names;
	}
}
