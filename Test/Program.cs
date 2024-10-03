using JCNET.Lua;
using System.Text;

LuaWorkspace workspace = new("F:/repos/ElectricBatch");
StringBuilder sb = new();
sb.AppendLine(workspace.CollectOtherFileContents());
sb.AppendLine(workspace.GetMainFileContent());
LuaCodeContent content = new(sb.ToString());
Console.WriteLine(content);
