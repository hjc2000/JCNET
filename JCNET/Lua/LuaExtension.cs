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
}
