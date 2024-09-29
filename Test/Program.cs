using JCNET.Lua;
using NLua;

Lua lua = new();
string[] paths = lua.GetCustomRequireSearchPath();
foreach (string path in paths)
{
	Console.WriteLine(path);
}
