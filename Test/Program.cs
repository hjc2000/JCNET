using JCNET.Lua;
using NLua;

Lua lua = new();
lua.DoString(@"
	Servo = {}
");
List<string> list = lua.GetCustomGlobalVariableNames();
foreach (string s in list)
{
	Console.WriteLine(s);
}

object num = lua.DoString(@"
	return 0;
")[0];
Console.WriteLine(num.GetType());
