using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoFile(@"F:\repos\lua_test\main.lua");
LuaTable table = lua.GetTable("_G");
Dictionary<string, object> contents = lua.GetTableContentsRecurse(table, string.Empty);
foreach (KeyValuePair<string, object> pair in contents)
{
	Console.WriteLine(pair.Key);
}

Console.WriteLine($"表转为字符串：{lua.LuaObjToString(table)}");
