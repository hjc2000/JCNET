using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoFile(@"F:\repos\lua_test\main.lua");
LuaTable table = lua.GetTable("_G");
Dictionary<string, object> contents = lua.GetSubTables(table, string.Empty);
foreach (KeyValuePair<string, object> pair in contents)
{
	Console.WriteLine(pair.Key);
}
