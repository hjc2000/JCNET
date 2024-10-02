using JCNET.Lua;
using NLua;

Lua lua = new();
lua.AddCustomRequireSearchPath(@"F:/repos/lua_test");
string[] require_search_paths = lua.GetCustomRequireSearchPath();
foreach (string path in require_search_paths)
{
	Console.WriteLine(path);
}

lua.DoRequire("main");
Dictionary<string, object> global_variables = lua.GetCustomGlobalTableContentsRecurse();
foreach (KeyValuePair<string, object> pair in global_variables)
{
	Console.WriteLine($"{pair.Key}");
}
