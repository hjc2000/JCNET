using JCNET.字符串处理;
using System.Text;

namespace JCNET.Lua;

/// <summary>
///		lua 工作区
/// </summary>
public class LuaWorkspace
{
	/// <summary>
	///		lua 工作区
	/// </summary>
	/// <param name="path">工作区路径</param>
	public LuaWorkspace(string path)
	{
		_wordspace_dir = new StringPath(path);
	}

	/// <summary>
	///		工作区路径
	/// </summary>
	private StringPath _wordspace_dir;

	/// <summary>
	///		获取工作区中的 lua 文件。除了 out 目录下的文件和 ${工作区根目录}/main.lua
	/// </summary>
	public IEnumerable<string> LuaFilePaths
	{
		get
		{
			EnumerationOptions options = new()
			{
				RecurseSubdirectories = true,
			};

			IEnumerable<string> paths = Directory.EnumerateFiles(_wordspace_dir.ToString(),
				"*", options);

			List<string> result = [];
			foreach (string path in paths)
			{
				string corrcted_path = StringPath.CorrectPath(path);
				string? directory = Path.GetDirectoryName(corrcted_path);
				if (directory is null)
				{
					continue;
				}

				if (directory.EndsWith("out"))
				{
					// out 目录下的文件不被收集
					continue;
				}

				if (corrcted_path == (_wordspace_dir + "main.lua").ToString())
				{
					continue;
				}

				string? extension = Path.GetExtension(corrcted_path);
				if (extension is null || extension != ".lua")
				{
					// 排除不是 .lua 的文件
					continue;
				}

				result.Add(corrcted_path);
			}

			return result;
		}
	}

	/// <summary>
	///		获取 main.lua 的路径。
	/// </summary>
	public string MainFilePath
	{
		get
		{
			return (_wordspace_dir + "main.lua").ToString();
		}
	}

	/// <summary>
	///		require 语句请求的模块的搜索路径。
	/// </summary>
	public HashSet<string> RequiredModuleSearchPaths { get; set; } = [];

	/// <summary>
	///		根据本工作区，创建一个新的 LuaWorkspaceContent。
	///		<br/>* 会将 RequiredModuleSearchPaths 复制给 LuaWorkspaceContent 中的 SigleContent。
	/// </summary>
	public LuaWorkspaceContent GetContent()
	{
		LuaWorkspaceContent content = new(GetMainFileContent(),
			CollectOtherFileContents());

		foreach (string path in RequiredModuleSearchPaths)
		{
			content.SigleContent.RequiredModuleSearchPaths.Add(path);
		}

		return content;
	}

	/// <summary>
	///		创建一个新的 lua 虚拟机并返回。
	///		<br/>* 会将本工作区的 RequiredModuleSearchPaths 传递给 lua 虚拟机。
	/// </summary>
	/// <returns></returns>
	public NLua.Lua GetLuaVm()
	{
		NLua.Lua lua = new();
		foreach (string path in RequiredModuleSearchPaths)
		{
			lua.AddCustomRequireSearchPath(path);
		}

		return lua;
	}

	/// <summary>
	///		收集 LuaFilePaths 中指示的所有文件，变成单个字符串。
	/// </summary>
	/// <returns></returns>
	private string CollectOtherFileContents()
	{
		StringBuilder sb = new();
		foreach (string path in LuaFilePaths)
		{
			using FileStream fs = File.OpenRead(path);
			using StreamReader sr = new(fs);
			sb.AppendLine(sr.ReadToEnd());
		}

		return sb.ToString();
	}

	/// <summary>
	///		获取 ${工作区根目录}/main.lua 的内容。
	/// </summary>
	/// <returns></returns>
	private string GetMainFileContent()
	{
		using FileStream fs = File.OpenRead(MainFilePath);
		using StreamReader sr = new(fs);
		return sr.ReadToEnd();
	}

	/// <summary>
	///		将 lua 代码内容字符串写入输出文件 ${工作区目录}/out/out.lua 中。
	///		<br/>* out.lua 始终是新建的空白文件。
	/// </summary>
	/// <param name="lua_code_content"></param>
	public void Output(string lua_code_content)
	{
		string out_dir = (_wordspace_dir + "out").ToString();
		if (!Directory.Exists(out_dir))
		{
			Directory.CreateDirectory(out_dir);
		}

		using FileStream out_file = File.Open($"{out_dir}/out.lua",
			FileMode.Create,
			FileAccess.ReadWrite,
			FileShare.Read);

		using StreamWriter writer = new(out_file);
		writer.Write(lua_code_content);
	}
}

/// <summary>
///		lua 工作区内容。
/// </summary>
public class LuaWorkspaceContent
{
	/// <summary>
	///		lua 工作区内容。
	/// </summary>
	/// <param name="main_file_content"></param>
	/// <param name="other_file_content"></param>
	public LuaWorkspaceContent(string main_file_content, string other_file_content)
	{
		MainFileContent = main_file_content;
		OtherFileContents = other_file_content;

		StringBuilder content_builder = new();
		content_builder.AppendLine(other_file_content);
		content_builder.AppendLine(main_file_content);
		SigleContent = new LuaCodeContent(content_builder.ToString());
	}

	/// <summary>
	///		LuaFilePaths 中指示的文件的内容被收集到本属性中。
	/// </summary>
	public string OtherFileContents { get; } = string.Empty;

	/// <summary>
	///		${工作区根目录}/main.lua 的内容被收集到这里。
	/// </summary>
	public string MainFileContent { get; set; } = string.Empty;

	/// <summary>
	///		整个工作区的 lua 文件整合成的单个 LuaCodeContent
	/// </summary>
	public LuaCodeContent SigleContent { get; }
}
