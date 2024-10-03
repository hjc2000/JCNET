using JCNET.Lua;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
foreach (string path in workspace.LuaFiles)
{
	Console.WriteLine(path);
}
