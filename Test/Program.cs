using JCNET.Lua;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
LuaWorkspaceContent workspace_content = workspace.Content;
Console.WriteLine(workspace_content.SigleContent);
