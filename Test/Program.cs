using JCNET.Lua;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
LuaCodeContent content = workspace.ToSingleLua();
Console.WriteLine(content);
