using JCNET.字符串处理;

namespace JCNET.Lua;

/// <summary>
///		lua 工作区
/// </summary>
public class LuaWorkspace
{
	/// <summary>
	///		lua 工作区
	/// </summary>
	/// <param name="path"></param>
	public LuaWorkspace(string path)
	{
		_path = new StringPath(path);
	}

	private StringPath _path;

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

			IEnumerable<string> paths = Directory.EnumerateFiles(_path.ToString(),
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

				if (corrcted_path == (_path + "main.lua").ToString())
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
			return (_path + "main.lua").ToString();
		}
	}
}
