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
	/// <param path="self"></param>
	/// <param path="table"></param>
	/// <returns>字典。其中，键为访问该 lua 表中的值的路径，值就是对 lua 表中的值的引用。</returns>
	public static Dictionary<string, object> GetTableContents(this NLua.Lua self, NLua.LuaTable table)
	{
		Dictionary<string, object> contents = [];
		Dictionary<object, object> dic = self.GetTableDict(table);
		foreach (KeyValuePair<object, object> pair in dic)
		{
			switch (pair.Key)
			{
			case long key:
				{
					contents[$"[{key}]"] = pair.Value;
					break;
				}
			case string key:
				{
					contents[$".{key}"] = pair.Value;
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
}
