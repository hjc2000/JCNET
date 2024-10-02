using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoFile(@"F:\repos\lua_test\main.lua");
Dictionary<string, object> global_variables = lua.GetCustomGlobalTableContentsRecurse();
foreach (KeyValuePair<string, object> pair in global_variables)
{
	Console.WriteLine($"{pair.Key}");
}
