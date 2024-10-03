using JCNET.Lua;
using NLua;

LuaCodeContent lua_code_content = new(
	@"require(""main"")",
	[@"F:/repos/lua_test"]);
lua_code_content.ExpandRequire();

Lua lua = new();
lua.DoString(lua_code_content.ToString());
Dictionary<string, object> global_variables = lua.GetCustomGlobalTableContentsRecurse();
foreach (KeyValuePair<string, object> pair in global_variables)
{
	Console.WriteLine($"{pair.Key}");
}
