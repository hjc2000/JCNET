using JCNET.Lua;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
LuaWorkspaceContent workspace_content = workspace.GetContent();
Console.WriteLine(workspace_content.SigleContent);
