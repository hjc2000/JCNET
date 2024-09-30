using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoFile(@"F:\repos\lua_test\main.lua");
List<string> list = lua.GetGlobalVariablePaths();
foreach (string s in list)
{
	Console.WriteLine(s);
}

