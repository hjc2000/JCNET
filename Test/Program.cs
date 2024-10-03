using JCNET.Lua;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
foreach (string path in workspace.LuaFilePaths)
{
	Console.WriteLine(path);
}

Console.WriteLine($"main 文件的路径：{workspace.MainFilePath}");
