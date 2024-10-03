using JCNET.字符串处理;
using System.Text;

namespace JCNET.Lua;

/// <summary>
///		lua 代码内容
/// </summary>
public class LuaCodeContent
{
	/// <summary>
	///		lua 代码内容
	/// </summary>
	/// <param name="code"></param>
	public LuaCodeContent(string code)
	{
		_code = code;
		RequiredModuleSearchPaths = [];
	}

	/// <summary>
	///		lua 代码内容
	/// </summary>
	/// <param name="code"></param>
	/// <param name="required_module_search_paths"></param>
	public LuaCodeContent(string code, IEnumerable<string> required_module_search_paths)
	{
		_code = code;
		RequiredModuleSearchPaths = [.. required_module_search_paths];
	}

	private string _code = string.Empty;

	/// <summary>
	///		已经被展开的模块放到这里，避免重复展开。
	/// </summary>
	private HashSet<string> _expanded_modules = [];

	/// <summary>
	///		require 语句请求的模块的搜索路径。
	/// </summary>
	public HashSet<string> RequiredModuleSearchPaths { get; set; }

	/// <summary>
	///		解析出代码中含有哪些 require 语句，请求了哪些模块，并将 require 语句移除。
	/// </summary>
	/// <returns>返回 require 语句所请求的模块。</returns>
	private HashSet<string> ParseRequiredModule()
	{
		string? ParseFirstRequiredModule()
		{
			string module = _code.GetBetween(@"require(""", @""")").ToString();
			_code = _code.ReplaceWholeMatch($"require(\"{module}\")", string.Empty).ToString();
			if (module != string.Empty)
			{
				return module;
			}

			module = _code.GetBetween(@"require('", @"')").ToString();
			_code = _code.ReplaceWholeMatch($"require(\'{module}\')", string.Empty).ToString();
			if (module != string.Empty)
			{
				return module;
			}

			return null;
		}

		HashSet<string> required_modules = [];
		while (true)
		{
			string? module = ParseFirstRequiredModule();
			if (module is null)
			{
				return required_modules;
			}

			required_modules.Add(module);
		}
	}

	/// <summary>
	///		查找文件系统，如果存在该模块的文件，则打开并返回其内容，如果该模块不存在，
	///		返回 null。
	/// </summary>
	/// <param name="module"></param>
	/// <returns></returns>
	private string? GetRequiredModuleContent(string module)
	{
		string module_path = module.Replace('.', '/');
		foreach (string search_path in RequiredModuleSearchPaths)
		{
			string full_path = $"{search_path}/{module_path}.lua";
			if (!File.Exists(full_path))
			{
				continue;
			}

			using FileStream fs = File.OpenRead(full_path);
			using StreamReader sr = new(fs);
			return sr.ReadToEnd();
		}

		return null;
	}

	/// <summary>
	///		对 _code 执行一次 require 的扫描和展开。
	///		展开后的内容可能又含有 require，所以需要多次调用本函数。
	/// </summary>
	/// <returns>展开成功则返回 true。返回 false 表示展开失败，已经不再含有 require 语句了。</returns>
	/// <exception cref="Exception"></exception>
	private bool ExpandRequireOnce()
	{
		HashSet<string> modules = ParseRequiredModule();
		if (modules.Count == 0)
		{
			return false;
		}

		foreach (string module in modules)
		{
			if (!_expanded_modules.Add(module))
			{
				continue;
			}

			string? module_content = GetRequiredModuleContent(module);
			if (module_content is null)
			{
				throw new Exception($"无法找到模块：{module}");
			}

			_code = $"{module_content}\r\n{_code}";
		}

		return true;
	}

	/// <summary>
	///		将 require 语句展开。
	/// </summary>
	public void ExpandRequire()
	{
		while (true)
		{
			bool result = ExpandRequireOnce();
			if (!result)
			{
				return;
			}
		}
	}

	/// <summary>
	///		去除空行。
	/// </summary>
	/// <returns></returns>
	public string RemoveEmptyLine()
	{
		StringReader reader = new(_code);
		StringBuilder sb = new();
		while (true)
		{
			string? line = reader.ReadLine();
			if (line is null)
			{
				sb.AppendLine();
				return sb.ToString();
			}

			sb.AppendLine(line);
		}
	}

	/// <summary>
	///		获取 lua 代码内容字符串
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return _code;
	}
}
