using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoFile(@"F:\repos\lua_test\main.lua");
List<string> list = lua.GetCustomGlobalVariableNames();
foreach (string s in list)
{
	Console.WriteLine(s);
}

LuaTable table = lua.GetTable("G");
Dictionary<string, object> table_contents = lua.GetTableContents(table);
foreach (string s in table_contents.Keys)
{
	Console.WriteLine(s);
}
