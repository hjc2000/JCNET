using JCNET.Lua;
using NLua;

List<string> required_module_search_paths = [
	@"F:/repos/lua_test",
];

LuaCodeContent lua_code_content = new(@"require(""main"")", required_module_search_paths);
Console.WriteLine(lua_code_content);
lua_code_content.ExpandRequire();

Lua lua = new();
lua.DoString(lua_code_content.ToString());
Dictionary<string, object> global_variables = lua.GetCustomGlobalTableContentsRecurse();
foreach (KeyValuePair<string, object> pair in global_variables)
{
	Console.WriteLine($"{pair.Key}");
}
