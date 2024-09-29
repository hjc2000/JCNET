using JCNET.Lua;
using NLua;

Lua lua = new();
List<string> list = lua.GetGlobalVariableNames();
foreach (string s in list)
{
	Console.WriteLine(s);
}
